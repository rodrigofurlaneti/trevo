using Entidade.Base;

namespace Entidade
{
    public class CheckListAtividadeTipoAtividade : BaseEntity
    {
        public virtual TipoAtividade TipoAtividade { get; set; }
        public int CheckListAtividade { get; set; }
    }
}
