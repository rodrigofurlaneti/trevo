using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IDocumentoAplicacao : IBaseAplicacao<Documento>
    {
    }

    public class DocumentoAplicacao : BaseAplicacao<Documento, IDocumentoServico>, IDocumentoAplicacao
    {
    }
}