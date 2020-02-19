using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class UnidadeCheckListAtividade : BaseEntity
    {
        public virtual CheckListAtividade CheckListAtividade { get; set; }
        public virtual StatusCheckList StatusCheckList { get; set; }
        public virtual int Unidade { get; set; }
    }
}
