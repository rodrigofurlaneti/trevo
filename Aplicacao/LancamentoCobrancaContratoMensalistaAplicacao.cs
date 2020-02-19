using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ILancamentoCobrancaContratoMensalistaAplicacao : IBaseAplicacao<LancamentoCobrancaContratoMensalista>
    {
        LancamentoCobrancaContratoMensalista RetornaUltimoLancamentoCobrancaPor(int id);
    }

    public class LancamentoCobrancaContratoMensalistaAplicacao : BaseAplicacao<LancamentoCobrancaContratoMensalista, ILancamentoCobrancaContratoMensalistaServico>, ILancamentoCobrancaContratoMensalistaAplicacao
    {
        private readonly ILancamentoCobrancaContratoMensalistaServico _lancamentoCobrancaContratoMensalistaServico;
        
        public LancamentoCobrancaContratoMensalistaAplicacao(ILancamentoCobrancaContratoMensalistaServico lancamentoCobrancaContratoMensalistaServico)
        {
            _lancamentoCobrancaContratoMensalistaServico = lancamentoCobrancaContratoMensalistaServico;
        }
        
        public LancamentoCobrancaContratoMensalista RetornaUltimoLancamentoCobrancaPor(int id)
        {
            return _lancamentoCobrancaContratoMensalistaServico.RetornaUltimoLancamentoCobrancaPor(id);
        }
    }
}