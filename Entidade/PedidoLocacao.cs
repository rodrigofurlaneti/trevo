using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entidade
{
    public class PedidoLocacao : BaseEntity, IBaseNotificacao, IAudit
    {
        public virtual Unidade Unidade { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual TipoLocacao TipoLocacao { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual decimal ValorTotal { get; set; }
        public virtual Desconto Desconto { get; set; }
        public virtual bool PossuiFiador { get; set; }
        public virtual string NomeFiador { get; set; }
        public virtual string FormaGarantia { get; set; }
        public virtual DateTime DataReajuste { get; set; }
        public virtual TipoReajuste TipoReajuste { get; set; }
        public virtual decimal ValorReajuste { get; set; }
        public virtual PrazoReajuste PrazoReajuste { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual DateTime DataPrimeiroPagamento { get; set; }
        public virtual decimal ValorPrimeiroPagamento { get; set; }
        public virtual DateTime DataDemaisPagamentos { get; set; }
        public virtual int CicloPagamentos { get; set; }
        public virtual DateTime DataVigenciaInicio { get; set; }
        public virtual DateTime DataVigenciaFim { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual bool PossuiCicloMensal { get; set; }

        public virtual IList<PedidoLocacaoLancamentoAdicional> PedidoLocacaoLancamentosAdicionais { get; set; }
        public virtual IList<PedidoLocacaoNotificacao> PedidoLocacaoNotificacoes { get; set; }
        
        public virtual DateTime DataVencimentoNotificacao { get => DataVigenciaFim; set => DataVencimentoNotificacao = value; }
        public virtual StatusSolicitacao Status { get; set; }

        public virtual bool Antecipado { get; set; }
        public virtual string RamoAtividade { get; set; }
        public virtual bool PrazoContratoDeterminado { get; set; }
        public virtual decimal ValorDeposito { get; set; }

        public virtual void AdicionarLancamentoAdicional(PedidoLocacaoLancamentoAdicional lancamentoAdicional)
        {
            if(!PedidoLocacaoLancamentosAdicionais.Any(x => x.Id == lancamentoAdicional.Id))
                lancamentoAdicional.AssociarPedidoLocacao(this);
        }
    }
}