using Entidade;

namespace Aplicacao.ViewModels
{
    public class TipoFilialViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public TipoFilialViewModel()
        {
            
        }

        public TipoFilialViewModel(TipoFilial tipoFilial)
        {
            Id = tipoFilial?.Id ?? 0;
            Nome = tipoFilial?.Nome;
        }

        public TipoFilial ToEntity() => new TipoFilial
        {
            Id = this.Id,
            Nome = this.Nome
        };
    }
}
