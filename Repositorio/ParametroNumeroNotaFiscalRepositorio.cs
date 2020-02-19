using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ParametroNumeroNotaFiscalRepositorio : NHibRepository<ParametroNumeroNotaFiscal>, IParametroNumeroNotaFiscalRepositorio
    {
        public ParametroNumeroNotaFiscalRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
