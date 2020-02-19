using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IParametrizacaoLocacaoServico : IBaseServico<ParametrizacaoLocacao>
    {
    }

    public class ParametrizacaoLocacaoServico : BaseServico<ParametrizacaoLocacao, IParametrizacaoLocacaoRepositorio>, IParametrizacaoLocacaoServico
    {
    }
}