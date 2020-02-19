using System.Collections.Generic;
using Entidade.Base;

namespace Entidade
{
    public class Trabalho : BaseEntity
    {
        public virtual string Empresa {get; set;}
        public virtual IList<TrabalhoContato> Contatos { get; set; }
        public virtual IList<TrabalhoEndereco> Enderecos {get; set;}
        public virtual string Profissao {get; set;}
        public virtual string Cargo {get; set;}
    }
}