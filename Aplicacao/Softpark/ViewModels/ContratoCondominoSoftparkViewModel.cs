using Aplicacao.Softpark.ViewModels;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a ContratoMensalista
    /// </summary>
    public class ContratoCondominoSoftparkViewModel : BaseSoftparkViewModel
    {
        public int NumeroVagas { get; set; }
        public IList<ContratoCondominoCarroSoftparkViewModel> Carros { get; set; }
        public EstacionamentoSoftparkViewModel Estacionamento { get; set; }
        public Boolean IsFrota { get; set; }

        public ContratoCondominoSoftparkViewModel()
        {
        }

        public ContratoCondominoSoftparkViewModel(ClienteCondomino condomino)
        {
            Id = condomino.Id;
            DataInsercao = condomino.DataInsercao;
            NumeroVagas = condomino.NumeroVagas;
            Carros = condomino.CondominoVeiculos.Select(x => new ContratoCondominoCarroSoftparkViewModel(this, new CarroSoftparkViewModel(x.Veiculo))).ToList();
            Estacionamento = new EstacionamentoSoftparkViewModel(condomino.Unidade.Unidade);
            IsFrota = condomino.Frota;
        }
    }
}
