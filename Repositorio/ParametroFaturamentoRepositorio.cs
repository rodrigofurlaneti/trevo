using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ParametroFaturamentoRepositorio : NHibRepository<ParametroFaturamento>, IParametroFaturamentoRepositorio
    {
        public ParametroFaturamentoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}