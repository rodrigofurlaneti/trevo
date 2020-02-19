using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IFuncionamentoServico : IBaseServico<Funcionamento>
    {

    }
    public class FuncionamentoServico : BaseServico<Funcionamento,IFuncionamentoRepositorio>,IFuncionamentoServico
    {

    }
}
