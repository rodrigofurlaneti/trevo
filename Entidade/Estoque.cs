using Entidade.Base;
using Entidade.Uteis;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Estoque : BaseEntity
    {
        [Required]
        public virtual string Nome { get; set; }
        [Required, StringLength(10)]
        public virtual string Cep { get; set; }

        [StringLength(200)]
        public virtual string Logradouro { get; set; }

        [StringLength(10)]
        public virtual string Numero { get; set; }

        [StringLength(50)]
        public virtual string Complemento { get; set; }

        [StringLength(100)]
        public virtual string Bairro { get; set; }
        public virtual TipoEndereco Tipo { get; set; }
        public virtual string CidadeNome { get; set; }
        public virtual string UF { get; set; }
        public virtual bool EstoquePrincipal { get; set; }

        public virtual Unidade Unidade { get; set; }
    }
}