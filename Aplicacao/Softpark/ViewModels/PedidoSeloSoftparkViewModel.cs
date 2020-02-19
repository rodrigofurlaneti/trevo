using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a PedidoSelo
    /// </summary>
    public class PedidoSeloSoftparkViewModel : BaseSoftparkViewModel
    {
        public PedidoSeloSoftparkViewModel()
        {
        }

        public PedidoSeloSoftparkViewModel(PedidoSelo PedidoSelo)
        {
            Id = PedidoSelo.Id;
            DataInsercao = PedidoSelo.DataInsercao;
        }
    }
}
