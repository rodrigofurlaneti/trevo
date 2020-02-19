using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MensalistaRepositorio : NHibRepository<Mensalista>, IMensalistaRepositorio
    {
        public MensalistaRepositorio(NHibContext context)
          : base(context)
        {
        }
    }
}
