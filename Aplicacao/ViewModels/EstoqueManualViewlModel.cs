using AutoMapper;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class EstoqueManualViewModel
    {
        public int Id { get; set; }

        public DateTime DataInsercao { get; set; }

        public string NumeroNFPedido { get; set; }
        public int Quantidade { get; set; }
        public string Preco { get; set; }
        public string ValorTotal { get; set; }
        public string Motivo { get; set; }

        public AcaoEstoqueManual Acao { get; set; }

        public PedidoCompraViewModel PedidoCompra { get; set; }
        public EstoqueViewModel Estoque { get; set; }
        public MaterialViewModel Material { get; set; }
        public UnidadeViewModel Unidade { get; set; }

        public IList<EstoqueManualItemViewModel> ListEstoqueManualItem { get; set; }

        public EstoqueManualViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public EstoqueManualViewModel(EstoqueManual EstoqueManual)
        {
            if (EstoqueManual != null)
            {
                Id = EstoqueManual.Id;
                DataInsercao = EstoqueManual.DataInsercao;
                NumeroNFPedido = EstoqueManual.NumeroNFPedido;
                Quantidade = EstoqueManual.Quantidade;
                Preco = EstoqueManual.Preco.ToString("N2");
                ValorTotal = EstoqueManual.ValorTotal.ToString("N2");
                Motivo = EstoqueManual.Motivo;
                Acao = EstoqueManual.Acao;
                Estoque = new EstoqueViewModel(EstoqueManual.Estoque);
                Material = Mapper.Map<MaterialViewModel>(EstoqueManual.Material);
                ListEstoqueManualItem = EstoqueManual.ListEstoqueManualItem.Select(x => new EstoqueManualItemViewModel(x)).ToList() ?? new List<EstoqueManualItemViewModel>();
                Unidade = new UnidadeViewModel(EstoqueManual.Unidade);
                PedidoCompra = Mapper.Map<PedidoCompraViewModel>(EstoqueManual.PedidoCompra);
            }
        }

        public EstoqueManual ToEntity()
        {
            var entidade = new EstoqueManual
            {
                Id = Id,
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
                NumeroNFPedido = NumeroNFPedido,
                Quantidade = Quantidade,
                Preco = decimal.Parse(Preco),
                ValorTotal = decimal.Parse(ValorTotal),
                Motivo = Motivo,
                Acao = Acao,
                Estoque = Estoque.ToEntity(),
                Material = Mapper.Map<Material>(Material),
                ListEstoqueManualItem = ListEstoqueManualItem?.Select(x => x.ToEntity())?.ToList(),
                Unidade = Unidade?.ToEntity(),
                PedidoCompra = Mapper.Map<PedidoCompra>(PedidoCompra)
        };

            return entidade;
        }
    }
}
