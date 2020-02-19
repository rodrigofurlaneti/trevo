namespace Aplicacao.ViewModels
{
    public class ChaveValorCategoriaViewModel
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }

        public ChaveValorCategoriaViewModel()
        {

        }

        public ChaveValorCategoriaViewModel(string Id, string Descricao, string Categoria)
        {
            this.Id = Id;
            this.Descricao = Descricao;
            this.Categoria = Categoria;
        }
    }
}