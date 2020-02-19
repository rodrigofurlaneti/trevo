using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class ConvenioUnidadeViewModel
    {
        public int Id { get; set; }
        public int IdTeste { get; set; }
        public DateTime DataInsercao { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public IList<ClienteViewModel> Clientes { get; set; }
        public TipoSeloViewModel TipoSelo { get; set; }
        public decimal Valor { get; set; }

        public ConvenioUnidadeViewModel()
        {
            Clientes = new List<ClienteViewModel>();
        }

        public ConvenioUnidadeViewModel(ConvenioUnidades entidade)
            : this(entidade.ConvenioUnidade)
        {

        }

        public ConvenioUnidadeViewModel(ConvenioUnidade entidade)
            : this()
        {
            if (entidade != null)
            {
                Id = entidade.Id;
                //IdTeste
                DataInsercao = entidade.DataInsercao;
                Unidade = new UnidadeViewModel(entidade.Unidade);
                TipoSelo = new TipoSeloViewModel(entidade.TipoSelo);
                Valor = entidade.Valor;
            }
        }
    }
}