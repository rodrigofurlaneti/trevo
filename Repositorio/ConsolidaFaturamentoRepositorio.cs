using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ConsolidaFaturamentoRepositorio : NHibRepository<ConsolidaFaturamento>, IConsolidaFaturamentoRepositorio
    {
        public ConsolidaFaturamentoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
