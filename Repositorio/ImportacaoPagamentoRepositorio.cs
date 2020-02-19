using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ImportacaoPagamentoRepositorio : NHibRepository<ImportacaoPagamento>, IImportacaoPagamentoRepositorio
    {
        public ImportacaoPagamentoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}