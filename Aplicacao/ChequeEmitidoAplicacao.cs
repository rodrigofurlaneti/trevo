using Aplicacao.Base;
using Aplicacao.ViewModels;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{

    public interface IChequeEmitidoAplicacao : IBaseAplicacao<ChequeEmitido>
    {
        IList<ContasAPagarViewModel> BuscarContasPagarPorFornecedor(int idCliente);
        IList<ChaveValorViewModel> ListaStatusCheque();
    }

    public class ChequeEmitidoAplicacao : BaseAplicacao<ChequeEmitido, IChequeEmitidoServico>, IChequeEmitidoAplicacao
    {
        private readonly IContaPagarServico _contaPagarServico;

        public ChequeEmitidoAplicacao(IContaPagarServico contaPagarServico)
        {
            _contaPagarServico = contaPagarServico;
        }

        public IList<ContasAPagarViewModel> BuscarContasPagarPorFornecedor(int idFornecedor)
        {
            var lamcamentos = _contaPagarServico.BuscarLancamentosPorFornecedor(idFornecedor);

            return lamcamentos != null && lamcamentos.Any() ? lamcamentos.Select(x => new ContasAPagarViewModel(x)).ToList() : new List<ContasAPagarViewModel>();
        }

        public IList<ChaveValorViewModel> ListaStatusCheque()
        {
            return Enum.GetValues(typeof(StatusCheque))
                .Cast<StatusCheque>()
                .Select(e => new ChaveValorViewModel
                {
                    Id = (int)e,
                    Descricao = e.ToDescription()
                })
                .ToList();
        }
    }
}
