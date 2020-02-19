using Entidade.Uteis;
using System;
using System.IO;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ArquivoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public bool Thumbnail { get; set; }
        public bool Ativo { get; set; }
        public TipoArquivo Tipo { get; set; }

        public string GetImagemPorEndereco()
        {
            if (!File.Exists(Endereco))
                return string.Empty;

            var bytes = File.ReadAllBytes(Endereco);
            return bytes.Any()
                    ? $"data:image/jpg;base64,{Convert.ToBase64String(bytes)}"
                    : "../../Content/img/avatars/sunny-big.png";
        }
    }
}