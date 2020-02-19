using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Entidade
{
    public class PedidoSelo : BaseEntity
    {
        public virtual Cliente Cliente { get; set; }
        public virtual Convenio Convenio { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual TipoPagamentoSelo TiposPagamento { get; set; }
        public virtual Desconto Desconto { get; set; }

        public virtual DateTime ValidadePedido { get; set; }
        public virtual TipoSelo TipoSelo { get; set; }
        public virtual int Quantidade { get; set; }

        [NotMapped]
        public virtual StatusPedidoSelo StatusPedido
        {
            get
            {
                //if (PedidoSeloHistorico == null || !PedidoSeloHistorico.Any())
                //    return StatusPedidoSelo.Rascunho;

                return PedidoSeloHistorico.OrderBy(x => x.DataInsercao).LastOrDefault().StatusPedidoSelo;
            }
        }

        [NotMapped]
        public virtual LancamentoCobranca UltimoLancamento
        {
            get
            {
                if (LancamentoCobranca == null || !LancamentoCobranca.Any())
                    return null;

                return LancamentoCobranca.OrderBy(x => x.DataInsercao).LastOrDefault();
            }
        }

        public virtual int DiasVencimento { get; set; }

        public virtual DateTime DataVencimento { get; set; }

        public virtual TipoPedidoSelo TipoPedidoSelo { get; set; }
        public virtual EmissaoSelo EmissaoSelo { get; set; }
        public virtual Proposta Proposta { get; set; }
        public virtual Usuario Usuario { get; set; }
                       
        public virtual IList<PedidoSeloNotificacao> Notificacoes { get; set; }
        public virtual IList<PedidoSeloEmail> PedidoSeloEmails { get; set; }
        public virtual IList<PedidoSeloHistorico> PedidoSeloHistorico { get; set; }
        public virtual IList<LancamentoCobrancaPedidoSelo> LancamentoCobranca { get; set; }

        public PedidoSelo()
        {
            ValidadePedido = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            DataVencimento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
    }
}