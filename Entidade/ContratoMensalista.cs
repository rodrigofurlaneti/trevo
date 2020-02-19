using Entidade.Base;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class ContratoMensalista : BaseEntity, IAudit
    {
        public ContratoMensalista()
        {
            DataVencimento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            DataInicio = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            Veiculos = new List<ContratoMensalistaVeiculo>();
        }

        public virtual Cliente Cliente { get; set; }
        public virtual TipoMensalista TipoMensalista { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual DateTime DataVencimento { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual int NumeroContrato { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual int NumeroVagas { get; set; }
        public virtual bool Frota { get; set; }

        public virtual string HorarioFim { get; set; }
        public virtual string HorarioInicio { get; set; }

        public virtual string Observacao { get; set; }
        
        public virtual IList<ContratoMensalistaVeiculo> Veiculos { get; set; }

        public virtual IList<LancamentoCobranca> LancamentoCobrancas { get; set; }

        public virtual IList<ContratoMensalistaNotificacao> ContratoMensalistaNotificacoes { get; set; }

        public virtual TabelaPrecoMensalista TabelaPrecoMensalista { get; set; }

        //Não Mapear
        public virtual string NumeroRecibo { get; set; }
        public virtual decimal ValorPago { get; set; }
        public virtual bool PagamentoCadastro { get; set; }
    }
}
