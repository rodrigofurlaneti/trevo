using Entidade.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class Faturamento : BaseEntity
    {
        public virtual int? IdSoftpark { get; set; }
        public virtual string NomeUnidade { get; set; }
        public virtual int NumFechamento { get; set; }
        public virtual int NumTerminal { get; set; }
        public virtual DateTime DataAbertura { get; set; }
        public virtual DateTime DataFechamento { get; set; }
        public virtual string TicketInicial { get; set; }
        public virtual string TicketFinal { get; set; }
        public virtual string PatioAtual { get; set; }
        public virtual decimal? ValorTotal { get; set; }
        public virtual decimal? ValorRotativo { get; set; }
        public virtual decimal? ValorRecebimentoMensalidade { get; set; }
        public virtual decimal? ValorDinheiro { get; set; }
        public virtual decimal? ValorCartaoDebito { get; set; }
        public virtual decimal? ValorCartaoCredito { get; set; }
        public virtual decimal? ValorSemParar { get; set; }
        public virtual decimal? ValorSeloDesconto { get; set; }
        public virtual decimal? SaldoInicial { get; set; }
        public virtual decimal? ValorSangria { get; set; }

        public virtual Unidade Unidade { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
