using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ParametroBoletoBancarioRepositorio : NHibRepository<ParametroBoletoBancario>, IParametroBoletoBancarioRepositorio
    {
        public ParametroBoletoBancarioRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}