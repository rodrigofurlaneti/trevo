using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class OrcamentoSinistroRepositorio : NHibRepository<OrcamentoSinistro>, IOrcamentoSinistroRepositorio
    {
        public OrcamentoSinistroRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}