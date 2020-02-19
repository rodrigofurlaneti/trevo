using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Practices.ServiceLocation;

namespace Dominio
{
    public interface IPedidoSeloServico : IBaseServico<PedidoSelo>
    {
        void Desbloquear(PedidoSelo pedidoSelo, int idUsuario);
        void Bloquear(PedidoSelo pedidoSelo, int idUsuario);
        int Salvar(PedidoSelo pedidoSelo, int? idUsuario = null, AcaoWorkflowPedido? acao = null, string descricaoHistorico = null);
        void ClonarPedido(PedidoSelo pedidoSelo, int idUsuario);
        void CancelarPedido(PedidoSelo pedidoSelo, int idUsuario);
        IList<PedidoSelo> ConsultaPedidosPorStatus(int statusid);
        IList<PedidoSelo> ListaPedidosSelo(int? idPedidoSelo, int? idCliente, int? idConvenio, int? idUnidade, int? idTipoSelo, DateTime? validade, int? idTipoPagamento);
        Dictionary<int, bool> EmailForamEnviados(List<int> listaIdPedido);
        string[] DescriptografarChave(string chave);
        void AdicionarEmail(PedidoSelo pedido, int? idUsuario, string email, bool enviadoComSucesso, TipoEnvioEmailPedidoSelo tipo, string descricao, DateTime? dataAtual = null);
        void EnviaEmailPropostaAprovada(PedidoSelo pedido, int idUsuario, Stream stream);
        void EnviaEmailPropostaReprovada(PedidoSelo pedido, int idUsuario);
        List<PedidoSelo> BuscarAprovadosPeloCliente();
        decimal CalculaValorLancamentoCobranca(PedidoSelo pedido);
        void GerarBoleto(int idPedido, int idUsuario);
        IList<PedidoSelo> BuscarPedidosSeloAprovados(int? clienteId, int? convenioId, int? unidadeId, int? tipoSeloId, TipoPagamentoSelo? tipoPagamento);
        decimal RetornaValorSeloComDesconto(PedidoSelo pedidoSelo);
        bool ExisteSelosAtivo(PedidoSelo pedidoSelo);
    }

    public class PedidoSeloServico : BaseServico<PedidoSelo, IPedidoSeloRepositorio>, IPedidoSeloServico
    {
        private readonly IEmissaoSeloRepositorio _emissaoSeloRepositorio;
        private readonly IPropostaRepositorio _propostaRepositorio;
        private readonly IDescontoRepositorio _negociacaoSeloDescontoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly IPedidoSeloNotificacaoRepositorio _pedidoSeloNotificacaoRepositorio;
        private readonly INegociacaoSeloDescontoNotificacaoRepositorio _negociacaoSeloDescontoNotificacaoRepositorio;
        private readonly IPedidoSeloRepositorio _pedidoSeloRepositorio;
        private readonly ILancamentoCobrancaPedidoSeloServico _lancamentoCobrancaPedidoSeloServico;
        private readonly INotificacaoAvisoPedidoSeloServico _notificacaoAvisoPedidoSeloServico;
        private readonly IGeradorPdfServico _geradorPdfServico;
        private readonly ITabelaPrecoAvulsoRepositorio _tabelaPrecoAvulsaRepositorio;
        private readonly IPrecoParametroSeloRepositorio _precoParametroSeloRepositorio;
        private readonly IPedidoSeloHistoricoRepositorio _pedidoSeloHistoricoRepositorio;
        private readonly IPedidoSeloEmailRepositorio _pedidoSeloEmailRepositorio;

        public PedidoSeloServico(
            IEmissaoSeloRepositorio emissaoSeloRepositorio,
            IPropostaRepositorio propostaRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
            IDescontoRepositorio negociacaoSeloDescontoRepositorio,
            IPedidoSeloNotificacaoRepositorio pedidoSeloNotificacaoRepositorio,
            INegociacaoSeloDescontoNotificacaoRepositorio negociacaoSeloDescontoNotificacaoRepositorio,
            IPedidoSeloRepositorio pedidoSeloRepositorio,
            ILancamentoCobrancaPedidoSeloServico lancamentoCobrancaPedidoSeloServico,
            INotificacaoAvisoPedidoSeloServico notificacaoAvisoPedidoSeloServico,
            IGeradorPdfServico geradorPdfServico,
            ITabelaPrecoAvulsoRepositorio tabelaPrecoAvulsaRepositorio,
            IPrecoParametroSeloRepositorio precoParametroSeloRepositorio,
            IPedidoSeloHistoricoRepositorio pedidoSeloHistoricoRepositorio,
            IPedidoSeloEmailRepositorio pedidoSeloEmailRepositorio)
        {
            _emissaoSeloRepositorio = emissaoSeloRepositorio;
            _propostaRepositorio = propostaRepositorio;
            _negociacaoSeloDescontoRepositorio = negociacaoSeloDescontoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _pedidoSeloNotificacaoRepositorio = pedidoSeloNotificacaoRepositorio;
            _negociacaoSeloDescontoNotificacaoRepositorio = negociacaoSeloDescontoNotificacaoRepositorio;
            _pedidoSeloRepositorio = pedidoSeloRepositorio;
            _lancamentoCobrancaPedidoSeloServico = lancamentoCobrancaPedidoSeloServico;
            _notificacaoAvisoPedidoSeloServico = notificacaoAvisoPedidoSeloServico;
            _geradorPdfServico = geradorPdfServico;
            _tabelaPrecoAvulsaRepositorio = tabelaPrecoAvulsaRepositorio;
            _precoParametroSeloRepositorio = precoParametroSeloRepositorio;
            _pedidoSeloHistoricoRepositorio = pedidoSeloHistoricoRepositorio;
            _pedidoSeloEmailRepositorio = pedidoSeloEmailRepositorio;
        }

        public int Salvar(PedidoSelo pedidoSelo, int? idUsuario, AcaoWorkflowPedido? acao = null, string descricaoHistorico = null)
        {
            if (pedidoSelo == null)
                throw new BusinessRuleException("Pedido não encontrado");

            if (pedidoSelo.StatusPedido != StatusPedidoSelo.Rascunho && acao == null && pedidoSelo.TipoPedidoSelo == TipoPedidoSelo.Emissao)
                throw new BusinessRuleException("O pedido não pode ser alterado após solicitar aprovação");

            if (!idUsuario.HasValue || idUsuario.Value <= 0)
                throw new BusinessRuleException("Usuário logado inválido");

            if (acao.HasValue)
                Workflow(pedidoSelo, acao.Value, idUsuario, descricaoHistorico);
            else
                Workflow(pedidoSelo, null, idUsuario, descricaoHistorico);

            return _pedidoSeloRepositorio.SaveAndReturn(pedidoSelo);
        }

        private void Workflow(PedidoSelo pedido, AcaoWorkflowPedido? acao, int? idUsuario, string descricaoHistorico = null)
        {
            try
            {
                if (acao == AcaoWorkflowPedido.Cancelar)
                {
                    EfetuaCancelamento(pedido);
                    WorkflowCancelamento(pedido, idUsuario, descricaoHistorico);
                    return;
                }

                if (pedido.TipoPedidoSelo == TipoPedidoSelo.Emissao)
                    WorkflowNovaEmissao(pedido, acao, idUsuario, descricaoHistorico);
                else if (pedido.TipoPedidoSelo == TipoPedidoSelo.Bloqueio)
                    WorkflowBloqueio(pedido, idUsuario, descricaoHistorico);
                else if (pedido.TipoPedidoSelo == TipoPedidoSelo.Desbloqueio)
                    WorkflowDesbloqueio(pedido, acao, idUsuario, descricaoHistorico);
            }
            catch (BusinessRuleException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EfetuaCancelamento(PedidoSelo pedido)
        {
            foreach (var lancamentoCobranca in pedido.LancamentoCobranca)
            {
                if (lancamentoCobranca.StatusLancamentoCobranca != StatusLancamentoCobranca.Pago)
                    lancamentoCobranca.StatusLancamentoCobranca = StatusLancamentoCobranca.ACancelar;
            }
        }

        private void WorkflowCancelamento(PedidoSelo pedido, int? idUsuario, string descricaoHistorico = null)
        {
            var listaStatusQueNaoPodemSerCancelados = new List<StatusPedidoSelo>
            {
                StatusPedidoSelo.Cancelado
            };

            if (listaStatusQueNaoPodemSerCancelados.Contains(pedido.StatusPedido))
            {
                throw new BusinessRuleException($"O pedido não pode ser cancelado no status \"{pedido.StatusPedido.ToDescription()}\"");
            }
            else
            {
                AdicionarHistorico(pedido, StatusPedidoSelo.Cancelado, idUsuario, descricaoHistorico);
                _notificacaoAvisoPedidoSeloServico.Criar(pedido, idUsuario.Value);
            }

            return;
        }

        private void WorkflowDesbloqueio(PedidoSelo pedido, AcaoWorkflowPedido? acao, int? idUsuario, string descricaoHistorico = null)
        {
            if (pedido.StatusPedido != StatusPedidoSelo.AprovadoPeloCliente)
                throw new BusinessRuleException("Não foi possível solicitar o desbloqueio da emissão de selo, pois é necessário que o pedido esteja aprovado pelo cliente");

            if (pedido.EmissaoSelo == null || pedido.EmissaoSelo.Id == 0)
                throw new BusinessRuleException("Não foi possível solicitar o desbloqueio da emissão de selo, pois é necessário que o pedido tenha uma emissão realizada");

            if (!acao.HasValue && pedido.EmissaoSelo.StatusSelo != StatusSelo.Bloqueado)
                throw new BusinessRuleException("Não foi possível solicitar o desbloqueio da emissão de selo, pois é necessário que a emissão esteja bloqueada");

            if (acao.HasValue && acao.Value == AcaoWorkflowPedido.Aprovar && pedido.EmissaoSelo.StatusSelo != StatusSelo.AguardandoDesbloqueio)
                throw new BusinessRuleException("Não foi possível concluir o desbloqueio da emissão de selo, pois é necessário que a emissão esteja aguardando o desbloqueio");

            if (acao == null)
            {
                //Validar se já possui notificação
                _pedidoSeloNotificacaoRepositorio.CriarNotificacaoBloqueio(pedido, idUsuario.Value);
                var emissao = _emissaoSeloRepositorio.GetById(pedido.EmissaoSelo.Id);
                emissao.StatusSelo = StatusSelo.AguardandoDesbloqueio;
                emissao.UsuarioAlteracaoStatus = _usuarioRepositorio.GetById(idUsuario.Value);
                _emissaoSeloRepositorio.Save(emissao);

                pedido.TipoPedidoSelo = TipoPedidoSelo.Desbloqueio;
            }
            else if (acao == AcaoWorkflowPedido.Aprovar)
            {
                var emissao = _emissaoSeloRepositorio.GetById(pedido.EmissaoSelo.Id);
                emissao.StatusSelo = StatusSelo.Ativo;
                emissao.UsuarioAlteracaoStatus = _usuarioRepositorio.GetById(idUsuario.Value);
                _emissaoSeloRepositorio.Save(emissao);

                pedido.TipoPedidoSelo = TipoPedidoSelo.Emissao;
                //Integração soft park
            }
        }

        private void WorkflowBloqueio(PedidoSelo pedido, int? idUsuario, string descricaoHistorico = null)
        {
            if (pedido.StatusPedido != StatusPedidoSelo.AprovadoPeloCliente)
                throw new BusinessRuleException("Não foi possível bloquear uma emissão de selo, pois é necessário que o pedido esteja aprovado pelo cliente");

            if (pedido.EmissaoSelo == null || pedido.EmissaoSelo.Id == 0)
                throw new BusinessRuleException("Não foi possível bloquear uma emissão de selo, pois é necessário que o pedido tenha uma emissão realizada");

            if (pedido.EmissaoSelo.StatusSelo != StatusSelo.Ativo)
                throw new BusinessRuleException("Não foi possível bloquear uma emissão de selo, pois é necessário que a emissão esteja ativa");

            var emissao = _emissaoSeloRepositorio.GetById(pedido.EmissaoSelo.Id);
            emissao.StatusSelo = StatusSelo.Bloqueado;
            emissao.UsuarioAlteracaoStatus = _usuarioRepositorio.GetById(idUsuario.Value);
            _emissaoSeloRepositorio.Save(emissao);

            pedido.TipoPedidoSelo = TipoPedidoSelo.Bloqueio;
            //Integração soft park
        }

        private void WorkflowNovaEmissao(PedidoSelo pedido, AcaoWorkflowPedido? acao, int? idUsuario, string descricaoHistorico = null)
        {
            pedido.EmissaoSelo = null;

            switch (pedido.StatusPedido)
            {
                case StatusPedidoSelo.Rascunho:
                    var necessitaAprovacao = false;
                    var possuiDesconto = PedidoPossuiDesconto(pedido);

                    if (possuiDesconto)
                        necessitaAprovacao = DescontoNecessitaAprovacao(pedido) ? true : false;
                    else
                        pedido.Desconto = null;

                    if (necessitaAprovacao)
                    {
                        AdicionarHistorico(pedido, StatusPedidoSelo.PendenteAprovacaoDesconto, idUsuario);
                        _pedidoSeloRepositorio.Save(pedido);
                        _negociacaoSeloDescontoNotificacaoRepositorio.Criar(pedido, idUsuario ?? 0);
                    }
                    else
                    {
                        AdicionarHistorico(pedido, StatusPedidoSelo.PendenteAprovacaoPedido, idUsuario);
                        _pedidoSeloRepositorio.Save(pedido);
                        _pedidoSeloNotificacaoRepositorio.Criar(pedido, idUsuario ?? 0);
                    }

                    break;
                case StatusPedidoSelo.PendenteAprovacaoDesconto:
                    if (acao == AcaoWorkflowPedido.Aprovar)
                    {
                        AdicionarHistorico(pedido, StatusPedidoSelo.DescontoAprovado, idUsuario);
                        AdicionarHistorico(pedido, StatusPedidoSelo.PendenteAprovacaoPedido, idUsuario);
                        _pedidoSeloNotificacaoRepositorio.Criar(pedido, idUsuario ?? 0);
                    }
                    else if (acao == AcaoWorkflowPedido.Reprovar)
                    {
                        AdicionarHistorico(pedido, StatusPedidoSelo.DescontoReprovado, idUsuario);
                        AdicionarHistorico(pedido, StatusPedidoSelo.PendenteAprovacaoPedido, idUsuario);
                        _pedidoSeloNotificacaoRepositorio.Criar(pedido, idUsuario ?? 0);
                    }
                    break;
                case StatusPedidoSelo.PendenteAprovacaoPedido:
                    if (acao == AcaoWorkflowPedido.Aprovar)
                    {
                        AdicionarHistorico(pedido, StatusPedidoSelo.PedidoAprovado, idUsuario);
                        AdicionarHistorico(pedido, StatusPedidoSelo.PendenteAprovacaoCliente, idUsuario);

                        Salvar(pedido, idUsuario, AcaoWorkflowPedido.Aprovar, string.Empty);
                    }
                    else if (acao == AcaoWorkflowPedido.Reprovar)
                    {
                        AdicionarHistorico(pedido, StatusPedidoSelo.PedidoReprovado, idUsuario);
                    }
                    break;
                case StatusPedidoSelo.PendenteAprovacaoCliente:
                    if (acao == AcaoWorkflowPedido.Aprovar)
                    {
                        AdicionarHistorico(pedido, StatusPedidoSelo.AprovadoPeloCliente, idUsuario);
                        var valor = CalculaValorLancamentoCobranca(pedido);

                        if (valor > 0)
                        {
                            _lancamentoCobrancaPedidoSeloServico.Salvar(pedido, valor);
                            GerarBoleto(pedido.Id, 1);
                        }
                        
                        if (valor <= 0
                            || pedido.TiposPagamento == TipoPagamentoSelo.Pospago)
                        {
                            var emissaoSeloServico = ServiceLocator.Current.GetInstance<IEmissaoSeloServico>();
                            var emissaoSelo = emissaoSeloServico.GerarEmissaoSelos(new EmissaoSelo(), pedido.Id, new DateTime?());
                            var idEmissaoSelo = emissaoSeloServico.SalvarComRetorno(emissaoSelo);
                            pedido.EmissaoSelo = emissaoSeloServico.BuscarPorId(idEmissaoSelo);
                            _pedidoSeloRepositorio.Save(pedido);

                            _notificacaoAvisoPedidoSeloServico.Criar(pedido, idUsuario.Value, null, $"EmissaoSelo/Edit?id={idEmissaoSelo}");
                        }
                    }
                    else if (acao == AcaoWorkflowPedido.Reprovar)
                    {
                        AdicionarHistorico(pedido, StatusPedidoSelo.ReprovadoPeloCliente, idUsuario);
                        EnviaEmailPropostaReprovada(pedido, idUsuario.Value);
                    }
                    break;
                case StatusPedidoSelo.DescontoAprovado:
                case StatusPedidoSelo.DescontoReprovado:
                case StatusPedidoSelo.PedidoAprovado:
                case StatusPedidoSelo.PedidoReprovado:
                case StatusPedidoSelo.AprovadoPeloCliente:
                case StatusPedidoSelo.ReprovadoPeloCliente:
                case StatusPedidoSelo.Cancelado:
                    _notificacaoAvisoPedidoSeloServico.Criar(pedido, idUsuario.Value);
                    break;
            }
        }

        public void ClonarPedido(PedidoSelo pedidoSelo, int idUsuario)
        {
            if (pedidoSelo == null)
                throw new BusinessRuleException("Pedido não encontrado");

            var pedidoSeloClone = new PedidoSelo()
            {
                Cliente = pedidoSelo.Cliente != null && pedidoSelo.Cliente.Id != 0 ? pedidoSelo.Cliente : null,
                Convenio = pedidoSelo.Convenio != null && pedidoSelo.Convenio.Id != 0 ? pedidoSelo.Convenio : null,
                DataVencimento = pedidoSelo.DataVencimento > System.Data.SqlTypes.SqlDateTime.MinValue.Value
                                    ? pedidoSelo.DataVencimento
                                    : DateTime.Now.Date.AddDays(5),
                EmissaoSelo = null,
                Desconto = pedidoSelo.Desconto != null && pedidoSelo.Desconto.Id != 0 ? pedidoSelo.Desconto : null,
                Proposta = pedidoSelo.Proposta != null && pedidoSelo.Proposta.Id != 0 ? pedidoSelo.Proposta : null,
                Quantidade = pedidoSelo.Quantidade,
                TipoPedidoSelo = TipoPedidoSelo.Emissao,
                TipoSelo = pedidoSelo.TipoSelo != null && pedidoSelo.TipoSelo.Id != 0 ? pedidoSelo.TipoSelo : null,
                TiposPagamento = pedidoSelo.TiposPagamento,
                Unidade = pedidoSelo.Unidade != null && pedidoSelo.Unidade.Id != 0 ? pedidoSelo.Unidade : null,
                ValidadePedido = pedidoSelo.ValidadePedido
            };

            pedidoSelo.Usuario = new Usuario { Id = idUsuario };
            AdicionarHistorico(pedidoSeloClone, StatusPedidoSelo.Rascunho, null);
            _pedidoSeloRepositorio.Save(pedidoSeloClone);
        }

        public bool ExisteSelosAtivo(PedidoSelo pedidoSelo)
        {
            var emissaoSelo = _emissaoSeloRepositorio.FirstBy(x=>x.PedidoSelo.Id == pedidoSelo.Id);
            if (!ReferenceEquals(emissaoSelo, null))
            {
                    return emissaoSelo.StatusSelo == StatusSelo.Ativo;
            }

            return false;
                    
        }

        public void CancelarPedido(PedidoSelo pedidoSelo, int idUsuario)
        {
            if (pedidoSelo == null)
                throw new BusinessRuleException("Pedido não encontrado");           

            pedidoSelo.EmissaoSelo = pedidoSelo.EmissaoSelo != null && pedidoSelo.EmissaoSelo.Id != 0 ? pedidoSelo.EmissaoSelo : null;
            pedidoSelo.Proposta = pedidoSelo.Proposta != null && pedidoSelo.Proposta.Id != 0 ? pedidoSelo.Proposta : null;

            pedidoSelo.Usuario = new Usuario { Id = idUsuario };
            Salvar(pedidoSelo, idUsuario, AcaoWorkflowPedido.Cancelar);
        }

        public IList<PedidoSelo> ConsultaPedidosPorStatus(int statusid)
        {
            var status = (StatusPedidoSelo)statusid + 1;
            //return _pedidoSeloRepositorio.ConsultaPedidosPorStatus(status);
            return Repositorio.List().Where(x => x.StatusPedido == status).ToList();
        }

        private List<string> RetornaListaEmailProposta(PedidoSelo pedido)
        {
            return pedido.Proposta.Email.Split(',').ToList();
        }

        public void EnviaEmailPropostaAprovada(PedidoSelo pedido, int idUsuario, Stream stream)
        {
            var urlApi = ConfigurationManager.AppSettings["WEB_API"];
            var emailFrom = ConfigurationManager.AppSettings["EMAIL_FROM"];
            var body = ConfigurationManager.AppSettings["EMAIL_CORPO_PEDIDO_SELO_PROPOSTA"];
            var dataAtual = DateTime.Now;
            var listaEmail = RetornaListaEmailProposta(pedido);
            foreach (var email in listaEmail)
            {
                try
                {
                    Mail.SendMail(email, "Proposta - Convênio - Estacionamento", body, emailFrom, stream, "proposta.pdf");
                    AdicionarEmail(pedido, idUsuario, email, true, TipoEnvioEmailPedidoSelo.Proposta, $"Proposta - Sucesso ao enviar email.", dataAtual);
                }
                catch (Exception ex)
                {
                    AdicionarEmail(pedido, idUsuario, email, false, TipoEnvioEmailPedidoSelo.Proposta, $"Proposta - Erro ao enviar email. Detalhes: {ex.Message}", dataAtual);
                }
            }
        }

        public void EnviaEmailPropostaReprovada(PedidoSelo pedido, int idUsuario)
        {
            var body = ConfigurationManager.AppSettings["EMAIL_CORPO_PEDIDO_SELO_PROPOSTA_REPROVADA"];
            var emailFrom = ConfigurationManager.AppSettings["EMAIL_FROM"];
            var dataAtual = DateTime.Now;
            var listaEmail = RetornaListaEmailProposta(pedido);
            foreach (var email in listaEmail)
            {
                try
                {
                    Mail.SendMail(email, "Proposta Reprovada - Convênio - Estacionamento", body, emailFrom);
                    AdicionarEmail(pedido, idUsuario, email, true, TipoEnvioEmailPedidoSelo.PropostaRecusada, $"Proposta Recusada - Sucesso ao enviar email.", dataAtual);
                }
                catch (Exception ex)
                {
                    AdicionarEmail(pedido, idUsuario, email, false, TipoEnvioEmailPedidoSelo.PropostaRecusada, $"Proposta Recusada - Erro ao enviar email. Detalhes: {ex.Message}", dataAtual);
                }
            }
        }

        public IList<PedidoSelo> ListaPedidosSelo(int? idPedidoSelo, int? idCliente, int? idConvenio, int? idUnidade, int? idTipoSelo, DateTime? validade, int? idTipoPagamento)
        {
            if (DateTime.TryParse(validade.Value.ToString(), out DateTime dtValida) && validade.Value > (new DateTime(1900, 1, 1)))
                validade = dtValida;
            else
                validade = null;

            var listaFiltrada = Repositorio.List().Where(x => x.TipoPedidoSelo == TipoPedidoSelo.Emissao
                                                      && x.StatusPedido == StatusPedidoSelo.AprovadoPeloCliente
                                                      && x.Cliente != null
                                                      && x.TipoSelo != null
                                                      && x.Unidade != null
                                                      && x.Proposta != null);

            listaFiltrada = listaFiltrada.Where(x => x.Id == (idPedidoSelo != null && idPedidoSelo != 0 ? idPedidoSelo : x.Id)
                                                      && (validade != null ? x.ValidadePedido == validade : x.ValidadePedido == x.ValidadePedido)
                                                      && (idCliente != 0 ? (x.Cliente.Id == idCliente) : x.Cliente.Id == x.Cliente.Id)
                                                      && (idConvenio != 0 ? x.Convenio.Id == idConvenio : x.Convenio.Id == x.Convenio.Id)
                                                      && (idTipoSelo != 0 ? x.TipoSelo.Id == idTipoSelo : x.TipoSelo.Id == x.TipoSelo.Id)
                                                      && (idUnidade != 0 ? x.Unidade.Id == idUnidade : x.Unidade.Id == x.Unidade.Id)
                                                      && (idTipoPagamento != 0 ? x.TiposPagamento == (TipoPagamentoSelo)idTipoPagamento : x.TiposPagamento == x.TiposPagamento)
                                                      ).ToList();

            var idsListaFiltro = listaFiltrada.Select(x => x.Id)?.ToList();
            var listaIdPedidoComEmissao = idsListaFiltro.Any() ? _emissaoSeloRepositorio.RetornaListaIdPedidoSeloDasEmissoesDeSelo(idsListaFiltro) : new List<int>();

            return listaFiltrada.Where(x => !listaIdPedidoComEmissao.Contains(x.Id)).ToList();
        }

        public IList<PedidoSelo> BuscarPedidosSeloAprovados(int? clienteId, int? convenioId, int? unidadeId, int? tipoSeloId, TipoPagamentoSelo? tipoPagamento)
        {
            var pedidosSelo = _pedidoSeloRepositorio.List().Where(x => x.StatusPedido == StatusPedidoSelo.AprovadoPeloCliente).ToList();

            pedidosSelo = pedidosSelo.Where(x => (!clienteId.HasValue || x.Cliente.Id == clienteId.Value)
                                                      && (!convenioId.HasValue || x.Convenio.Id == convenioId.Value)
                                                      && (!unidadeId.HasValue || x.Unidade.Id == unidadeId.Value)
                                                      && (!tipoSeloId.HasValue || x.TipoSelo.Id == tipoSeloId.Value)
                                                      && (!tipoPagamento.HasValue || (int)tipoPagamento == 0 || x.TiposPagamento == tipoPagamento)
                                                      ).ToList();

            return pedidosSelo.ToList();
        }

        public Dictionary<int, bool> EmailForamEnviados(List<int> listaIdPedido)
        {
            var retorno = new Dictionary<int, bool>();
            foreach (var idPedido in listaIdPedido)
                retorno.Add(idPedido, EmailFoiEnviado(idPedido));

            return retorno;
        }

        private bool EmailFoiEnviado(int idPedido)
        {
            var listaPedidoEmail = BuscarPorId(idPedido)?.PedidoSeloEmails?.ToList();
            if (listaPedidoEmail == null || !listaPedidoEmail.Any() || !listaPedidoEmail.Any(x => x.Tipo == TipoEnvioEmailPedidoSelo.Proposta))
                return false;

            listaPedidoEmail = listaPedidoEmail.Where(x => x.Tipo == TipoEnvioEmailPedidoSelo.Proposta).ToList();
            var ultimaData = listaPedidoEmail?.GroupBy(x => x.DataInsercao)?.Select(x => x.Key)?.OrderByDescending(x => x)?.FirstOrDefault();
            if (ultimaData == null)
                return false;

            return listaPedidoEmail.Any(x => x.DataInsercao == ultimaData && x.Enviado);
        }

        private void AdicionarHistorico(PedidoSelo pedido, StatusPedidoSelo status, int? idUsuario, string descricao = null)
        {
            if (pedido.PedidoSeloHistorico == null)
                pedido.PedidoSeloHistorico = new List<PedidoSeloHistorico>();

            var hist = new PedidoSeloHistorico
            {
                DataInsercao = DateTime.Now,
                PedidoSelo = pedido,
                Proposta = pedido.Proposta, //Verificar se há necessidade, pois não mudar nunca, acredito eu né... 
                StatusPedidoSelo = status,
                Usuario = idUsuario.HasValue && idUsuario.Value > 0 ? new Usuario { Id = idUsuario.Value } : null,
                Descricao = descricao
            };
            pedido.PedidoSeloHistorico.Add(hist);
            _pedidoSeloRepositorio.Save(pedido);
        }

        public void AdicionarEmail(PedidoSelo pedido, int? idUsuario, string email, bool enviadoComSucesso, TipoEnvioEmailPedidoSelo tipo, string descricao, DateTime? dataAtual = null)
        {
            if (pedido.PedidoSeloEmails == null)
                pedido.PedidoSeloEmails = new List<PedidoSeloEmail>();

            var eml = new PedidoSeloEmail
            {
                DataInsercao = !dataAtual.HasValue || dataAtual.Value == DateTime.MinValue ? DateTime.Now : dataAtual.Value,
                PedidoSelo = pedido,
                Proposta = pedido.Proposta, //Verificar se há necessidade, pois não mudar nunca, acredito eu né... 
                Descricao = descricao,
                Email = email,
                Enviado = enviadoComSucesso,
                Tipo = tipo
            };
            pedido.PedidoSeloEmails.Add(eml);
            _pedidoSeloRepositorio.Save(pedido);
        }

        public string[] DescriptografarChave(string chave)
        {
            var dadosDescriptografados = Crypt.DeCrypt(chave, ConfigurationManager.AppSettings["CryptKey"]).FromBase64().Split('|');
            return dadosDescriptografados;
        }

        public decimal CalculaValorLancamentoCobranca(PedidoSelo pedido)
        {
            decimal valorCalculado = 0;


            var tabelaPreco = _tabelaPrecoAvulsaRepositorio.FirstBy(x => x.Padrao && x.ListaUnidade.Count(y => y.Unidade.Id == pedido.Unidade.Id) > 0);
            var precoParametro = _precoParametroSeloRepositorio.FirstBy(x => x.Unidade.Id == pedido.Unidade.Id && x.TipoPreco.Id == pedido.TipoSelo.Id);

            if (tabelaPreco != null)
            {
                var diaria = tabelaPreco.ListaUnidade.FirstOrDefault(y => y.Unidade.Id == pedido.Unidade.Id).ValorDiaria;
                decimal desconto;

                if (precoParametro != null)
                {
                    desconto = precoParametro.DescontoCustoTabelaPreco;

                    if (pedido.TipoSelo.ParametroSelo == ParametroSelo.Monetario)
                        valorCalculado = pedido.TipoSelo.Valor;
                    else if (pedido.TipoSelo.ParametroSelo == ParametroSelo.Percentual)
                        valorCalculado = diaria - (diaria * desconto);
                    else if (pedido.TipoSelo.ParametroSelo == ParametroSelo.HoraInicial)
                    {
                        var linhaInicial = tabelaPreco.ListaHoraValor.FirstOrDefault();
                        if (linhaInicial != null)
                            valorCalculado = linhaInicial.Valor - (linhaInicial.Valor * desconto);
                        else
                        {
                            var erro = $"Foi selecionado um selo de Linha Inicial da Tabela de Preço, porém a mesma não foi configurada para unidade {pedido.Unidade.Codigo}! Pedido Cancelado";
                            AdicionarHistorico(pedido, StatusPedidoSelo.Cancelado, pedido.Usuario.Id, erro);
                            //_notificacaoAvisoPedidoSeloServico.Criar(pedido, pedido.Usuario.Id);
                            return 0;
                        }
                    }
                    else if (pedido.TipoSelo.ParametroSelo == ParametroSelo.HoraAdicional)
                    {
                        if (tabelaPreco.HoraAdicional)
                            valorCalculado = tabelaPreco.ValorHoraAdicional - (tabelaPreco.ValorHoraAdicional * desconto);
                        else
                        {
                            var erro = $"Foi selecionado um selo de Linha Inicial da Tabela de Preço, porém a mesma não foi configurada para unidade {pedido.Unidade.Codigo}! Pedido Cancelado";
                            AdicionarHistorico(pedido, StatusPedidoSelo.Cancelado, pedido.Usuario.Id, erro);
                            //_notificacaoAvisoPedidoSeloServico.Criar(pedido, pedido.Usuario.Id);
                            return 0;
                        }
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
            {
                //valorCalculado = (pedido.Convenio.ConvenioUnidades.Count(x => x.ConvenioUnidade.Unidade.Id == pedido.Unidade.Id) > 0
                //                    ? pedido.Convenio.ConvenioUnidades.FirstOrDefault(x => x.ConvenioUnidade.Unidade.Id == pedido.Unidade.Id
                //                                                                            && x.ConvenioUnidade.TipoSelo.Id == pedido.TipoSelo.Id).ConvenioUnidade.Valor > 0
                //                       ? pedido.Convenio.ConvenioUnidades.FirstOrDefault(x => x.ConvenioUnidade.Unidade.Id == pedido.Unidade.Id
                //                                                                            && x.ConvenioUnidade.TipoSelo.Id == pedido.TipoSelo.Id).ConvenioUnidade.Valor
                //                        : pedido.TipoSelo.Valor
                //                    : pedido.TipoSelo.Valor);
                valorCalculado = pedido.TipoSelo.Valor;
            }

            valorCalculado = valorCalculado * pedido.Quantidade;

            var valorDoDesconto = RetornaValorDescontoPedido(pedido, valorCalculado);
            var valorTotal = valorCalculado - valorDoDesconto;

            if (valorTotal < 0)
                return 0;

            return valorTotal;
        }

        private decimal RetornaValorDescontoPedido(PedidoSelo pedido, decimal? valorCalculadoPedido)
        {
            var desconto = 0;

            if (pedido.Desconto == null || pedido.Desconto.Id == 0)
                return desconto;

            if (!pedido.Desconto.NecessitaAprovacao)
                return CalculoDesconto(pedido, valorCalculadoPedido);

            var descontoAprovado = _pedidoSeloRepositorio.PedidoPassouPeloStatus(pedido.Id, StatusPedidoSelo.DescontoAprovado);
            if (descontoAprovado)
                return CalculoDesconto(pedido, valorCalculadoPedido);

            return desconto;
        }
        private decimal CalculoDesconto(PedidoSelo pedido, decimal? valorCalculadoPedido)
        {
            return pedido.Desconto.Valor <= 0
                        ? 0
                        : pedido.Desconto.TipoDesconto == TipoDesconto.Monetario
                            ? pedido.Desconto.Valor
                            : pedido.Desconto.Valor / 100 * (valorCalculadoPedido ?? 0);
        }

        public void GerarBoleto(int idPedido, int idUsuario)
        {
            var lancamentoCobrancaPedidoSelo = _lancamentoCobrancaPedidoSeloServico.RetornaUltimoLancamentoCobrancaPorPedidoSelo(idPedido);
            var boletoHtml = _lancamentoCobrancaPedidoSeloServico.GerarBoletoBancarioImpressao(lancamentoCobrancaPedidoSelo);
            var boletoStream = Core.Tools.GerarPdfFromHtmlToStream(boletoHtml);

            var OutputPath = "Boleto.pdf";

            var corpoEmail = ConfigurationManager.AppSettings["EMAIL_CORPO_PEDIDO_SELO_BOLETO"];
            var remetente = ConfigurationManager.AppSettings["EMAIL_FROM"];
            var pedido = lancamentoCobrancaPedidoSelo?.PedidoSelo;
            var dataAtual = DateTime.Now;

            foreach (var email in lancamentoCobrancaPedidoSelo?.PedidoSelo?.Proposta?.Email?.Trim().Split(','))
            {
                try
                {
                    Mail.SendMail(email, "Boleto - Convênio - Estacionamento", corpoEmail, remetente, boletoStream, OutputPath);
                    AdicionarEmail(pedido, idUsuario, email, true, TipoEnvioEmailPedidoSelo.Boleto, "Boleto - Sucesso ao enviar email", dataAtual);
                }
                catch (Exception ex)
                {
                    AdicionarEmail(pedido, idUsuario, email, false, TipoEnvioEmailPedidoSelo.Boleto, $"Boleto - Erro ao enviar email. Detalhes: {ex.Message}", dataAtual);
                }
            }
        }

        public List<PedidoSelo> BuscarAprovadosPeloCliente()
        {
            return _pedidoSeloRepositorio.List().Where(x => x.StatusPedido == StatusPedidoSelo.AprovadoPeloCliente).ToList();
        }

        private bool PedidoPossuiDesconto(PedidoSelo pedido)
        {
            return pedido.Desconto.Id != 0;
        }

        private bool DescontoNecessitaAprovacao(PedidoSelo pedido)
        {
            var negociacaoSeloDesconto = new Desconto();
            negociacaoSeloDesconto = _negociacaoSeloDescontoRepositorio.GetById(pedido.Desconto.Id);

            return negociacaoSeloDesconto != null && negociacaoSeloDesconto.NecessitaAprovacao
                ? true
                : false;
        }

        public void Desbloquear(PedidoSelo pedidoSelo, int idUsuario)
        {
            if (pedidoSelo == null)
                throw new BusinessRuleException("Pedido não encontrado");

            pedidoSelo.TipoPedidoSelo = TipoPedidoSelo.Desbloqueio;
            Salvar(pedidoSelo, idUsuario);
        }

        public void Bloquear(PedidoSelo pedidoSelo, int idUsuario)
        {
            if (pedidoSelo == null)
                throw new BusinessRuleException("Pedido não encontrado");

            pedidoSelo.TipoPedidoSelo = TipoPedidoSelo.Bloqueio;
            Salvar(pedidoSelo, idUsuario);
        }

        public decimal RetornaValorSeloComDesconto(PedidoSelo pedidoSelo)
        {
            var emissaoSeloServico = ServiceLocator.Current.GetInstance<IEmissaoSeloServico>();
            return emissaoSeloServico.RetornaValorSeloComDesconto(pedidoSelo);
        }
    }
}