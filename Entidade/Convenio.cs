using Entidade.Base;
using System.Collections.Generic;

namespace Entidade
{
    public class Convenio : BaseEntity
    {
        public Convenio()
        {
            Clientes = new List<Cliente>();
            ConvenioUnidades = new List<ConvenioUnidades>();
        }

        public virtual string Descricao { get; set; }
        public virtual bool Status { get; set; }
        public virtual IList<ConvenioUnidades> ConvenioUnidades { get; set; }
        public virtual IList<Cliente> Clientes { get; set; }
    }
}