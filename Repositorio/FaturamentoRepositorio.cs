using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class FaturamentoRepositorio : NHibRepository<Faturamento>, IFaturamentoRepositorio
    {
        public FaturamentoRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}
