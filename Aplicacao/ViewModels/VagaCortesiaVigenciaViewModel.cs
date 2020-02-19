using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class VagaCortesiaVigenciaViewModel
    {
        public int Id { get; set; }
        public Unidade Unidade { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }

        public bool Selecionado { get; set; }

        public string HorarioInicio { get; set; }
        public string HorarioFim { get; set; }

        public VagaCortesiaVigenciaViewModel()
        {


        }

        public VagaCortesiaVigenciaViewModel(VagaCortesiaVigencia VagaCortesiaVigencia)
        {
            Id = VagaCortesiaVigencia.Id;
            Unidade = VagaCortesiaVigencia.Unidade;
            DataInicio = VagaCortesiaVigencia.DataInicio.ToShortDateString();
            DataFim = VagaCortesiaVigencia.DataFim.ToShortDateString();
            HorarioInicio = VagaCortesiaVigencia.HorarioInicio;
            HorarioFim = VagaCortesiaVigencia.HorarioFim;
        }

        public VagaCortesiaVigencia ToEntity()
        {
            var entidade = new VagaCortesiaVigencia
            {
                Id = Id,
                Unidade = Unidade,
                DataInicio = Convert.ToDateTime(DataInicio),
                DataFim = Convert.ToDateTime(DataFim),
                HorarioInicio = HorarioInicio,
                HorarioFim = HorarioFim
            };

            return entidade;
        }
    }
}


