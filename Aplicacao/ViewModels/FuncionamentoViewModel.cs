using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Aplicacao.ViewModels
{
    public class FuncionamentoViewModel
    {
        public int? Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Nome { get; set; }
        public int? CodFuncionamento { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public virtual IList<HorarioPrecoViewModel> HorariosPrecos { get; set; }
    }
}
