using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class PedidoLocacaoLancamentoAdicional : BaseEntity
    {
        public virtual PedidoLocacao PedidoLocacao { get; set; }
        public virtual string Descricao { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual bool Ativo { get; set; }

        public virtual void AssociarPedidoLocacao(PedidoLocacao pedidoLocacao)
        {
            PedidoLocacao = pedidoLocacao;
        }
    }
}