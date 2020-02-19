namespace Aplicacao.ViewModels
{
    public class ParametroNumeroNotaFiscalViewModel
    {   
        public  int Id { get; set; }
        public string  ValorMaximoNota { get; set; }
        public string ValorMaximoNotaDia { get; set; }
        public UnidadeViewModel Unidade { get; set; }

        public ParametroNumeroNotaFiscalViewModel()
        {

        }
    }
}
