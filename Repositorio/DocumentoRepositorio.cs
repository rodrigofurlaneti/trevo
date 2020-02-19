using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class DocumentoRepositorio : NHibRepository<Documento>, IDocumentoRepositorio
    {
        public DocumentoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}