using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class MaterialHistorico : BaseEntity
    {
        public virtual string NumeroNota { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual bool EhUmAtivo { get; set; }
        public virtual AcaoEstoqueManual AcaoEstoqueManual { get; set; }

        public virtual Material Material { get; set; }
        public virtual Estoque Estoque { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual Usuario Usuario { get; set; }

        protected MaterialHistorico()
        {
        }

        public MaterialHistorico(EstoqueManual estoqueManual)
        {
            AcaoEstoqueManual = estoqueManual.Acao;
            DataInsercao = estoqueManual.DataInsercao;
            EhUmAtivo = estoqueManual.Material.EhUmAtivo;
            Estoque = estoqueManual.Estoque;
            Material = estoqueManual.Material;
            Quantidade = estoqueManual.Material.QuantidadeTotalEstoque;
            NumeroNota = estoqueManual.NumeroNFPedido;
            Unidade = estoqueManual.Unidade;
        }
    }
}