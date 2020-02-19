using Entidade.Base;

namespace Entidade
{
    public class OrcamentoSinistroCotacaoNotificacao : BaseEnitidadeNotificacao
    {
        public virtual OrcamentoSinistroCotacao OrcamentoSinistroCotacao { get; set; }
    }
}