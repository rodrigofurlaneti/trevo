using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class PrecoViewModel
    {
        public int? Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Nome { get; set; }
        public int TempoTolerancia { get; set; }
        public string NomeUsuario { get; set; }
        public StatusPreco PrecoStatus { get; set; }
        public IList<FuncionamentoViewModel> Funcionamentos { get; set; }

        //public IList<PrecoNotificacao> Notificacoes { get; set; }
    }

}
