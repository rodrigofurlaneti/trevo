using Core.Extensions;
using Entidade;
using System;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoConfiguracaoDiariaSoftparkViewModel : BaseSoftparkViewModel
    {
        public int DiaSemana { get; set; }

        public int HoraInicial { get; set; }

        public int HoraFinal { get; set; }
        
        public virtual int TabelaPrecoId { get; set; }

        public TabelaPrecoConfiguracaoDiariaSoftparkViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public TabelaPrecoConfiguracaoDiariaSoftparkViewModel(Funcionamento funcionamento, int unidadeId, int tabelaPrecoId)
        {
            
        }
    }
}
