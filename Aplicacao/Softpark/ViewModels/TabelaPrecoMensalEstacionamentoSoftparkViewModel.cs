namespace Aplicacao.ViewModels
{
    public class TabelaPrecoMensalEstacionamentoSoftparkViewModel : BaseSoftparkViewModel
    {
        public int TabelaPrecoMensalId { get; set; }
        public TabelaPrecoMensalSoftparkViewModel TabelaPrecoMensal { get; set; }
        public int EstacionamentoId { get; set; }
        public EstacionamentoSoftparkViewModel Estacionamento { get; set; }
        public int DiasParaCorte { get; set; }
    }
}
