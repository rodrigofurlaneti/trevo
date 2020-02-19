using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class OrcamentoSinistroViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public StatusOrcamentoSinistro Status { get; set; }

        public OISViewModel OIS { get; set; }
        public OrcamentoSinistroCotacaoViewModel OrcamentoSinistroCotacao { get; set; }
        public List<OrcamentoSinistroOficinaViewModel> OrcamentoSinistroOficinas { get; set; }
        public List<OrcamentoSinistroOficinaClienteViewModel> OrcamentoSinistroOficinaClientes { get; set; }
        public List<OrcamentoSinistroPecaServicoViewModel> OrcamentoSinistroPecaServicos { get; set; }
        public List<OrcamentoSinistroFornecedorViewModel> OrcamentoSinistroFornecedores { get; set; }
    }
}