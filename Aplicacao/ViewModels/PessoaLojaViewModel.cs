namespace Aplicacao.ViewModels
{
    public class PessoaLojaViewModel
    {
        public virtual EmpresaViewModel Loja { get; set; }
        
        public PessoaLojaViewModel()
        {
            Loja = new EmpresaViewModel();
        }
    }
}