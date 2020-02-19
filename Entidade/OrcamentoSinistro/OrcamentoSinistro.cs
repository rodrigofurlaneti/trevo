using System.Collections.Generic;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class OrcamentoSinistro : BaseEntity
    {
        public virtual OIS OIS { get; set; }
        public virtual StatusOrcamentoSinistro Status { get; set; }
        public virtual OrcamentoSinistroCotacao OrcamentoSinistroCotacao { get; set; }
        public virtual IList<OrcamentoSinistroOficina> OrcamentoSinistroOficinas { get; set; }
        public virtual IList<OrcamentoSinistroOficinaCliente> OrcamentoSinistroOficinaClientes { get; set; }
        public virtual IList<OrcamentoSinistroPecaServico> OrcamentoSinistroPecaServicos { get; set; }
        public virtual IList<OrcamentoSinistroFornecedor> OrcamentoSinistroFornecedores { get; set; }
        public virtual IList<OrcamentoSinistroNotificacao> OrcamentoSinistroNotificacoes { get; set; }

        public virtual void Aprovar()
        {
            Status = StatusOrcamentoSinistro.Aprovado;
        }

        public virtual void Negar()
        {
            Status = StatusOrcamentoSinistro.Negado;
        }
    }
}