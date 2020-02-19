using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class HorarioParametroEquipeViewModel
    {
        public int Id { get; set; }
        public Unidade Unidade { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string HorarioInicio { get; set; }
        public string HorarioFim { get; set; }


        public HorarioParametroEquipeViewModel()
        {
            

        }

        public HorarioParametroEquipeViewModel(HorarioParametroEquipe HorarioParametroEquipe)
        {
            Id = HorarioParametroEquipe.Id;
            Unidade = HorarioParametroEquipe.Unidade;
            DataInicio = HorarioParametroEquipe.DataInicio;
            DataFim = HorarioParametroEquipe.DataFim;
            HorarioInicio = HorarioParametroEquipe.HorarioInicio;
            HorarioFim = HorarioParametroEquipe.HorarioFim;
        }

        public HorarioParametroEquipe ToEntity()
        {
            var entidade = new HorarioParametroEquipe
            {
                Id = Id,
                Unidade = Unidade,
                DataInicio = DataInicio,
                DataFim = DataFim,
                HorarioInicio = HorarioInicio,
                HorarioFim = HorarioFim
            };

            return entidade;
        }
    }
}


