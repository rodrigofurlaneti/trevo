using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entidade.Uteis;

namespace Portal.Models
{
    public class PesquisaDevedor
    {
        public string Cpf { get; set; }
        public int IdadeDe { get; set; }
        public int IdadeAte { get; set; }
        public Genero Genero { get; set; }
        public int Estado { get; set; }
        public int Cidade { get; set; }
        public int Bairro { get; set; }
        public bool CelCadastrado { get; set; }
        public bool TelCadastrado { get; set; }
        public bool SemCadContato { get; set; }
        public bool EmailCadastrado { get; set; }
        public bool SemCadEmail { get; set; }
        public ComSemCobranca ComSemCobranca { get; set; }
        public FiltroNasc FiltroNasc { get; set; }
        public FiltroEnd FiltroEnd { get; set; }
        public string TotalPesquisado { get; set; }
    }
}