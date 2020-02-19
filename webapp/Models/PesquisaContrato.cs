using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entidade.Uteis;

namespace Portal.Models
{
    public class PesquisaContrato
    {
        public bool Atrelados { get; set; }
        public string Contrato { get; set; }
        public int PeriodoAtrasoDe { get; set; }
        public int PeriodoAtrasoAte { get; set; }
        public int ValDividaDe { get; set; }
        public int ValDividaAte { get; set; }
        public int ParcelaDe { get; set; }
        public int ParcelaAte { get; set; }
        public int QtdParcelaDe { get; set; }
        public int QtdParcelaAte { get; set; }
        public int TaxaDe { get; set; }
        public int TaxaAte { get; set; }

        public ComSemCobranca ComSemCobranca { get; set; }
        public string TotalPesquisado { get; set; }
    }
}