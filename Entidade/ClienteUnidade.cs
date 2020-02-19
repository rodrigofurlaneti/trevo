using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class ClienteUnidade
    {
        public virtual Unidade Unidade { get; set; }
        public virtual int Cliente { get; set; }
    }
}
