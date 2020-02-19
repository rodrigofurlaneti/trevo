using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class BuscarGridControleFeriasViewModel
    {
        public string NomeFuncionario { get; set; }
        public string NomeUnidade { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public string Trabalhada { get; set; }
        public DateTime? TrabalhoDe { get; set; }
        public DateTime? TrabalhoAte { get; set; }
    }
}