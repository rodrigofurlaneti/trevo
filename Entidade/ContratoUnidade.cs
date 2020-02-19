using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class ContratoUnidade : BaseEntity
    {
        public ContratoUnidade()
        {
            InicioContrato = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            FinalContrato = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        public virtual Unidade Unidade { get; set; }
        public virtual TipoContrato TipoContrato { get; set; }
        public virtual string NumeroContrato { get; set; }
        public virtual int DiaVencimento { get; set; }
        public virtual int InformarVencimentoDias { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual TipoValor TipoValor { get; set; }
        public virtual DateTime InicioContrato { get; set; }
        public virtual DateTime FinalContrato { get; set; }
        public virtual bool ExistiraReajuste { get; set; }
        public virtual IndiceReajuste IndiceReajuste { get; set; }
        public virtual bool Ativo { get; set; }
        

        public virtual IList<ContratoUnidadeContasAPagar> ContratoUnidadeContasAPagar { get; set; }
        //public virtual IList<Parcela> Parcelas { get; set; }

    }
}