using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System.Linq;

namespace Dominio
{

    public interface IChequeEmitidoServico : IBaseServico<ChequeEmitido>
    {

    }

    public class ChequeEmitidoServico : BaseServico<ChequeEmitido,IChequeEmitidoRepositorio>, IChequeEmitidoServico
    {
        private readonly IContaPagarRepositorio _contaPagarRepositorio;

        public ChequeEmitidoServico(IContaPagarRepositorio contaPagarRepositorio)
        {
            _contaPagarRepositorio = contaPagarRepositorio;
        }

        public override void Salvar(ChequeEmitido chequeEmitido)
        {
            var contaIds = chequeEmitido.ListaContaPagar.Select(x => x.ContaPagar.Id).ToList();
            var contas = _contaPagarRepositorio.ListBy(x => contaIds.Contains(x.Id));

            foreach (var conta in contas)
            {
                conta.StatusConta = StatusContasAPagar.Paga;
            }

            _contaPagarRepositorio.Save(contas);

            base.Salvar(chequeEmitido);
        }
    }
}
