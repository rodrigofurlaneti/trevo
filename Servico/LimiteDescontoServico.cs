using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ILimiteDescontoServico: IBaseServico<LimiteDesconto>
    {

    }

    public class LimiteDescontoServico : BaseServico<LimiteDesconto, ILimiteDescontoRepositorio>, ILimiteDescontoServico
    {
    }
}
