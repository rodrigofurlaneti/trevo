using System;
using System.IO;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class ParametroFaturamento : BaseEntity
    {
        public virtual Empresa Empresa { get; set; }

        public virtual string Descricao { get; set; }
       
        public virtual Banco Banco { get; set; }
       
        public virtual string Agencia { get; set; }
       
        public virtual string DigitoAgencia { get; set; }
       
        public virtual string Conta { get; set; }
       
        public virtual string DigitoConta { get; set; }
       
        public virtual decimal SaldoInicial { get; set; }

        public virtual string Convenio { get; set; }

        public virtual string Carteira { get; set; }

        public virtual string CodigoTransmissao { get; set; }
    }
}