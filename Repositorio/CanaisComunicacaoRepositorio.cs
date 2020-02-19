using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CanaisComunicacaoRepositorio : NHibRepository<CanaisComunicacao>, ICanaisComunicacaoRepositorio
    {
        public CanaisComunicacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}