using Entidade.Base;
using System;

namespace Entidade
{
    public class PagamentoMensalidade : BaseEntity
    {
        public virtual int NumFechamento { get; set; }
        public virtual int NumTerminal { get; set; }
        public virtual DateTime DataAbertura { get; set; }
        public virtual DateTime DataFechamento { get; set; }
        public virtual DateTime DataRecebimento { get; set; }
        public virtual string NumContratoMensalista { get; set; }
        public virtual decimal ValorRecebido { get; set; }
        public virtual int NumCobranca { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual LancamentoCobranca LancamentoCobranca { get; set; }

        public PagamentoMensalidade()
        {
            DataAbertura = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            DataFechamento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            DataRecebimento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
    }
}
