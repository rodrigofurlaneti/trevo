using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ChequeRepositorio : NHibRepository<Cheque>, IChequeRepositorio
    {
        public ChequeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}