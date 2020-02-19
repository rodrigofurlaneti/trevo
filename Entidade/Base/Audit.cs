using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade.Base
{
    public class Audit : BaseEntity
    {
        public Audit()
        {
            Data = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        public Audit(string usuario, string entidade, string atributo, int codigoEntidade, string valorAntigo, string valorNovo, string usuarioNome)
        {
            Usuario = usuario;
            Entidade = entidade;
            Atributo = atributo;
            CodigoEntidade = codigoEntidade;
            ValorAntigo = valorAntigo;
            ValorNovo = valorNovo;
            Data = DateTime.Now;
            UsuarioNome = usuarioNome;
        }

        [Required]
        public virtual DateTime Data { get; set; }

        [Required, StringLength(50)]
        public virtual string Usuario { get; set; }

        [Required, StringLength(50)]
        public virtual string Entidade { get; set; }

        [Required, StringLength(50)]
        public virtual string Atributo { get; set; }

        [Required]
        public virtual int CodigoEntidade { get; set; }

        [Required, StringLength(200)]
        public virtual string ValorAntigo { get; set; }

        [Required, StringLength(200)]
        public virtual string ValorNovo { get; set; }
        
        [Required, StringLength(200)]
        public virtual string UsuarioNome { get; set; }
    }
}