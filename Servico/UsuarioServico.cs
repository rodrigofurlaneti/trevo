using System;
using System.Configuration;
using System.Linq;
using System.Web;
using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IUsuarioServico : IBaseServico<Usuario>
    {
        Usuario ValidarLogin(string cpf, string senha);
        void RecuperarSenha(string cpf, string template, string url);
        void TrocarSenha(string cpf, string senha, string senhaConfirmacao);
        void PrimeiroLoginRealizado(string cpf);
        Usuario RetornarPorCPF(string cpf);
    }

    public class UsuarioServico : BaseServico<Usuario, IUsuarioRepositorio>, IUsuarioServico
    {
        private readonly IPessoaServico _pessoaServico;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServico(IPessoaServico pessoaServico, IUsuarioRepositorio usuarioRepositorio)
        {
            _pessoaServico = pessoaServico;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Usuario ValidarLogin(string cpf, string senha)
        {
            return Repositorio.ToCast<IUsuarioRepositorio>().ValidarLogin(cpf, senha);
        }

        public void RecuperarSenha(string cpf, string template, string url)
        {
            var usuarioSolicitado = Repositorio.ListBy(x => x.Login == cpf).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(usuarioSolicitado?.Login))
                throw new NotFoundException("Nenhum usuário foi encontrado para o email informado.");

            if (string.IsNullOrEmpty(usuarioSolicitado.Funcionario.Pessoa.ContatoEmail))
            {
                throw new BusinessRuleException("O usuário informado não possui um e-mail vinculado, favor contatar o administrador");
            }

            //para quem vai o email
            var from = ConfigurationManager.AppSettings["EMAIL_FROM"];
            //muda dinâmicamente as variáveis no template do email
            template = template.Replace("[LINK]", GerarLinkRecuperarSenha(usuarioSolicitado.Login.Replace(".", "").Replace("-", "").Replace("/", ""), url));
            //chama a classe que envia o email
            var email = usuarioSolicitado?.Funcionario?.Pessoa?.Contatos?.FirstOrDefault(x => x.Contato.Tipo == Entidade.Uteis.TipoContato.Email)?.Contato?.Email;
            if (!string.IsNullOrEmpty(email))
                Mail.SendMail(email, "Recuperar senha", template, from);
        }

        public void TrocarSenha(string cpf, string senha, string senhaConfirmacao)
        {
            if (!senha.Equals(senhaConfirmacao))
                throw new Exception("A senha e a confirmação devem ser iguais.");

            var usuarioSolicitado = Repositorio.ListBy(x => x.Login == cpf).FirstOrDefault();
            if (usuarioSolicitado == null)
                throw new Exception("Nenhum usuário foi encontrado para o e-mail informado.");

            usuarioSolicitado.Senha = senha;
            Salvar(usuarioSolicitado);
        }

        private string GerarLinkRecuperarSenha(string cpf, string url)
        {
            var emailEncrypt = HttpUtility.UrlEncode(Crypt.EnCrypt(cpf.ToBase64(), ConfigurationManager.AppSettings["CryptKey"]));
            var link = $"{url}/account/RecuperarMinhaSenha?key={emailEncrypt}";
            return link;
        }

        public void PrimeiroLoginRealizado(string cpf)
        {
            var usuario = _usuarioRepositorio.RetornarPorCPF(cpf);// RetornarUsuarioPorCPFESenha(cpf);
            if (usuario == null) throw new Exception("Email não encontrado!");

            usuario.PrimeiroLogin = false;
            Repositorio.Save(usuario);
        }

        private Usuario RetornarUsuarioPorCPFESenha(string cpf = "", string senha = "")
        {
            var pessoa = this._pessoaServico.BuscarPorCpfComOuSemMascara(cpf);
            return pessoa == null
                ? null
                : !string.IsNullOrEmpty(cpf) && !string.IsNullOrEmpty(senha)
                ? Repositorio.FirstBy(x => x.Funcionario.Id == pessoa.Id && x.Senha == senha)
                : !string.IsNullOrEmpty(cpf) ? Repositorio.FirstBy(x => x.Funcionario.Id == pessoa.Id) : null;
        }

        public Usuario RetornarPorCPF(string cpf)
        {
            return _usuarioRepositorio.RetornarPorCPF(cpf);
        }
    }
}