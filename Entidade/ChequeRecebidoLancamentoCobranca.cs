namespace Entidade
{
    public class ChequeRecebidoLancamentoCobranca
    {
        public virtual LancamentoCobranca LancamentoCobranca { get; set; }
        public virtual ChequeRecebido ChequeRecebido { get; set; }
    }
}