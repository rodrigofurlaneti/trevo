using Entidade;
using Entidade.Uteis;
using System.Collections;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class EmissaoNotaFiscalViewModel
    {
        public int Id { get; set; }
        public Mes Mes { get; set; }
        public int Ano { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public EmpresaViewModel Empresa { get; set; }
        public ICollection<Pagamento> Pagamentos { get; set; }
    }
}