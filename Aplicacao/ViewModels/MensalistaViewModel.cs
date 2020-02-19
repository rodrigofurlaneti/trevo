using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class MensalistaViewModel
    {
        public int? Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }
}
