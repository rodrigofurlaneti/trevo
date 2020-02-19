using System;
using System.ComponentModel.DataAnnotations;
using Entidade;
namespace Aplicacao.ViewModels
{
    public class MarcaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public string Nome { get; set; }

        public MarcaViewModel() { }

        public MarcaViewModel(Marca marca)
        {
            Id = marca.Id;
            DataInsercao = marca.DataInsercao;
            Nome = marca.Nome;
        }

        public Marca ToEntity()
        {
            return new Marca
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Nome = Nome            
            };
        }

        public MarcaViewModel ToViewModel(Marca marca)
        {
            return new MarcaViewModel
            {
                Id = marca.Id,
                DataInsercao = marca.DataInsercao,
                Nome = marca.Nome
            };
        }
    }
}