using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class FeriasClienteDetalheViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public DateTime DataInicio { get; set; }
        public string DataInicioStr
        {
            get
            {
                return DataInicio.ToShortDateString();
            }
        }
        public DateTime DataFim { get; set; }
        public string DataFimStr
        {
            get
            {
                return DataFim.ToShortDateString();
            }
        }

        public DateTime DataCompetencia
        {
            get
            {
                return new DateTime(DataFim.AddMonths(1).Year, DataFim.AddMonths(1).Month, 1);
            }
        }
        
        public decimal ValorFeriasCalculada { get; set; }

        public decimal ValorFeriasCalculadaAnterior { get; set; }

        public bool IsEdited { get; set; }

        public FeriasClienteDetalheViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public FeriasClienteDetalheViewModel(FeriasClienteDetalhe obj)
        {
            DataInicio = obj?.DataInicio ?? DateTime.Now;
            DataFim = obj?.DataFim ?? DateTime.Now;
            ValorFeriasCalculada = obj?.ValorFeriasCalculada ?? 0;
            ValorFeriasCalculadaAnterior = obj?.ValorFeriasCalculadaAnterior ?? 0;
        }

        public FeriasClienteDetalhe ToEntity() => new FeriasClienteDetalhe()
        {
            DataInicio = this.DataInicio,
            DataFim = this.DataFim,
            ValorFeriasCalculada = this.ValorFeriasCalculada,
            ValorFeriasCalculadaAnterior = this.ValorFeriasCalculadaAnterior
        };
    }
}
