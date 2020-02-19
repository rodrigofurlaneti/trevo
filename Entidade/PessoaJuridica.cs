using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entidade.Base;

namespace Entidade
{
    public class PessoaJuridica : BaseEntity
    {
        [Required]
        public virtual string Descricao { get; set; }
        [Required]
        public virtual string CNPJ { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual string InscricaoEstadual { get; set; }
        public virtual string InscricaoMunicipal { get; set; }
        public virtual Endereco Endereco { get; set; }
    }
}
