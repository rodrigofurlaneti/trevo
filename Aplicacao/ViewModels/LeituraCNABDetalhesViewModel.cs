using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class LeituraCNABDetalhesViewModel
    {
        public string NumeroDocumento { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCnpj { get; set; }
        public string Valor { get; set; }
        public StatusLancamentoCobranca Status { get; set; }
    }
}