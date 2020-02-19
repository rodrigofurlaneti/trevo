using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;
using System.Linq;
using Entidade.Uteis;
using System;
using Entidade.Base;
using Core.Extensions;

namespace Dominio
{
    public interface IPedidoLocacaoServico : IBaseServico<PedidoLocacao>
    {

        IList<PedidoLocacao> ListarPedidoLocacaoFiltro(int? idUnidade, int? idTipoLocacao);
        void RemoveLancamentosAdicional(int id);

        void AdicionarPedidoLocacaoAosLancamentosAdicionais(PedidoLocacao pedidoLocacao);
        PedidoLocacao SalvarComRetorno(PedidoLocacao pedidoLocacao, int usuarioId);
        void AtualizarStatus(int idNotificacao, Usuario usuario, AcaoNotificacao acao);
        void SalvarPedidoLocacaoNotificacao(PedidoLocacao pedidoLocacao, Notificacao notificacao);
        PedidoLocacaoNotificacao BuscarPedidoLocacaoNotificacao(int notificacaoId);
        void LancarCobrancas(PedidoLocacao pedidoLocacao);
        Notificacao SalvarNotificacaoComRetorno(PedidoLocacao pedidoLocacao, int idUsuario, string urlPersonalizada = "");
        bool LiberarControles(int Id);
    }

    public class PedidoLocacaoServico : BaseServico<PedidoLocacao, IPedidoLocacaoRepositorio>, IPedidoLocacaoServico
    {
        private readonly IUnidadeRepositorio _unidadeRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly ITipoLocacaoRepositorio _tipoLocacaoRepositorio;
        private readonly IDescontoRepositorio _descontoRepositorio;
        private readonly IPedidoLocacaoLancamentoAdicionalRepositorio _pedidoLocacaoLancamentoAdicionalRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPedidoLocacaoNotificacaoRepositorio _pedidoLocacaoNotificacaoRepositorio;
        private readonly ILancamentoCobrancaRepositorio _lancamentoCobrancaRepositorio;
        private readonly IContaFinanceiraRepositorio _contaFinanceiraRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IPedidoLocacaoLancamentoCobrancaRepositorio _pedidoLocacaoLancamentoCobrancaRepositorio;
        private readonly IPedidoLocacaoRepositorio _pedidoLocacaoRepositorio;

        public PedidoLocacaoServico(IPedidoLocacaoRepositorio pedidoLocacaoRepositorio,
            IUnidadeRepositorio unidadeRepositorio,
            IClienteRepositorio clienteRepositorio,
            ITipoLocacaoRepositorio tipoLocacaoRepositorio,
            IDescontoRepositorio descontoRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            IPedidoLocacaoNotificacaoRepositorio pedidoLocacaoNotificacaoRepositorio,
            IPedidoLocacaoLancamentoAdicionalRepositorio pedidoLocacaoLancamentoAdicionalRepositorio,
            ILancamentoCobrancaRepositorio lancamentoCobrancaRepositorio,
            IContaFinanceiraRepositorio contaFinanceiraRepositorio,
            ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
            INotificacaoRepositorio notificacaoRepositorio,
            IPedidoLocacaoLancamentoCobrancaRepositorio pedidoLocacaoLancamentoCobrancaRepositorio
            )
        {
            _pedidoLocacaoRepositorio = pedidoLocacaoRepositorio;
            _unidadeRepositorio = unidadeRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _tipoLocacaoRepositorio = tipoLocacaoRepositorio;
            _descontoRepositorio = descontoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _pedidoLocacaoNotificacaoRepositorio = pedidoLocacaoNotificacaoRepositorio;
            _pedidoLocacaoLancamentoAdicionalRepositorio = pedidoLocacaoLancamentoAdicionalRepositorio;
            _lancamentoCobrancaRepositorio = lancamentoCobrancaRepositorio;
            _contaFinanceiraRepositorio = contaFinanceiraRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
            _pedidoLocacaoLancamentoCobrancaRepositorio = pedidoLocacaoLancamentoCobrancaRepositorio;
        }

        public void AdicionarPedidoLocacaoAosLancamentosAdicionais(PedidoLocacao pedidoLocacao)
        {
            foreach (var item in pedidoLocacao.PedidoLocacaoLancamentosAdicionais)
            {
                item.AssociarPedidoLocacao(pedidoLocacao);
            }
        }

        public PedidoLocacao SalvarComRetorno(PedidoLocacao pedidoLocacao, int usuarioId)
        {
            if (pedidoLocacao.Id <= 0)
                pedidoLocacao.Ativo = true;

            pedidoLocacao.Unidade = _unidadeRepositorio.GetById(pedidoLocacao.Unidade.Id);
            pedidoLocacao.Cliente = _clienteRepositorio.GetById(pedidoLocacao.Cliente.Id);
            pedidoLocacao.TipoLocacao = _tipoLocacaoRepositorio.GetById(pedidoLocacao.TipoLocacao.Id);
            pedidoLocacao.Desconto = pedidoLocacao.Desconto != null && pedidoLocacao.Desconto.Id > 0 ? _descontoRepositorio.GetById(pedidoLocacao.Desconto.Id) : null;
            pedidoLocacao.PedidoLocacaoNotificacoes = pedidoLocacao.PedidoLocacaoNotificacoes ?? new List<PedidoLocacaoNotificacao>();
            AdicionarPedidoLocacaoAosLancamentosAdicionais(pedidoLocacao);
            var id = Repositorio.SaveAndReturn(pedidoLocacao);

            return Repositorio.GetById(id);
        }

        public void AtualizarStatus(int idNotificacao, Usuario usuario, AcaoNotificacao acao)
        {
            var notificacao = _notificacaoRepositorio.GetById(idNotificacao);
            var pedidoLocacaoNotificacao = BuscarPedidoLocacaoNotificacao(idNotificacao);
            var pedidoLocacao = _pedidoLocacaoRepositorio.GetById(pedidoLocacaoNotificacao.PedidoLocacao.Id);

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    {
                        pedidoLocacaoNotificacao.Aprovar(usuario);

                        notificacao.DataAprovacao = DateTime.Now;
                        notificacao.Aprovador = pedidoLocacaoNotificacao.Notificacao.Aprovador;
                        notificacao.Status = pedidoLocacaoNotificacao.Notificacao.Status;
                        _notificacaoRepositorio.Save(notificacao);

                        if (pedidoLocacaoNotificacao.PossuiNotificacaoDeDesconto())
                        {
                            var notificacaoPedido = SalvarNotificacaoComRetorno(pedidoLocacao, usuario.Id);
                            SalvarPedidoLocacaoNotificacao(pedidoLocacao, notificacaoPedido);
                        }
                        else
                        {
                            LancarCobrancas(pedidoLocacao);
                        }
                    }
                    break;
                case AcaoNotificacao.Reprovado:
                    pedidoLocacaoNotificacao.Reprovar(usuario);

                    notificacao.Aprovador = pedidoLocacaoNotificacao.Notificacao.Aprovador;
                    notificacao.Status = pedidoLocacaoNotificacao.Notificacao.Status;
                    _notificacaoRepositorio.Save(notificacao);
                    break;
                default:
                    break;
            }
            
            pedidoLocacao.Status = pedidoLocacaoNotificacao.PedidoLocacao.Status;
            _pedidoLocacaoRepositorio.Save(pedidoLocacao);
        }

        public void SalvarPedidoLocacaoNotificacao(PedidoLocacao pedidoLocacao, Notificacao notificacao)
        {
            var pedidoLocacaoNotificacao = new PedidoLocacaoNotificacao
            {
                PedidoLocacao = pedidoLocacao,
                Notificacao = notificacao
            };

            _pedidoLocacaoNotificacaoRepositorio.Save(pedidoLocacaoNotificacao);
        }

        public IList<PedidoLocacao> ListarPedidoLocacaoFiltro(int? idUnidade, int? idTipoLocacao)
        {
            return _pedidoLocacaoRepositorio.ListarPedidoLocacaoFiltro(idUnidade, idTipoLocacao);
        }

        public void RemoveLancamentosAdicional(int id)
        {
             _pedidoLocacaoLancamentoAdicionalRepositorio.DeleteById(id);
        }

        public Notificacao SalvarNotificacaoComRetorno(PedidoLocacao pedidoLocacao, int idUsuario, string urlPersonalizada = "")
        {
            var usuario = new Usuario { Id = idUsuario };
            return _notificacaoRepositorio.SalvarNotificacaoComRetorno(pedidoLocacao, Entidades.PedidoLocacao, usuario, pedidoLocacao.DataVencimentoNotificacao, "", urlPersonalizada);
        }

        public PedidoLocacaoNotificacao BuscarPedidoLocacaoNotificacao(int notificacaoId)
        {
            return _pedidoLocacaoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == notificacaoId);
        }

        public void LancarCobrancas(PedidoLocacao pedidoLocacao)
        {
            var lancamentoCobrancas = new List<LancamentoCobranca>();

            var contaPadrao = _contaFinanceiraRepositorio.FirstBy(x => x.ContaPadrao);

            lancamentoCobrancas.Add(CriarPrimeiraCobranca(pedidoLocacao, contaPadrao));

            if(pedidoLocacao.PossuiCicloMensal)
                lancamentoCobrancas.AddRange(CriarCobrancasPeloCicloMensal(pedidoLocacao, contaPadrao));
            else
                lancamentoCobrancas.AddRange(CriarCobrancasPeloCicloEmDias(pedidoLocacao, contaPadrao));


            _lancamentoCobrancaRepositorio.Save(lancamentoCobrancas);
            SalvarPedidoLocacaoLancamentoCobrancas(pedidoLocacao, lancamentoCobrancas);
        }

        private LancamentoCobranca CriarPrimeiraCobranca(PedidoLocacao pedidoLocacao, ContaFinanceira conta)
        {
            return new LancamentoCobranca
            {
                Unidade = pedidoLocacao.Unidade,
                Cliente = pedidoLocacao.Cliente,
                ContaFinanceira = conta,
                TipoServico = TipoServico.Locacao,
                DataGeracao = DateTime.Now,
                DataVencimento = pedidoLocacao.DataPrimeiroPagamento,
                DataCompetencia = new DateTime(pedidoLocacao.DataPrimeiroPagamento.Year, pedidoLocacao.DataPrimeiroPagamento.Month, 1),
                StatusLancamentoCobranca = StatusLancamentoCobranca.EmAberto,
                ValorContrato = pedidoLocacao.ValorPrimeiroPagamento
            };
        }

        private List<LancamentoCobranca> CriarCobrancasPeloCicloMensal(PedidoLocacao pedidoLocacao, ContaFinanceira conta)
        {
            var lancamentoCobrancas = new List<LancamentoCobranca>();

            var totalMeses = pedidoLocacao.DataDemaisPagamentos.DiferencaDeMeses(pedidoLocacao.DataVigenciaFim);
            var mesInicioReajuste = pedidoLocacao.DataVigenciaInicio.DiferencaDeMeses(pedidoLocacao.DataReajuste);
            var reajusteMeses = (int)pedidoLocacao.PrazoReajuste;
            var valor = pedidoLocacao.Valor;
            var valorTotal = pedidoLocacao.ValorTotal;

            for (int i = 0; i < totalMeses; i++)
            {
                if(i >= mesInicioReajuste && i % reajusteMeses == 0)
                {
                    if (pedidoLocacao.TipoReajuste == TipoReajuste.Monetario)
                        valor = valor + pedidoLocacao.ValorReajuste;
                    else
                        valor += (valor * pedidoLocacao.ValorReajuste) / 100;

                    valorTotal = valor + pedidoLocacao.PedidoLocacaoLancamentosAdicionais.Sum(x => x.Valor);
                }

                lancamentoCobrancas.Add(new LancamentoCobranca
                {
                    Unidade = pedidoLocacao.Unidade,
                    Cliente = pedidoLocacao.Cliente,
                    ContaFinanceira = conta,
                    TipoServico = TipoServico.Locacao,
                    DataGeracao = DateTime.Now,
                    DataVencimento = pedidoLocacao.DataDemaisPagamentos.AddMonths(i),
                    DataCompetencia = new DateTime(pedidoLocacao.DataDemaisPagamentos.AddMonths(i).Year, pedidoLocacao.DataDemaisPagamentos.AddMonths(i).Month, 1),
                    StatusLancamentoCobranca = StatusLancamentoCobranca.EmAberto,
                    ValorContrato = valorTotal
                });
            }

            return lancamentoCobrancas;
        }

        private List<LancamentoCobranca> CriarCobrancasPeloCicloEmDias(PedidoLocacao pedidoLocacao, ContaFinanceira conta)
        {
            var lancamentoCobrancas = new List<LancamentoCobranca>();

            var totalDias = (pedidoLocacao.DataVigenciaFim - pedidoLocacao.DataDemaisPagamentos).Days;
            var diasCorridos = 1;
            var ciclo = pedidoLocacao.CicloPagamentos;
            while (diasCorridos < totalDias)
            {
                var diasAteVencer = totalDias - diasCorridos;
                var valorTotal = pedidoLocacao.ValorTotal;
                diasCorridos += ciclo;
                if (ciclo > diasAteVencer)
                {
                    diasCorridos = totalDias;
                    valorTotal = (diasAteVencer * valorTotal) / ciclo;
                }

                lancamentoCobrancas.Add(new LancamentoCobranca
                {
                    Unidade = pedidoLocacao.Unidade,
                    Cliente = pedidoLocacao.Cliente,
                    ContaFinanceira = conta,
                    TipoServico = TipoServico.Locacao,
                    DataGeracao = DateTime.Now,
                    DataVencimento = pedidoLocacao.DataDemaisPagamentos.AddDays(diasCorridos),
                    DataCompetencia = new DateTime(pedidoLocacao.DataDemaisPagamentos.AddDays(diasCorridos).Year, pedidoLocacao.DataDemaisPagamentos.AddDays(diasCorridos).Month, 1),
                    StatusLancamentoCobranca = StatusLancamentoCobranca.EmAberto,
                    ValorContrato = valorTotal
                });
            }

            return lancamentoCobrancas;
        }

        private void SalvarPedidoLocacaoLancamentoCobrancas(PedidoLocacao pedidoLocacao, List<LancamentoCobranca> lancamentoCobrancas)
        {
            var pedidoLocacaoLancamentoCobrancas = new List<PedidoLocacaoLancamentoCobranca>();
            foreach (var cobranca in lancamentoCobrancas)
            {
                pedidoLocacaoLancamentoCobrancas.Add(new PedidoLocacaoLancamentoCobranca
                {
                    PedidoLocacao = pedidoLocacao,
                    LancamentoCobranca = cobranca
                });
            }
            _pedidoLocacaoLancamentoCobrancaRepositorio.Save(pedidoLocacaoLancamentoCobrancas);
        }

        public bool LiberarControles(int Id)
        {
            var notificacoes = _pedidoLocacaoNotificacaoRepositorio.List().Where(x => x.Notificacao.TipoNotificacao.Entidade == Entidades.PedidoLocacao);

            var objeto = notificacoes.Where(x => x.PedidoLocacao.Id == Id && x.Notificacao.Status == StatusNotificacao.Aguardando).Select(x => x.Notificacao);

            if(objeto.Any())
            {
                return true;
            }

            return false;
        }
    }
}