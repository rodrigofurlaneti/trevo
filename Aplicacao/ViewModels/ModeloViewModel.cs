using System;
using System.ComponentModel.DataAnnotations;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class ModeloViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public string Descricao { get; set; }
        public MarcaViewModel Marca { get; set; }
        public int Quantidade {get; set;}
        
        public ModeloViewModel()
        {
            Marca = new MarcaViewModel();
        }

        public ModeloViewModel(Modelo modelo)
        {
            Id = modelo?.Id ?? 0;
            DataInsercao = modelo?.DataInsercao ?? DateTime.Now;
            Descricao = modelo?.Descricao;
            Marca = new MarcaViewModel(modelo?.Marca ?? new Marca());
        }

        public Modelo ToEntity()
        {
            return new Modelo{
                Id = Id,
                DataInsercao = DataInsercao != DateTime.MinValue ? DataInsercao : DateTime.Now,
                Descricao= Descricao,
                Marca = Marca?.ToEntity()
            };
        }

      
        public ModeloViewModel ToViewModel(Modelo modelo)
        {
            return new ModeloViewModel
            {
                Id = modelo.Id,
                DataInsercao = modelo.DataInsercao,
                Descricao = modelo.Descricao,
                Marca = new MarcaViewModel(modelo?.Marca ?? new Marca())
            };
        }
    }
}