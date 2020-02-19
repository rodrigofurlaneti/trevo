using System.Collections.Generic;
using System.Linq;
using Aplicacao;
using Core;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace InitializerHelper.Startup
{
    public static class PerfilStartup
    {
        #region Private Members
        private static void AdicionaPerfisDefault()
        {
            var perfilAplicacao = ServiceLocator.Current.GetInstance<IPerfilAplicacao>();
            foreach (var perfilPermissao in AppStartup.PerfilPermissaoDefault)
            {
                var perfilEncontrado = perfilAplicacao.PrimeiroPor(x => x.Nome.Equals(perfilPermissao.Key));
                if (perfilEncontrado != null)
                    continue;

                var permissaoAplicacao = ServiceLocator.Current.GetInstance<IPermissaoAplicacao>();
                var permissaoRoot = permissaoAplicacao.PrimeiroPor(x => x.Regra.Equals(perfilPermissao.Value));
                var perfil = new Perfil
                {
                    Nome = perfilPermissao.Key,
                    Permissoes = new List<Permissao> { permissaoRoot }
                };
                perfilAplicacao.Salvar(perfil);
            }
        }

        private static void AdicionaCachePerfisRegras()
        {
            var perfilAplicacao = ServiceLocator.Current.GetInstance<IPerfilAplicacao>();
            var perfis = perfilAplicacao.Buscar();
            foreach (var perfil in perfis)
                AdicionaPerfilAoCache(perfil);
        }

        public static void AdicionaPerfilAoCache(Perfil perfil)
        {
            CacheLayer.Clear(perfil.Id.ToString());
            CacheLayer.Add(perfil.Id.ToString(), perfil.Permissoes.Select(x => x.Regra));
        }
        #endregion

        #region Public Members

        public static void Start()
        {
            AdicionaPerfisDefault();
            AdicionaCachePerfisRegras();
        }

        #endregion
    }
}