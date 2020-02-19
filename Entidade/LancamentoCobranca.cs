using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Entidade
{
    public class LancamentoCobranca : BaseEntity, IAudit
    {
        [Required]
        [Description("Geracao")]
        public virtual DateTime DataGeracao { get; set; }
        [Required]
        [Description("Vencimento")]
        public virtual DateTime DataVencimento { get; set; }
        [Description("Competencia")]
        public virtual DateTime? DataCompetencia { get; set; }
        [Required]
        public virtual DateTime? DataBaixa { get; set; }
        [Required]
        public virtual StatusLancamentoCobranca StatusLancamentoCobranca { get; set; }
        [Required]
        public virtual decimal ValorContrato { get; set; }
        public virtual bool PossueCnab { get; set; }
        public virtual TipoValor TipoValorMulta { get; set; }
        public virtual decimal ValorMulta { get; set; }
        public virtual TipoValor TipoValorJuros { get; set; }
        public virtual decimal ValorJuros { get; set; }
        public virtual string CiaSeguro { get; set; }
        public virtual Recebimento Recebimento { get; set; }
        public virtual ContaFinanceira ContaFinanceira { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual TipoServico TipoServico { get; set; }
        public virtual int NossoNumero { get; set; }
        public virtual decimal ValorTotal => DateTime.Now.Date > DataVencimento.Date ? ValorContrato + ValorJuros + ValorMulta : ValorContrato;

        public virtual string Observacao { get; set; }

        //public virtual bool Proporcional { get; set; }

        public virtual string ValorRecebido
        {
            get
            {
                if (Recebimento != null
                    && Recebimento.StatusRecebimento != StatusRecebimento.Estornado)
                {
                    var listaPagamento =
                        Recebimento.Pagamentos.ToList();
                    var valorTotal = listaPagamento.Sum(pagamento => pagamento.ValorPago + pagamento.ValorDivergente ?? 0);
                    
                    return valorTotal.ToString(Constantes.MascaraMonetaria);
                }

                return "0,00";
            }
        }

        public virtual IList<LancamentoCobrancaNotificacao> LancamentoCobrancaNotificacoes { get; set; }

        public virtual decimal ValorAReceber
        {
            get { return ValorTotal - Convert.ToDecimal(ValorRecebido); }
        }
        
        public LancamentoCobranca()
        {
            DataGeracao = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            DataVencimento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
        
        public virtual TipoOcorrenciaRetorno TipoOcorrenciaRetorno { get; set; }

        //Não Mapear
        public virtual string NumerosContratos { get; set; }
        public virtual IList<LancamentoCobranca> ListaCobrancaTipoServico { get; set; }
    }
}