using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class HorarioUnidadeViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Fixo { get; set; }
        public DateTime DataValidade { get; set; }
        public TipoHorario TipoHorario { get; set; }
        public bool Feriados { get; set; }
        public StatusHorario Status { get; set; }
        public Unidade Unidade { get; set; }
        
        public IList<HorarioUnidadePeriodoHorarioViewModel> PeriodosHorario { get; set; }

        private List<int> ListaIds { get; set; }
        public List<int> ListaTipoHorario
        {
            get
            {

                var retorno = new List<int>();

                if (PeriodosHorario != null)
                    retorno.AddRange(PeriodosHorario.Select(x => (int)x.PeriodoHorario.TipoHorario));

                return ListaIds != null && ListaIds.Any()
                        ? ListaIds
                        : retorno;
            }
            set { ListaIds = value; }
        }

        public HorarioUnidadeViewModel()
        {
            DataValidade = DateTime.Now;
            
        }

        public HorarioUnidadeViewModel(HorarioUnidade HorarioUnidade)
        {
            Id = HorarioUnidade.Id;
            Nome = HorarioUnidade.Nome;
            Fixo = HorarioUnidade.Fixo;
            Unidade = HorarioUnidade.Unidade;
            DataValidade = HorarioUnidade.DataValidade;
            TipoHorario = HorarioUnidade.TipoHorario;
            Feriados = HorarioUnidade.Feriados;
            Status = HorarioUnidade.Status;
            ListaTipoHorario = ListaTipoHorario;
            PeriodosHorario = new HorarioUnidadePeriodoHorarioViewModel().ListaPeriodoHorarios(HorarioUnidade.PeriodosHorario);
        }

        public HorarioUnidade ToEntity()
        {
            var entidade = new HorarioUnidade
            {
                Id = Id,
                Nome = Nome,
                Fixo = Fixo,
                Unidade = Unidade,
                DataValidade = DataValidade,
                TipoHorario = TipoHorario,
                Feriados = Feriados,
                Status = Status,
                ListaTipoHorario = ListaTipoHorario
            };

            return entidade;
        }
    }
}
