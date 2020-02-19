using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IPagamentoReembolsoAplicacao : IBaseAplicacao<PagamentoReembolso>
    {
        void SalvarSolicitacoes(List<ContasAPagarViewModel> contasAPagar);
        void NegarSolicitacao(int idContaAPagar);
        IList<PagamentoReembolso> ListarLancamentoReembolso(PagamentoReembolsoViewModel filtro);
    }

    public class PagamentoReembolsoAplicacao : BaseAplicacao<PagamentoReembolso, IPagamentoReembolsoServico>, IPagamentoReembolsoAplicacao
    {
        private readonly IPagamentoReembolsoServico _pagamentoReembolsoServico;
        private readonly IContaPagarAplicacao _contaPagarAplicacao;

        public PagamentoReembolsoAplicacao(
            IPagamentoReembolsoServico pagamentoReembolsoServico,
            IContaPagarAplicacao contaPagarAplicacao
        )
        {
            _pagamentoReembolsoServico = pagamentoReembolsoServico;
            _contaPagarAplicacao = contaPagarAplicacao;
        }

        public void NegarSolicitacao(int idContaAPagar)
        {
            var entidade = _pagamentoReembolsoServico.BuscarPor(x => x.ContaAPagar.Id == idContaAPagar)?.FirstOrDefault();
            if (entidade == null)
            {
                var contaAPagar = _contaPagarAplicacao.BuscarPorId(idContaAPagar);
                entidade = new PagamentoReembolso
                {
                    ContaAPagar = contaAPagar
                };
            }

            entidade.Status = StatusPagamentoReembolso.Negado;
            _pagamentoReembolsoServico.Salvar(entidade);
        }

        public void SalvarSolicitacoes(List<ContasAPagarViewModel> contasAPagar)
        {
            var contasAPagarIds = contasAPagar.Select(x => x.Id);
            var contas = _contaPagarAplicacao.BuscarPor(x => contasAPagarIds.Contains(x.Id));
            var listaPagamentoReembolso = new List<PagamentoReembolso>();

            foreach (var conta in contas)
            {
                var numeroRecibo = contasAPagar.Find(x => x.Id == conta.Id)?.NumeroRecibo;
                var pagamentoReembolso = new PagamentoReembolso
                {
                    ContaAPagar = conta,
                    NumeroRecibo = numeroRecibo,
                    Status = StatusPagamentoReembolso.Pendente
                };

                listaPagamentoReembolso.Add(pagamentoReembolso);
            }

            _pagamentoReembolsoServico.Salvar(listaPagamentoReembolso);
        }

        public IList<PagamentoReembolso> ListarLancamentoReembolso(PagamentoReembolsoViewModel filtro)
        {
            return _pagamentoReembolsoServico.ListarLancamentoReembolso(filtro?.ContasAPagarViewModel?.Unidade?.Id, filtro?.DataInsercao, filtro.ContasAPagarViewModel.Departamento?.Id, filtro.NumeroRecibo);
        }
    }
}