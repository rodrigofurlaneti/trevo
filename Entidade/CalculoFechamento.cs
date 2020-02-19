using Entidade.Base;
using System;

namespace Entidade
{
    public class CalculoFechamento : BaseEntity
    {
        public CalculoFechamento()
        {
            DataInsercao = DateTime.Now; 
        }
        public virtual bool PrefeituraMaiorIgualCartao { get; set; }
        public virtual bool ValorComplementarEmitido { get; set; }
        public virtual bool PrefeituraComplementarMaiorIgualDespesa { get; set; }
        public virtual decimal ValorNotaEmissao { get; set; }
        public virtual ConsolidaAjusteFaturamento AjusteFaturamento { get; set; }
    }
}
