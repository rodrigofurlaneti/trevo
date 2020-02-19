using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CheckListAtividadeRepositorio : NHibRepository<CheckListAtividade>, ICheckListAtividadeRepositorio
    {
        public CheckListAtividadeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}