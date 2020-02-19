using System.Linq;
using Aplicacao;
using Entidade;
using Microsoft.Practices.ServiceLocation;
using System;

namespace InitializerHelper.Startup
{
    public static class MenuStartup
    {
        #region Private Members
        private static void AdicionaMenusDefault()
        {
            var menuAplicacao = ServiceLocator.Current.GetInstance<IMenuAplicacao>();
            var menuConfiguracoes = menuAplicacao.BuscarPor(x => x.Descricao.ToLower() == "configurações").FirstOrDefault();
            if (menuConfiguracoes == null)
            {
                menuAplicacao.Salvar(new Menu()
                {
                    DataInsercao = DateTime.Now,
                    Ativo = true,
                    Descricao = "Configurações",
                    Posicao = 1
                });

                menuConfiguracoes = menuAplicacao.BuscarPor(x => x.Descricao.ToLower() == "configurações").FirstOrDefault();
            }

            var menuUsuario = menuAplicacao.BuscarPor(x => x.Descricao.ToLower() == "cadastro de usuário").FirstOrDefault();
            if (menuUsuario == null)
            {
                menuAplicacao.Salvar(new Menu
                {
                    DataInsercao = DateTime.Now,
                    Ativo = true,
                    Descricao = "Cadastro de Usuário",
                    Posicao = 1,
                    MenuPai = menuConfiguracoes,
                    Url = "Usuario/Index"
                });
            }

            var menuMenu = menuAplicacao.BuscarPor(x => x.Descricao.ToLower() == "cadastro de menu").FirstOrDefault();
            if (menuMenu == null)
            {
                menuAplicacao.Salvar(new Menu
                {
                    DataInsercao = DateTime.Now,
                    Ativo = true,
                    Descricao = "Cadastro de Menu",
                    Posicao = 2,
                    MenuPai = menuConfiguracoes,
                    Url = "Menu/Index"
                });
            }

            var menuPerfil = menuAplicacao.BuscarPor(x => x.Descricao.ToLower() == "cadastro de perfil").FirstOrDefault();
            if (menuPerfil == null)
            {
                menuAplicacao.Salvar(new Menu
                {
                    DataInsercao = DateTime.Now,
                    Ativo = true,
                    Descricao = "Cadastro de Perfil",
                    Posicao = 3,
                    MenuPai = menuConfiguracoes,
                    Url = "Perfil/Index"
                });
            }

            var perfilAplicacao = ServiceLocator.Current.GetInstance<IPerfilAplicacao>();
            var perfilRoot = perfilAplicacao.BuscarPor(x => x.Nome.ToLower() == "root").FirstOrDefault();
            if (perfilRoot == null)
                return;

            var listaMenus = perfilRoot.ListaMenu;
            var menusAtualizados = false;
            menuAplicacao.BuscarPor(x => x.Id == menuConfiguracoes.Id || x.MenuPai.Id == menuConfiguracoes.Id).ToList().ForEach(x =>
            {
                if (!perfilRoot.ListaMenu.Any(m => m == x.Id))
                {
                    listaMenus.Add(x.Id);
                    menusAtualizados = true;
                }

            });

            perfilRoot.ListaMenu = listaMenus;

            if (menusAtualizados)
                perfilAplicacao.Salvar(perfilRoot);

        }

        #endregion

        #region Public Members

        public static void Start()
        {
            AdicionaMenusDefault();
        }

        #endregion
    }
}