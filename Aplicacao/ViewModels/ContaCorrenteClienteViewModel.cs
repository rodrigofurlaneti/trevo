using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ContaCorrenteClienteViewModel
    {
        public int ContaCorrenteClienteId { get; set; }
        public DateTime DataInsercao { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public List<ContaCorrenteClienteDetalheViewModel> ContaCorrenteClienteDetalhes { get; set; }

        public ContaCorrenteClienteDetalheViewModel ContaCorrenteClienteDetalhe { get; set; }

        public ContaCorrenteClienteViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ContaCorrenteClienteViewModel(ContaCorrenteCliente obj)
        {
            if (obj != null)
            {
                ContaCorrenteClienteId = obj.Id;
                DataInsercao = obj.DataInsercao;
                Cliente = new ClienteViewModel(obj?.Cliente ?? new Cliente());
                ContaCorrenteClienteDetalhes = obj?.ContaCorrenteClienteDetalhes?.Select(x => new ContaCorrenteClienteDetalheViewModel(x))?.ToList() ?? new List<ContaCorrenteClienteDetalheViewModel>();
            }
        }

        public ContaCorrenteCliente ToEntity()
        {
            var obj = new ContaCorrenteCliente();
            obj.Id = ContaCorrenteClienteId;
            obj.DataInsercao = DataInsercao;
            obj.Cliente = Cliente?.ToEntity();
            obj.ContaCorrenteClienteDetalhes = ContaCorrenteClienteDetalhes?.Select(x => x.ToEntity())?.ToList() ?? new List<ContaCorrenteClienteDetalhe>();
            return obj;
        }
    }
}
