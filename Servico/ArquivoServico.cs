using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IArquivoServico : IBaseServico<Arquivo>
    {
    }

    public class ArquivoServico : BaseServico<Arquivo, IArquivoRepositorio>, IArquivoServico
    {
    }
}