using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class BancoRepositorio : NHibRepository<Banco>, IBancoRepositorio
    {
        public BancoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}