namespace Aplicacao.ViewModels
{
    public class PessoaContatoViewModel
    {
        public ContatoViewModel Contato { get; set; }

        public PessoaContatoViewModel()
        {
            Contato = new ContatoViewModel();
        }
    }
}