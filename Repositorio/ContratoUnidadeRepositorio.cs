using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ContratoUnidadeRepositorio : NHibRepository<ContratoUnidade>, IContratoUnidadeRepositorio
    {
        public ContratoUnidadeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}