using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ParametrizacaoLocacaoRepositorio : NHibRepository<ParametrizacaoLocacao>, IParametrizacaoLocacaoRepositorio
    {
        public ParametrizacaoLocacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}