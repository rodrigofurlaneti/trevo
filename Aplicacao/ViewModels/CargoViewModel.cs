using Entidade;

namespace Aplicacao.ViewModels
{
    public class CargoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public CargoViewModel() { }

        public CargoViewModel(Cargo entidade)
        {
            Id = entidade?.Id ?? 0;
            Nome = entidade?.Nome ?? string.Empty;
        }

        public Cargo ToEntity()
        {
            return new Cargo
            {
                Id = Id,
                Nome = Nome
            };
        }

        public CargoViewModel ToViewModel(Cargo entidade)
        {
            return new CargoViewModel
            {
                Id = entidade.Id,
                Nome = entidade.Nome
            };
        }
    }
}