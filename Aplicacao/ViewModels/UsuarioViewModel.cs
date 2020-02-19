using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class UsuarioViewModel
    {
        public int? Id { get; set; }
        public FuncionarioViewModel Funcionario { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public IList<UsuarioPerfilViewModel> Perfils { get; set; }
        public string GetImage() => AvatarUpload != null && AvatarUpload.Any() ? $"data:image/jpg;base64,{Convert.ToBase64String(AvatarUpload)}" : "../../Content/img/avatars/sunny-big.png";
        public string NomeId => Id + " - " + Funcionario?.Pessoa?.Nome;
        public bool PrimeiroLogin { get; set; }
        public bool TemAcessoAoPDV { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public OperadorPerfilSoftpark? OperadorPerfil { get; set; }
        public bool Ativo { get; set; }
        public byte[] AvatarUpload { get; set; }
        public List<int> ListaPerfilId { get; set; }
        public string NomeCompleto { get; set; }
        public bool EhFuncionario { get; set; }

        public UsuarioViewModel() { }

        public UsuarioViewModel(Usuario entidade)
        {
            ListaPerfilId = new List<int>();

            Id = entidade.Id;
            Funcionario = entidade?.Funcionario != null ? new FuncionarioViewModel(entidade?.Funcionario) : new FuncionarioViewModel();
            Login = entidade.Login;
            Senha = entidade.Senha;
            Email = entidade.EhFuncionario ? entidade.Funcionario?.Pessoa?.ContatoEmail : entidade.Email;

            if (entidade.Perfils != null && entidade.Perfils.Any())
                ListaPerfilId.AddRange(entidade.Perfils.Select(x => x.Perfil.Id).ToList());

            PrimeiroLogin = entidade.PrimeiroLogin;
            TemAcessoAoPDV = entidade.TemAcessoAoPDV;
            Ativo = entidade.Ativo;
            AvatarUpload = entidade.ImagemUpload;
            OperadorPerfil = string.IsNullOrEmpty(entidade.OperadorPerfil) ? null : (OperadorPerfilSoftpark?)Enum.Parse(typeof(OperadorPerfilSoftpark), entidade.OperadorPerfil);
            Unidade = new UnidadeViewModel(entidade.Unidade);
            EhFuncionario = entidade.EhFuncionario;
            NomeCompleto = entidade.NomeCompleto;
        }

        public virtual Usuario ToEntity() => new Usuario
        {
            Id = this.Id.GetValueOrDefault(),
            DataInsercao = DateTime.Now,
            Funcionario = this?.Funcionario?.ToEntity(),
            Login = this.Login,
            Senha = this.Senha,
            Perfils = AutoMapper.Mapper.Map<List<UsuarioPerfilViewModel>, List<UsuarioPerfil>>(this.Perfils.ToList()),
            Ativo = this.Ativo,
            TemAcessoAoPDV = this.TemAcessoAoPDV,
            OperadorPerfil = this.OperadorPerfil?.ToString() ?? string.Empty,
            Unidade = this.Unidade.ToEntity(),
            EhFuncionario = this.EhFuncionario,
            NomeCompleto = this.NomeCompleto
        };
    }
}