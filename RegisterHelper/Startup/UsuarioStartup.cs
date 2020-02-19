using System.Collections.Generic;
using Aplicacao;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace InitializerHelper.Startup
{
    public static class UsuarioStartup
    {
        #region Private Members
        #endregion

        #region Public Members

        public static void Start()
        {
            AdicionaPerfilRoot();
        }

        private static void AdicionaPerfilRoot()
        {
            var usuarioAplicacao = ServiceLocator.Current.GetInstance<IUsuarioAplicacao>();
            var usuarioRoot = usuarioAplicacao.PrimeiroPor(x => x.Funcionario.Pessoa.Nome.Equals("Administrador"));
            if (usuarioRoot != null)
                return;

            var perfilAplicacao = ServiceLocator.Current.GetInstance<IPerfilAplicacao>();
            var perfilRoot = perfilAplicacao.PrimeiroPor(x => x.Nome.Equals("Root"));

            var _funcionarioAplicacao = ServiceLocator.Current.GetInstance<IFuncionarioAplicacao>();
            var funcionarioRoot = _funcionarioAplicacao.PrimeiroPor(x => x.Pessoa.Nome.Equals("Administrador"));
            
            var usuario = new Usuario
            {
                Login = "523.203.170-82",
                Senha = "1234",
                Perfils = new List<UsuarioPerfil> { new UsuarioPerfil { Perfil = perfilRoot } },
                Funcionario = funcionarioRoot,
                ImagemUpload = null,
                Ativo = true
            };
            usuarioAplicacao.Salvar(usuario);
        }

        #endregion
    }
}