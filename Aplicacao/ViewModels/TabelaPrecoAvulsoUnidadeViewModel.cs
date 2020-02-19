using Entidade;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoAvulsoUnidadeViewModel
    {
        public UnidadeViewModel Unidade { get; set; }
        public string Vigencia { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFim { get; set; }
        public string ValorDiaria { get; set; }

        public TabelaPrecoAvulsoUnidadeViewModel() { }

        public TabelaPrecoAvulsoUnidadeViewModel(Unidade unidade, string horaInicio, string horaFim, string valorDiaria)
        {
            if (unidade != null)
            {
                Unidade = new UnidadeViewModel(unidade);
                HoraInicio = horaInicio;
                HoraFim = horaFim;
                Vigencia = $"{horaInicio} - {horaFim}";
                ValorDiaria = valorDiaria;
            }
        }

        public TabelaPrecoAvulsoUnidadeViewModel(UnidadeViewModel unidade, string horaInicio, string horaFim, string valorDiaria)
        {
            if (unidade != null)
            {
                Unidade = unidade;
                Vigencia = $"{horaInicio} - {horaFim}";
                ValorDiaria = valorDiaria;
                HoraInicio = horaInicio;
                HoraFim = horaFim;
            }
        }
    }
}