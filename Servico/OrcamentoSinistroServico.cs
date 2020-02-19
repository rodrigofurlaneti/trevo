using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface IOrcamentoSinistroServico : IBaseServico<OrcamentoSinistro>
    {
        void Salvar(OrcamentoSinistro orcamentoSinistro, Usuario usuario);
        void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao);
        OrcamentoSinistro BuscarPelaNotificacaoId(int notificacaoId);
        void SalvarCotacao(OrcamentoSinistro orcamentoSinistro, List<OrcamentoSinistroCotacaoItem> orcamentoSinistroCotacaoItens, Usuario usuario);
        void AtualizarStatusCotacao(int notificacaoId, Usuario usuario, AcaoNotificacao acao);
        OrcamentoSinistroCotacao BuscarCotacaoPelaNotificacaoId(int notificacaoId);
        void Cancelar(int id);
        void GerarNotificacaoSeDataServicoVencida();
    }

    public class OrcamentoSinistroServico : BaseServico<OrcamentoSinistro, IOrcamentoSinistroRepositorio>, IOrcamentoSinistroServico
    {
        private readonly IOrcamentoSinistroRepositorio _orcamentoSinistroRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IPecaServicoRepositorio _pecaServicoRepositorio;
        private readonly IContaPagarRepositorio _contaPagarRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IContaFinanceiraRepositorio _contaFinanceiraRepositorio;

        public OrcamentoSinistroServico(
            IOrcamentoSinistroRepositorio orcamentoSinistroRepositorio
            , INotificacaoRepositorio notificacaoRepositorio
            , IPecaServicoRepositorio pecaServicoRepositorio
            , IContaPagarRepositorio contaPagarRepositorio
            , IUsuarioRepositorio usuarioRepositorio
            , IContaFinanceiraRepositorio contaFinanceiraRepositorio
            )
        {
            _orcamentoSinistroRepositorio = orcamentoSinistroRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
            _pecaServicoRepositorio = pecaServicoRepositorio;
            _contaPagarRepositorio = contaPagarRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _contaFinanceiraRepositorio = contaFinanceiraRepositorio;
        }

        private void AdicionarNotificacao(OrcamentoSinistro orcamentoSinistro, Usuario usuario)
        {
            orcamentoSinistro.OrcamentoSinistroNotificacoes = Repositorio.GetById(orcamentoSinistro.Id)?.OrcamentoSinistroNotificacoes?.ToList() ?? new List<OrcamentoSinistroNotificacao>();

            var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(orcamentoSinistro, Entidades.OrcamentoSinistro, usuario, null, "");

            var orcamentoSinistroNotificacao = new OrcamentoSinistroNotificacao
            {
                OrcamentoSinistro = orcamentoSinistro,
                Notificacao = notificacao
            };

            orcamentoSinistro.OrcamentoSinistroNotificacoes.Add(orcamentoSinistroNotificacao);
            orcamentoSinistro.Status = StatusOrcamentoSinistro.AguardandoAprovacao;
        }

        private void AdicionarPecaServico(OrcamentoSinistro orcamentoSinistro)
        {
            var pecasServicosNomes = orcamentoSinistro.OrcamentoSinistroPecaServicos.Select(x => x.PecaServico.Nome);
            var pecasServicosExistentes = _pecaServicoRepositorio.ListBy(x => pecasServicosNomes.Contains(x.Nome));

            foreach (var item in orcamentoSinistro.OrcamentoSinistroPecaServicos)
            {
                var existente = pecasServicosExistentes.FirstOrDefault(x => x.Nome == item.PecaServico.Nome);
                item.PecaServico.Id = existente != null ? existente.Id : 0;
            }

            var pecasServicosParaSalvar = orcamentoSinistro.OrcamentoSinistroPecaServicos.Select(x => x.PecaServico).Where(x => x.Id == 0).ToList();
            if (pecasServicosParaSalvar != null && pecasServicosParaSalvar.Any())
                _pecaServicoRepositorio.Save(pecasServicosParaSalvar);
        }

        private void AdicionarCotacaoNotificacao(OrcamentoSinistroCotacao orcamentoSinistroCotacao, Usuario usuario)
        {
            orcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes = orcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes ?? new List<OrcamentoSinistroCotacaoNotificacao>();

            var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(orcamentoSinistroCotacao, Entidades.OrcamentoSinistroCotacao, usuario, null, "", "OrcamentoSinistro/Cotacao");

            var orcamentoSinistroNotificacao = new OrcamentoSinistroCotacaoNotificacao
            {
                Notificacao = notificacao
            };

            orcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes.Add(orcamentoSinistroNotificacao);
            orcamentoSinistroCotacao.Status = StatusOrcamentoSinistroCotacao.AguardandoAprovacao;
        }

        private void AdicionarContasAPagar(OrcamentoSinistro orcamentoSinistro)
        {
            var orcamentoSinistroCotacao = orcamentoSinistro.OrcamentoSinistroCotacao;

            foreach (var item in orcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens)
            {
                item.ContasAPagar = new ContasAPagar
                {
                    Unidade = orcamentoSinistro.OIS?.Unidade,
                    Fornecedor = item.Fornecedor,
                    ValorTotal = item.ValorTotal,
                    FormaPagamento = (FormaPagamento)Enum.Parse(typeof(FormaPagamento), item.FormaPagamento.ToString()),
                    CodigoAgrupadorParcela = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TipoPagamento = TipoContaPagamento.Etc,
                    StatusConta = StatusContasAPagar.EmAberto
                };
            }
        }

        private void AdicionarContasAPagarNotificacoes(IList<OrcamentoSinistroCotacaoItem> itens, int orcamentoSinistroId, Usuario usuario)
        {
            foreach (var item in itens)
            {
                if (item.ContasAPagar != null)
                {
                    var descrição = $"Complete as informações da Conta a pagar de ID: {item.ContasAPagar.Id} - Referente ao Orçamento Sinistro de ID: {orcamentoSinistroId}" +
                            $" para o Fornecedor {item.ContasAPagar.Fornecedor.Nome}.";

                    var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(item.ContasAPagar, Entidades.ContasAPagar, usuario, null, descrição, "contapagar/finalizarcadastro", TipoAcaoNotificacao.Aviso);

                    if (item.ContasAPagar.ContaPagarNotificacoes == null)
                        item.ContasAPagar.ContaPagarNotificacoes = new List<ContaPagarNotificacao>();

                    item.ContasAPagar.ContaPagarNotificacoes.Add(new ContaPagarNotificacao
                    {
                        Notificacao = notificacao
                    });
                }
            }
        }

        private void AdicionarLancamentoCobranca(OrcamentoSinistro orcamentoSinistro)
        {
            var orcamentoSinistroCotacao = orcamentoSinistro.OrcamentoSinistroCotacao;
            var contaPadrao = _contaFinanceiraRepositorio.FirstBy(x => x.ContaPadrao);

            foreach (var item in orcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens)
            {
                if (item.TemSeguroReembolso && item.ValorReembolso > 0)
                {
                    item.LancamentoCobranca = new LancamentoCobranca
                    {
                        Unidade = orcamentoSinistro.OIS?.Unidade,
                        ContaFinanceira = contaPadrao,
                        TipoServico = TipoServico.SeguroReembolso,
                        DataGeracao = DateTime.Now,
                        DataVencimento = item.DataReembolso.Value,
                        DataCompetencia = new DateTime(item.DataReembolso.Value.Year, item.DataReembolso.Value.Month, 1),
                        StatusLancamentoCobranca = StatusLancamentoCobranca.EmAberto,
                        ValorContrato = item.ValorTotal,
                        CiaSeguro = item.CiaSeguro
                    };
                }
            }
        }

        private void AdicionarLancamentoCobrancaNotificacoes(IList<OrcamentoSinistroCotacaoItem> itens, Usuario usuario)
        {
            foreach (var item in itens)
            {
                if (item.LancamentoCobranca != null)
                {
                    var descrição = $"Seguro Reembolso - Gerado na tela de lançamento de cobrança";

                    var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(item.LancamentoCobranca, Entidades.LancamentoCobranca, usuario, null, descrição, "", TipoAcaoNotificacao.Aviso);

                    if (item.LancamentoCobranca.LancamentoCobrancaNotificacoes == null)
                        item.LancamentoCobranca.LancamentoCobrancaNotificacoes = new List<LancamentoCobrancaNotificacao>();

                    item.LancamentoCobranca.LancamentoCobrancaNotificacoes.Add(new LancamentoCobrancaNotificacao
                    {
                        Notificacao = notificacao
                    });
                }
            }
        }

        private OrcamentoSinistro BuscarPelaCotacaoNotificacaoId(int notificacaoId)
        {
            return _orcamentoSinistroRepositorio.FirstBy(x => x.OrcamentoSinistroCotacao != null &&
                                                        x.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes
                                                        .FirstOrDefault(on => on.Notificacao.Id == notificacaoId) != null);
        }

        public void Salvar(OrcamentoSinistro orcamentoSinistro, Usuario usuario)
        {
            AdicionarPecaServico(orcamentoSinistro);

            _orcamentoSinistroRepositorio.Save(orcamentoSinistro);
            AdicionarNotificacao(orcamentoSinistro, usuario);
            _orcamentoSinistroRepositorio.Save(orcamentoSinistro);
        }

        public void GerarNotificacaoSeDataServicoVencida()
        {
            var orcamentosComItensVencidos = _orcamentoSinistroRepositorio.ListBy(x => x.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens
                                                                                       .Any(item => item.DataServico.Date < DateTime.Now.Date));

            var itensEmAbertoVencidos = orcamentosComItensVencidos.SelectMany(x => x.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens)
                                                          .Where(item => item.StatusCompraServico == StatusCompraServico.EmAberto && item.DataServico.Date < DateTime.Now.Date).ToList();

            var usuario = _usuarioRepositorio.GetById(1);

            foreach (var item in itensEmAbertoVencidos)
            {
                var descricao = $"A data limite de {item.DataServico.ToShortDateString()} da peça/serviço" +
                                $" {item.PecaServico.Nome} da cotação com ID: {item.OrcamentoSinistroCotacao.Id} está vencida.";

                if(!item.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes.Any(x => x.Notificacao.Descricao == descricao))
                {
                    var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(item.OrcamentoSinistroCotacao, Entidades.OrcamentoSinistroCotacao, usuario, null, descricao,
                                    $"controlecompra/Editar?orcamentoCotacaoId={item.OrcamentoSinistroCotacao.Id}&pecaServicoId={item.PecaServico.Id}", TipoAcaoNotificacao.Aviso);

                    var orcamentoSinistroNotificacao = new OrcamentoSinistroCotacaoNotificacao
                    {
                        OrcamentoSinistroCotacao = item.OrcamentoSinistroCotacao,
                        Notificacao = notificacao
                    };

                    item.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes.Add(orcamentoSinistroNotificacao);
                }
            }

            _orcamentoSinistroRepositorio.Save(orcamentosComItensVencidos);
        }

        public void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao)
        {
            var orcamentoSinistro = BuscarPelaNotificacaoId(notificacaoId);
            var orcamentoSinistroNotificacao = orcamentoSinistro.OrcamentoSinistroNotificacoes.FirstOrDefault(x => x.Notificacao.Id == notificacaoId);

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    orcamentoSinistroNotificacao.Aprovar(usuario);
                    orcamentoSinistro.Aprovar();
                    break;
                case AcaoNotificacao.Reprovado:
                    orcamentoSinistroNotificacao.Reprovar(usuario);
                    orcamentoSinistro.Negar();
                    break;
                default:
                    break;
            }

            _orcamentoSinistroRepositorio.Save(orcamentoSinistro);
        }

        public void AtualizarStatusCotacao(int notificacaoId, Usuario usuario, AcaoNotificacao acao)
        {
            var orcamentoSinistro = BuscarPelaCotacaoNotificacaoId(notificacaoId);
            var orcamentoSinistroCotacaoNotificacao = orcamentoSinistro.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes
                                                        .FirstOrDefault(x => x.Notificacao.Id == notificacaoId);

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    orcamentoSinistroCotacaoNotificacao.Aprovar(usuario);
                    orcamentoSinistro.OrcamentoSinistroCotacao.Aprovar();
                    orcamentoSinistro.Status = StatusOrcamentoSinistro.OrcamentoAprovado;
                    this.AdicionarContasAPagar(orcamentoSinistro);
                    this.AdicionarLancamentoCobranca(orcamentoSinistro);

                    _orcamentoSinistroRepositorio.Save(orcamentoSinistro);

                    AdicionarContasAPagarNotificacoes(orcamentoSinistro.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens, orcamentoSinistro.Id, usuario);
                    AdicionarLancamentoCobrancaNotificacoes(orcamentoSinistro.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens, usuario);
                    break;
                case AcaoNotificacao.Reprovado:
                    orcamentoSinistroCotacaoNotificacao.Reprovar(usuario);
                    orcamentoSinistro.OrcamentoSinistroCotacao.Negar();
                    orcamentoSinistro.Status = StatusOrcamentoSinistro.OrcamentoNegado;
                    break;
                default:
                    break;
            }

            _orcamentoSinistroRepositorio.Save(orcamentoSinistro);
        }

        public OrcamentoSinistro BuscarPelaNotificacaoId(int notificacaoId)
        {
            return _orcamentoSinistroRepositorio.FirstBy(x => x.OrcamentoSinistroNotificacoes.Any(on => on.Notificacao.Id == notificacaoId));
        }

        public OrcamentoSinistroCotacao BuscarCotacaoPelaNotificacaoId(int notificacaoId)
        {
            return this.BuscarPelaCotacaoNotificacaoId(notificacaoId).OrcamentoSinistroCotacao;
        }

        public void SalvarCotacao(OrcamentoSinistro orcamentoSinistro, List<OrcamentoSinistroCotacaoItem> orcamentoSinistroCotacaoItens, Usuario usuario)
        {
            if (orcamentoSinistro.OrcamentoSinistroCotacao == null)
                orcamentoSinistro.OrcamentoSinistroCotacao = new OrcamentoSinistroCotacao();

            orcamentoSinistro.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens = orcamentoSinistroCotacaoItens;
            orcamentoSinistro.OrcamentoSinistroCotacao.Status = StatusOrcamentoSinistroCotacao.AguardandoAprovacao;
            orcamentoSinistro.Status = StatusOrcamentoSinistro.AguardandoAprovacaoOrcamento;

            _orcamentoSinistroRepositorio.Save(orcamentoSinistro);
            AdicionarCotacaoNotificacao(orcamentoSinistro.OrcamentoSinistroCotacao, usuario);
            _orcamentoSinistroRepositorio.Save(orcamentoSinistro);
        }

        public void Cancelar(int id)
        {
            var orcamentoSinistro = _orcamentoSinistroRepositorio.GetById(id);

            orcamentoSinistro.Status = StatusOrcamentoSinistro.Cancelado;

            foreach (var item in orcamentoSinistro.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens)
            {
                if (item.ContasAPagar != null)
                {
                    item.ContasAPagar.StatusConta = StatusContasAPagar.Cancelado;
                }

                if (item.LancamentoCobranca != null)
                {
                    item.LancamentoCobranca.StatusLancamentoCobranca = StatusLancamentoCobranca.Cancelado;
                }
            }

            _orcamentoSinistroRepositorio.Save(orcamentoSinistro);
        }
    }
}