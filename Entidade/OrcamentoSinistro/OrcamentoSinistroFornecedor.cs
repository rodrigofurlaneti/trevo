namespace Entidade
{
    public class OrcamentoSinistroFornecedor
    {
        public virtual OrcamentoSinistro OrcamentoSinistro { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
    }
}