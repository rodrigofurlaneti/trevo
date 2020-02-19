using Entidade.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class PrecoStatus : BaseEntity
    {
        public virtual string Descricao { get; set; }
    }
}
