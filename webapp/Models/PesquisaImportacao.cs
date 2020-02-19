using System;
using Aplicacao.ViewModels;
using Entidade.Uteis;

namespace Portal.Models
{
    public class PesquisaImportacao
    {
        public DateTime DataDe { get; set; }
        public DateTime DataAte { get; set; }
        public int CodigoCarteira { get; set; }
        public int StatusArquivo { get; set; }

    }
}