using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CidadeRepositorio : NHibRepository<Cidade>, ICidadeRepositorio
    {
        public CidadeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}