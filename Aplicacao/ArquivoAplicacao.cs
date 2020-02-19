using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IArquivoAplicacao : IBaseAplicacao<Arquivo>
    {
    }

    public class ArquivoAplicacao : BaseAplicacao<Arquivo, IArquivoServico>, IArquivoAplicacao
    {
    }
}