using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Pagamento : BaseEntity, IAudit
    {
        public virtual int NossoNumero { get; set; }

        public virtual DateTime DataPagamento { get; set; }
        public virtual decimal ValorPago { get; set; }

        public virtual TipoDescontoAcrescimo? TipoDescontoAcrescimo { get; set; }
        public virtual decimal? ValorDivergente { get; set; }
        public virtual string Justificativa { get; set; }

        [Required]
        public virtual FormaPagamento FormaPagamento { get; set; }

        public virtual Recebimento Recebimento { get; set; }

        public virtual Unidade Unidade { get; set; }

        public virtual string NumeroRecibo { get; set; }

        public virtual bool StatusPagamento { get; set; }

        public virtual StatusEmissao StatusEmissao { get; set; }
        public virtual DateTime DataEnvio { get; set; }

        public virtual int? PagamentoMensalistaId { get; set; }

        [NotMapped]
        public virtual ContaContabil ContaContabil { get; set; }

        public Pagamento()
        {
            DataPagamento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            DataEnvio = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
    }
}
