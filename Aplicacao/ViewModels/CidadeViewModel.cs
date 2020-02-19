using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class CidadeViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; }
        public EstadoViewModel Estado { get; set; }
        public string Descricao { get; set; }

        public CidadeViewModel()
        {
        }

        public override string ToString()
        {
            return $"{Descricao}{(Estado != null && !string.IsNullOrEmpty(Estado.Sigla) ? $"/{Estado.Sigla}" : string.Empty)}";
        }

		public CidadeViewModel(Cidade cidade)
	    {
		    Id = cidade?.Id ?? 0;
		    DataInsercao = cidade?.DataInsercao ?? DateTime.Now;
		    Descricao = cidade?.Descricao;
		    Estado = new EstadoViewModel(cidade?.Estado);
	    }

	    public Cidade ToEntity() => new Cidade
	    {
		    Id = Id,
		    DataInsercao = DataInsercao,
		    Descricao = Descricao,
		    Estado = new Estado
		    {
			    Descricao = Estado?.Descricao,
			    Sigla = Estado?.Sigla
		    }
	    };
    }
}
