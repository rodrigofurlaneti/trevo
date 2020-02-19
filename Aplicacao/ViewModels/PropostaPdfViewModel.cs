using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class PropostaPdfViewModel
    {
        public string Empresa { get; set; }
        public string Telefone { get; set; }
        public string Filial { get; set; }
        public string Endereco { get; set; }
        public string HorarioFuncionamento { get; set; }
        public string CaminhoImagemLogo { get; set; }
        public string CaminhoImagemTicket { get; set; }
        public bool PossuiValidade { get; set; }
        public TipoPagamentoSelo TipoPagamento { get; set; }

        public string Quantidade { get; set; }
        public string ValorTotal { get; set; }
        public string Periodo { get; set; }
    }
}