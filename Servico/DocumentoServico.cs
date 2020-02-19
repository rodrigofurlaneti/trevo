using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IDocumentoServico : IBaseServico<Documento>
    {
    }

    public class DocumentoServico : BaseServico<Documento, IDocumentoRepositorio>, IDocumentoServico
    {
    }
}