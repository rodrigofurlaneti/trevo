using Core.Extensions;
using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a ClienteUnidade
    /// </summary>
    public class CondutorEstacionamentoSoftparkViewModel : BaseSoftparkViewModel
    {
        public int CondutorId { get; set; }
        public CondutorSoftparkViewModel Condutor { get; set; }
        public int EstacionamentoId { get; set; }
        public EstacionamentoSoftparkViewModel Estacionamento { get; set; }

        public CondutorEstacionamentoSoftparkViewModel()
        {
        }

        public CondutorEstacionamentoSoftparkViewModel(ClienteUnidade clienteUnidade, CondutorSoftparkViewModel condutor)
        {
            Id = clienteUnidade.Id;
            DataInsercao = clienteUnidade.DataInsercao;
            CondutorId = condutor.Id;
            Condutor = condutor;
            EstacionamentoId = clienteUnidade.Unidade.Id;
            Estacionamento = new EstacionamentoSoftparkViewModel(clienteUnidade.Unidade);
        }
    }
}
