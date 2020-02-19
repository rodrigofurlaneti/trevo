using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class TipoUnidadeViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public TipoUnidadeViewModel() { }

        public TipoUnidadeViewModel(TipoUnidade tipoUnidade)
        {
            Id = tipoUnidade.Id;
            DataInsercao = tipoUnidade.DataInsercao;
            Codigo = tipoUnidade.Codigo;
            Descricao = tipoUnidade.Descricao;
        }

        public TipoUnidade ToEntity()
        {
            return new TipoUnidade
            {
                Id=Id,
                DataInsercao = DataInsercao,
                Codigo = Codigo,
                Descricao = Descricao
            };
        }

        public TipoUnidadeViewModel ToViewModel(TipoUnidade tipoUnidade)
        {
            return new TipoUnidadeViewModel
            {
                Id = tipoUnidade.Id,
                DataInsercao = tipoUnidade.DataInsercao,
                Codigo = tipoUnidade.Codigo,
                Descricao = tipoUnidade.Descricao
            };
        }
    }
}
