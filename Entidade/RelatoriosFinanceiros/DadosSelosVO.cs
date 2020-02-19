using System;

namespace Entidade
{
    public class DadosSelosVO
    {
        public virtual string Unidade { get; set; }
        public virtual string Convenio { get; set; }
        public virtual string Cliente { get; set; }
        public virtual DateTime DataPagamento { get; set; }
        public virtual decimal QuantidadeSelos { get; set; }
        public virtual string Periodo { get; set; }
        public virtual decimal ValorPago { get; set; }
    }
}