using Entidade.Base;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class ContaFinanceira : BaseEntity
    {
        [Required(ErrorMessage = "*")]
        public virtual string Agencia { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string DigitoAgencia { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Conta { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string DigitoConta { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Cpf { get; set; }

        public virtual string Cnpj { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Descricao { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual Banco Banco { get; set; }
        
        public virtual string Convenio { get; set; }

        public virtual string Carteira { get; set; }

        public virtual string CodigoTransmissao { get; set; }

        public virtual bool ContaPadrao { get; set; }

        public virtual Empresa Empresa{ get; set; }

        public virtual string ConvenioPagamento { get; set; }
    }
}
