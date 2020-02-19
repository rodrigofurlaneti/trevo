using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ChequeEmitidoRepositorio : NHibRepository<ChequeEmitido>, IChequeEmitidoRepositorio
    {
        public ChequeEmitidoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
