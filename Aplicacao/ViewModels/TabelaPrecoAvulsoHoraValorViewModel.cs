using Entidade;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoAvulsoHoraValorViewModel
    {
        public string Hora { get; set; }
        public string Valor { get; set; }

        public TabelaPrecoAvulsoHoraValorViewModel() { }

        public TabelaPrecoAvulsoHoraValorViewModel(TabelaPrecoAvulsoHoraValor entidade)
        {
            if (entidade != null)
            {
                Hora = entidade.Hora;
                Valor = entidade.Valor.ToString();
            }
        }

        public TabelaPrecoAvulsoHoraValorViewModel(string hora, string valor)
        {
            Hora = hora;
            Valor = valor;
        }
    }
}