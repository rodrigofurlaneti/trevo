using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
namespace Repositorio
{
    public class PrecoStatusRepositorio : NHibRepository<PrecoStatus>, IPrecoStatusRepositorio
    {
        public PrecoStatusRepositorio(NHibContext context)
          : base(context)
        {
        }
    }
}
