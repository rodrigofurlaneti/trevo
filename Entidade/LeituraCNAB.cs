using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class LeituraCNAB : BaseEntity, IAudit
    {
        public virtual string NomeArquivo { get; set; }
       
        public virtual string CodigoBanco { get; set; }
        public virtual string Agencia { get; set; }
        public virtual string Conta { get; set; }
        public virtual string DACConta { get; set; }

        public virtual string NumeroCNAB { get; set; }
        
        public virtual decimal ValorTotal { get; set; }

        //public virtual Unidade Unidade { get; set; }

        public virtual DateTime DataGeracao { get; set; }
        public virtual DateTime DataCredito { get; set; }

        public virtual IList<LeituraCNABLancamentoCobranca> ListaLancamentos { get; set; }

        public virtual byte[] Arquivo { get; set; }
        public virtual ContaFinanceira ContaFinanceira { get; set; }

        public LeituraCNAB()
        {
            DataGeracao = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            DataCredito = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
    }
}
