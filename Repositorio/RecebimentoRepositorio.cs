using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class RecebimentoRepositorio : NHibRepository<Recebimento>, IRecebimentoRepositorio
    {
        public RecebimentoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}