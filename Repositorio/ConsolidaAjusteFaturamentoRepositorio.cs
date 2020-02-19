using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ConsolidaAjusteFaturamentoRepositorio : NHibRepository<ConsolidaAjusteFaturamento>, IConsolidaAjusteFaturamentoRepositorio
    {
        public ConsolidaAjusteFaturamentoRepositorio(NHibContext context) : base(context)
        {

        }

    }
}