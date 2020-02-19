using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class FuncionamentoRepositorio : NHibRepository<Funcionamento>, IFuncionamentoRepositorio
    {
        public FuncionamentoRepositorio(NHibContext context)
          : base(context)
        {
        }
    }
}
