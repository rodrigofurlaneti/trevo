using System;

namespace Entidade
{
    public class LeituraCNABPreviaVO
    {
        public string CobrancaId { get; set; }
        public string NossoNumero { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataBaixa { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorDesconto { get; set; }
        public string Cliente { get; set; }
        public string Observacao { get; set; }
        public string MotivoCodigoOcorrencia { get; set; }
        public string CodigoOcorrencia { get; set; }
        public string StatusCobranca { get; set; }
        public string CampoErrosDoRetornoCNAB { get; set; }
        public string Resultado { get; set; }
    }
}