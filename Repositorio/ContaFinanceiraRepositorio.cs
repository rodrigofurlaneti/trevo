using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ContaFinanceiraRepositorio : NHibRepository<ContaFinanceira>, IContaFinanceiraRepositorio
    {
        public ContaFinanceiraRepositorio(NHibContext context)
            : base(context)
        {
        }

        public ContaFinanceira BuscarContaPadrao()
        {
            return Session.GetItemBy<ContaFinanceira>(x => x.ContaPadrao);
        }
    }
}