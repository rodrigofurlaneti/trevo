using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ArquivoRepositorio : NHibRepository<Arquivo>, IArquivoRepositorio
    {
        public ArquivoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}