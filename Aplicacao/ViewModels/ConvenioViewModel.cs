using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ConvenioViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public bool Status { get; set; }
        public IList<ConvenioUnidadeViewModel> ConvenioUnidade { get; set; }
        public IList<ClienteViewModel> ConvenioCliente { get; set; }

        public ConvenioViewModel()
        {
            ConvenioUnidade = new List<ConvenioUnidadeViewModel>();
            ConvenioCliente = new List<ClienteViewModel>();
        }

        public ConvenioViewModel(Convenio convenio)
            : this()
        {
            if (convenio != null)
            {
                Id = convenio.Id;
                DataInsercao = convenio.DataInsercao;
                Descricao = convenio.Descricao;
                //Valor
                Status = convenio.Status;
                ConvenioUnidade = convenio?.ConvenioUnidades?.Select(x => new ConvenioUnidadeViewModel(x)).ToList();
                ConvenioCliente = convenio?.Clientes?.Select(x => new ClienteViewModel(x)).ToList();
            }
        }

        public Convenio ToEntity()
        {
            return new Convenio
            {
                //ConvenioUnidades
                DataInsercao = DataInsercao,
                Descricao = Descricao,
                Id = Id,
                Status = Status
            };
        }
    }
}