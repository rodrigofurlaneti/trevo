using Entidade.Uteis;
using System;
using System.IO;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class DownloadArquivoViewModel
    {
        public bool IsString { get; set; }
        public object DataSource { get; set; }
        public ContentType ContentType { get; set; }
        public string Nome { get; set; }
    }
}