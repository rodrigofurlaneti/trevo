using Entidade.Base;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class Movimentacao : BaseEntity
    {
        public virtual int? IdSoftpark { get; set; }
        public virtual int NumFechamento { get; set; }
        public virtual int NumTerminal { get; set; }
        public virtual DateTime DataAbertura { get; set; }
        public virtual DateTime DataFechamento { get; set; }
        public virtual string Ticket { get; set; }
        public virtual string Placa { get; set; }
        public virtual DateTime DataEntrada { get; set; }
        public virtual DateTime DataSaida { get; set; }
        public virtual decimal ValorCobrado { get; set; }
        public virtual string DescontoUtilizado { get; set; }
        public virtual decimal ValorDesconto { get; set; }
        public virtual string TipoCliente { get; set; }
        public virtual string NumeroContrato { get; set; }
        public virtual bool VagaIsenta { get; set; }
        public virtual string Cpf { get; set; }
        public virtual string Rps { get; set; }
        public virtual string FormaPagamento { get; set; }

        public virtual IList<MovimentacaoSelo> MovimentacaoSelo { get; set; }

        public virtual Unidade Unidade { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
