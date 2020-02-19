using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Core.Exceptions;
using Core.Extensions;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPerfilAplicacao : IBaseAplicacao<Perfil>
    {
        void SalvarEAtualizarCache(Perfil perfil, int[] permissoes);
        IList<Perfil> BuscarPerfis();
        void ExcluirEAtualizarCache(Perfil perfil);
        List<Menu> BuscaMenus();
        bool VerificaPerfilJaAssociado(int idPerfil);
        bool VerificarSeTemAcessoAoMenu(IList<int> perfilsId, string requestPath);
    }

    public class PerfilAplicacao : BaseAplicacao<Perfil, IPerfilServico>, IPerfilAplicacao
    {
        private readonly IMenuAplicacao _menuAplicacao;
        private readonly IPerfilServico _perfilServico;
        private readonly IPermissaoAplicacao _permissaoAplicacao;

        public PerfilAplicacao(IPerfilServico perfilServico, IMenuAplicacao menuAplicacao, IPermissaoAplicacao permissaoAplicacao)
        {
            _perfilServico = perfilServico;
            _menuAplicacao = menuAplicacao;
            _permissaoAplicacao = permissaoAplicacao;
        }

        public IList<Perfil> BuscarPerfis()
        {
            return BuscarPor(x => !x.Permissoes.Any(p => p.Regra.Equals("root")));
        }

        public void SalvarEAtualizarCache(Perfil perfil, int[] permissoes)
        {
            Servico.ToCast<IPerfilServico>().SalvarEAtualizarCache(perfil, permissoes);
        }
        public void ExcluirEAtualizarCache(Perfil perfil)
        {
            Servico.ToCast<IPerfilServico>().ExcluirEAtualizarCache(perfil);
        }
        public void ExcluirEAtualizarCache(int idPerfil)
        {
            Servico.ToCast<IPerfilServico>().ExcluirEAtualizarCache(idPerfil);
        }

        public List<Menu> BuscaMenus()
        {
            return _menuAplicacao.Buscar().ToList();
        }

        public new void Salvar(Perfil entity)
        {
            var entidadePerfil = BuscarPorId(entity.Id) ?? entity;
            if (entidadePerfil.Menus == null)
                entidadePerfil.Menus = new List<PerfilMenu>();

            entidadePerfil.Nome = entity.Nome;

            var menus = _menuAplicacao.Buscar().ToList();
            entidadePerfil.Menus.Clear();
            foreach (var idMenu in entity.ListaMenu)
            {
                entidadePerfil.Menus.Add(new PerfilMenu { Menu = menus.FirstOrDefault(x => x.Id == idMenu) });
            }

            var permissoes = _permissaoAplicacao.Buscar();

            //Por não ter cadastro será vinculado a este registro padrão.
            entidadePerfil.Permissoes = new List<Permissao> { permissoes.FirstOrDefault(x=>x.Regra.Contains("root")) };

            SalvarEAtualizarCache(entidadePerfil, entidadePerfil.Permissoes.Select(x=>x.Id).ToArray());
        }

        public bool VerificaPerfilJaAssociado(int idPerfil)
        {
            return _perfilServico.VerificaPerfilJaAssociado(idPerfil);
        }

        public new void Excluir(Perfil entity)
        {
            ExcluirEAtualizarCache(entity);
        }
        public new void ExcluirPorId(int id)
        {
            ExcluirEAtualizarCache(id);
        }

        public bool VerificarSeTemAcessoAoMenu(IList<int> perfilsId, string requestPath)
        {
            var perfis = _perfilServico.BuscarPor(x => perfilsId.Contains(x.Id)).ToList();
            var menus = perfis?.SelectMany(x => x.Menus)
                               .Where(x => !string.IsNullOrEmpty(x.Menu.Url))
                               .Select(x => x.Menu.Url.Split('/').FirstOrDefault() == "" ? x.Menu.Url.Split('/')[1] : x.Menu.Url.Split('/')[0]).ToList();

            var urlArray = requestPath.Split('/');
            var menuAtual = urlArray.FirstOrDefault() == string.Empty ? urlArray[1] : urlArray[0];
            return menuAtual.ToLower() == "home" || menus.Any(x => x.ToLower() == menuAtual.ToLower());
        }
    }
}