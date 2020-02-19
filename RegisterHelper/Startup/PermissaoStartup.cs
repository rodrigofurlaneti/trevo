using System.Collections.Generic;
using Aplicacao;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace InitializerHelper.Startup
{
    public static class PermissaoStartup
    {
        #region Private Members
        
        #endregion

        #region Public Members
        public static void Start()
        {
            AdicionaPermissoesDefault();
        }

        private static void AdicionaPermissoesDefault()
        {
            var permissaoAplicacao = ServiceLocator.Current.GetInstance<IPermissaoAplicacao>();
            foreach (var permissaoDefault in AppStartup.PerfilPermissaoDefault)
            {
                var permissaoEncontrada = permissaoAplicacao.PrimeiroPor(x => x.Regra.Equals(permissaoDefault.Value));
                if (permissaoEncontrada != null)
                    continue;
                //
                permissaoAplicacao.Salvar(new Permissao { Nome = permissaoDefault.Key, Regra = permissaoDefault.Value });
            }
        }

        #endregion
    }
}