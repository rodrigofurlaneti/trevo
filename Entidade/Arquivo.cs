using System;
using System.IO;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Arquivo : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Endereco { get; set; }
        public virtual TipoArquivo Tipo {get; set;}
        public virtual bool Ativo { get; set; }

        public virtual string GetImagemPorEndereco()
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