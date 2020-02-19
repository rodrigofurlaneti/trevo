using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class PropostaGridViewModel
    {
        public int Id { get; set; }
        public string Empresa { get; set; }
        public string Filial { get; set; }
        public string EmailEnviado { get; set; }
        public string Pedido { get; set; }
        public string StatusPedido { get; set; }
        public bool TemPedido { get; set; }
    }
}