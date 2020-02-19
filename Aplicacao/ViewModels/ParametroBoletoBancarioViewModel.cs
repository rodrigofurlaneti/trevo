using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class ParametroBoletoBancarioViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public TipoServico TipoServico { get; set; }
        public int DiasAntesVencimento { get; set; }
        public string ValorDesconto { get; set; }
        public UnidadeViewModel Unidade { get; set; }

        public List<ParametroBoletoBancarioDescritivoViewModel> ParametroBoletoBancarioDescritivos { get; set; }

        public ParametroBoletoBancarioViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}
