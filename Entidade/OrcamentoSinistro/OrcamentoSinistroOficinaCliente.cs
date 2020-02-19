namespace Entidade
{
    public class OrcamentoSinistroOficinaCliente
    {
        public virtual OrcamentoSinistro OrcamentoSinistro { get; set; }
        public virtual Oficina Oficina { get; set; }
    }
}