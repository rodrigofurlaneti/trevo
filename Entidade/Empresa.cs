using System.Collections.Generic;

namespace Entidade
{
    public class Empresa : PessoaJuridica
    {
	    public virtual IList<EmpresaContato> Contatos { get; set; }
		public virtual Grupo Grupo { get; set; }
    }
}
