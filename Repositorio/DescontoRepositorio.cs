using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class DescontoRepositorio : NHibRepository<Desconto>, IDescontoRepositorio
    {
        public DescontoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
