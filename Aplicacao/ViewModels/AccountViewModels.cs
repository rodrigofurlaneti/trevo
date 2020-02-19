using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class AccountLoginModel
    {
        public AccountLoginModel()
        {

        }

        public AccountLoginModel(Usuario usuario)
        {
            CPF = usuario?.Funcionario?.Pessoa.Documentos?.FirstOrDefault(x => x.Documento.Tipo == Entidade.Uteis.TipoDocumento.Cpf)?.Documento?.Numero;
            Email = usuario?.Login;
            Password = usuario?.Senha;
        }

        [Required]
        public string CPF { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Imagem { get; set; }
        public string Nome { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
        public string Sexo { get; set; }
    }

    public class AccountForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class AccountResetPasswordModel
    {
        public string Email { get; set; }
        public string Nome { get; set; }
        
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        public string CPF { get; set; }

        public byte[] ImagemUpload { get; set; }
        public string GetImage
        {
            get
            {
                return ImagemUpload != null && ImagemUpload.Any()
                      ? $"data:image/jpg;base64,{Convert.ToBase64String(ImagemUpload)}"
                      : "../../Content/img/avatars/sunny-big.png";
            }
        }
    }

    public class AccountRegistrationModel
    {
        public string Username { get; set; }

        [Required]
        public string CPF { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [EmailAddress]
        [Compare("Email")]
        public string EmailConfirm { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public int UsuarioId { get; set; }
        public int PessoaId { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public IList<int> PerfilsId { get; set; }
        public byte[] Imagem { get; set; }
        public bool RememberMe { get; set; }

        public virtual string GetImage()
        {
            return Imagem != null && Imagem.Any()
                      ? $"data:image/jpg;base64,{Convert.ToBase64String(Imagem)}"
                      : "../../Content/img/avatars/sunny-big.png";
        }
    }

    public class CustomPrincipal : IPrincipal
    {
        public CustomPrincipal(string login)
        {
            Identity = new GenericIdentity(login);
        }

        public IIdentity Identity { get; }
        public int UsuarioId { get; set; }
        public int PessoaId { get; set; }
        public string Nome { get; set; }
        public IList<int> PerfilsId { get; set; }
        public byte[] Imagem { get; set; }
        public bool RememberMe { get; set; }

        public virtual string GetImage()
        {
            return Imagem != null && Imagem.Any()
                      ? $"data:image/jpg;base64,{Convert.ToBase64String(Imagem)}"
                      : "../../Content/img/avatars/sunny-big.png";
        }
        public bool IsInRole(string role) { return false; }
    }
}