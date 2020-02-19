using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class PeriodoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public PeriodoViewModel() { }

        public PeriodoViewModel(Periodo periodo)
        {
            Id = periodo.Id;
            DataInsercao = periodo.DataInsercao;
            Codigo = periodo.Codigo;
            Descricao = periodo.Descricao;
        }

        public Periodo ToEntity()
        {
            return new Periodo
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Codigo = Codigo,
                Descricao = Descricao
            };
        }

        public PeriodoViewModel ToViewModel(Periodo periodo)
        {
            return new PeriodoViewModel
            {
                Id = periodo.Id,
                DataInsercao = periodo.DataInsercao,
                Codigo = periodo.Codigo,
                Descricao = periodo.Descricao
            };
        }
    }
}
