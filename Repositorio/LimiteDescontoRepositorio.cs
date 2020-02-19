using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class LimiteDescontoRepositorio : NHibRepository<LimiteDesconto>, ILimiteDescontoRepositorio
    {
        public LimiteDescontoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
