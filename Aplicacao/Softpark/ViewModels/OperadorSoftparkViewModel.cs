using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class OperadorSoftparkViewModel : BaseSoftparkViewModel
    {
        public string Login { get; set; }
        public string OperadorPerfil { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public int? AlterSenha { get; set; }
        public bool AbrirCaixa { get; set; }
        public string Matricula { get; set; }
        public string CPF { get; set; }
        public EstacionamentoSoftparkViewModel Estacionamento { get; set; }

        public OperadorSoftparkViewModel()
        {
        }

        public OperadorSoftparkViewModel(Usuario usuario)
        {
            Id = usuario.Id;
            Matricula = usuario.Id.ToString(); //TODO: Informar a matricula
            OperadorPerfil = usuario.OperadorPerfil;
            Nome = usuario.Funcionario?.Pessoa?.Nome ?? usuario.NomeCompleto;
            Senha = usuario.Senha;
            Ativo = usuario.Ativo;
            Login = usuario.Login?.Replace(".","").Replace("-","");
            Estacionamento = new EstacionamentoSoftparkViewModel(usuario.Unidade);
            CPF = (usuario.Funcionario?.Pessoa?.DocumentoCpf ?? usuario.Login)?.Replace(".", "").Replace("-", "");
        }
    }
}
