using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class EstadoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public PaisViewModel Pais { get; set; }
        public bool AssociaMaterialCampanha { get; set; }

        public EstadoViewModel()
        {
        }

        public EstadoViewModel(Estado estado)
        {
            Id = estado?.Id ?? 0;
            DataInsercao = estado?.DataInsercao ?? DateTime.Now;
            Descricao = estado?.Descricao;
            Sigla = estado?.Sigla;
            Pais = new PaisViewModel(estado?.Pais);
            AssociaMaterialCampanha = estado?.AssociaMaterialCampanha??false;
        }
    }
}
