using Entidade.Base;
using Entidade.Uteis;
using System.Collections.Generic;

namespace Entidade
{
    public class UnidadeCheckListAtividadeTipoAtividade : BaseEntity
    {
        public virtual TipoAtividade TipoAtividade { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual bool Selecionado { get; set; }
    }
}
