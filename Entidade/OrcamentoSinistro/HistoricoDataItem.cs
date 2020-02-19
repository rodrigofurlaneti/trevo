using Entidade.Base;
using Entidade.Uteis;
using System;

namespace Entidade
{
    public class OrcamentoSinistroCotacaoHistoricoDataItem
    {
        public virtual OrcamentoSinistroCotacaoItem OrcamentoSinistroCotacaoItem { get; set; }
        public virtual DateTime Data { get; set; }
    }
}