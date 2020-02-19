using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IInconsistenciaImportacaoAplicacao : IBaseAplicacao<InconsistenciaImportacao>
    {
        void Clear();
    }

    public class InconsistenciaImportacaoAplicacao : BaseAplicacao<InconsistenciaImportacao, IInconsistenciaImportacaoServico>, IInconsistenciaImportacaoAplicacao
    {
        private readonly IInconsistenciaImportacaoServico _inconsistenciaImportacaoServico;
    
        public InconsistenciaImportacaoAplicacao(IInconsistenciaImportacaoServico inconsistenciaImportacaoServico)
        {
            _inconsistenciaImportacaoServico = inconsistenciaImportacaoServico;
        }
    
        public void Clear()
        {
            _inconsistenciaImportacaoServico.Clear();
        }
    }
}