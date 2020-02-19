using Entidade;

namespace Aplicacao.ViewModels
{
    public class LancamentoCobrancaContratoMensalistaViewModel
    {
        public int Id { get; set; }
        public LancamentoCobrancaViewModel LancamentoCobranca { get; set; }
        public ContratoMensalistaViewModel ContratoMensalista { get; set; }
        
        public LancamentoCobrancaContratoMensalistaViewModel()
        {
        }

        public LancamentoCobrancaContratoMensalistaViewModel(LancamentoCobrancaContratoMensalista objeto)
        {
            Id = objeto.Id;
            LancamentoCobranca = objeto?.LancamentoCobranca != null ? new LancamentoCobrancaViewModel(objeto.LancamentoCobranca) : new LancamentoCobrancaViewModel();
            ContratoMensalista = objeto?.ContratoMensalista != null ? new ContratoMensalistaViewModel(objeto.ContratoMensalista) : new ContratoMensalistaViewModel();
        }
    }
}