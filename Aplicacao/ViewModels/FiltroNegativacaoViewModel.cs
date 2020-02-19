using System;

namespace Aplicacao.ViewModels
{
    public class FiltroNegativacaoViewModel
    {
        public DateTime DataDe { get; set; }
        public DateTime DataAte { get; set; }
        public int CodigoDestino { get; set; }
        public int CodigoSituacao { get; set; }
    }
}
