using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ChequeEmitidoContaPagarRepositorio : NHibRepository<ChequeEmitidoContaPagar>, IChequeEmitidoContaPagarRepositorio
    {
        public ChequeEmitidoContaPagarRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
