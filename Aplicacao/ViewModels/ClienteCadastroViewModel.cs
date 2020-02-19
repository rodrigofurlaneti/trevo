
namespace Aplicacao.ViewModels
{
    public class ClienteCadastroViewModel
    {
        public int ClienteId { get; set; }
        public ClienteViewModel Cliente { get; set; }

        public ContratoMensalistaViewModel ContratoMensalista { get; set; }

        public EnderecoViewModel EnderecoResidencial { get; set; }

        public EnderecoViewModel EnderecoComercial { get; set; }

        public ContatoViewModel Contato { get; set; }

        public OcorrenciaClienteViewModel Ocorrencia { get; set; }

        public ClienteCadastroViewModel()
        {
            Cliente = new ClienteViewModel();
            Cliente.TipoPessoa = Entidade.Uteis.TipoPessoa.Fisica;
            ContratoMensalista = new ContratoMensalistaViewModel();
            EnderecoComercial = new EnderecoViewModel();
            EnderecoComercial.Cidade = new CidadeViewModel();
            EnderecoResidencial = new EnderecoViewModel();
            EnderecoResidencial.Cidade = new CidadeViewModel();
            Contato = new ContatoViewModel();
            Ocorrencia = new OcorrenciaClienteViewModel();
            Cliente.SeloCliente = new SeloClienteViewModel();
        }
    }
}
