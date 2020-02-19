using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Entidade.Base;

namespace Entidade
{
    public class Usuario : BaseEntity
    {
        public Usuario()
        {
            ListaPerfilId = new List<int>();
        }
        
        [Required]
        public virtual Funcionario Funcionario { get; set; }
        [Required, StringLength(50)]
        public virtual string Login { get; set; }

        [Required, StringLength(50)]
        [IgnoreDataMember]
        public virtual string Senha { get; set; }
        [Required]
        public virtual IList<UsuarioPerfil> Perfils { get; set; }
        
        public virtual byte[] ImagemUpload { get; set; }

        public virtual string GetImage()
        {
            return ImagemUpload != null && ImagemUpload.Any() 
                        ? $"data:image/jpg;base64,{Convert.ToBase64String(ImagemUpload)}" 
                        : "../../Content/img/avatars/sunny-big.png";
        }

        public virtual string NomeId => Id + " - " + Funcionario?.Pessoa?.Nome;

        public virtual bool PrimeiroLogin { get; set; }

        public virtual bool TemAcessoAoPDV { get; set; }

        public virtual string OperadorPerfil { get; set; }

        public virtual Unidade Unidade { get; set; }

        public virtual bool Ativo { get; set; }

        public virtual List<int> ListaPerfilId { get; set; }

        public virtual string NomeCompleto { get; set; }

        public virtual bool EhFuncionario { get; set; }

        public virtual string Email { get; set; }
    }
}