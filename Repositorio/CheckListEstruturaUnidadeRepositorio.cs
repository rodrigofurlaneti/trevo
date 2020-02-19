using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CheckListEstruturaUnidadeRepositorio : NHibRepository<CheckListEstruturaUnidade>, ICheckListEstruturaUnidadeRepositorio
    {
        public CheckListEstruturaUnidadeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}