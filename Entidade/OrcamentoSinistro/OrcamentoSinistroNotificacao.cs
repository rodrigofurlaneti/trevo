using Entidade.Base;

namespace Entidade
{
    public class OrcamentoSinistroNotificacao : BaseEnitidadeNotificacao
    {
        public virtual OrcamentoSinistro OrcamentoSinistro { get; set; }
    }
}