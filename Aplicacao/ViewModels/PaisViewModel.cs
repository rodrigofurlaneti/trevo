using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class PaisViewModel
    {
        public int Id { get; private set; }
        public DateTime DataInsercao { get; }
        public string Descricao { get; set; }

        public PaisViewModel()
        {
        }

        public PaisViewModel(Pais pais)
        {
            Id = pais?.Id ?? 0;
            DataInsercao = pais?.DataInsercao ?? DateTime.Now;
            Descricao = pais?.Descricao;
        }
    }
}