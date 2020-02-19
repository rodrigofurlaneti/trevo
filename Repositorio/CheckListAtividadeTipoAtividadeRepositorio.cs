using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CheckListAtividadeTipoAtividadeRepositorio : NHibRepository<CheckListAtividadeTipoAtividade>, ICheckListAtividadeTipoAtividadeRepositorio
    {
        public CheckListAtividadeTipoAtividadeRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}