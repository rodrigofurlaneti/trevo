using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ContratoMensalistaVeiculoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ContratoMensalistaViewModel Contrato { get; set; }
        public VeiculoViewModel Veiculo { get; set; }
       
        public ContratoMensalistaVeiculoViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}