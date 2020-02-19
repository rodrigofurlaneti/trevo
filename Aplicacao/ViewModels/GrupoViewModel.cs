using Entidade;

namespace Aplicacao.ViewModels
{
    public class GrupoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public GrupoViewModel()
        {

        }

        public GrupoViewModel(Grupo grupo)
        {
            Id = grupo?.Id ?? 0;
            Nome = grupo?.Nome;
        }

        public Grupo ToEntity() => new Grupo
        {
            Id = this.Id,
            Nome = this.Nome
        };
    }
}