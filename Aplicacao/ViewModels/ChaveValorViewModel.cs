namespace Aplicacao.ViewModels
{
    public class ChaveValorViewModel
    {
        public ChaveValorViewModel()
        {
            Id = 0;
            Descricao = string.Empty;
        }

        public ChaveValorViewModel(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}