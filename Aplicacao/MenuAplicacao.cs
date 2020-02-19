using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;

namespace Aplicacao
{
    public interface IMenuAplicacao : IBaseAplicacao<Menu>
    {
        bool VerificaMenuJaAssociado(int idMenu);
    }

    public class MenuAplicacao : BaseAplicacao<Menu, IMenuServico>, IMenuAplicacao
    {
        private readonly IMenuServico _menuServico;

        public MenuAplicacao(IMenuServico menuServico)
        {
            _menuServico = menuServico;
        }

        public new void Salvar(Menu entity)
        {
            if (string.IsNullOrEmpty(entity.Descricao))
                throw new BusinessRuleException("Informe o Nome da Página!");
            
            if (entity.Posicao <= 0)
                throw new BusinessRuleException("Informe um número de posição para a página [Ex.: 1, 2, 3...]!");

            if (entity.MenuPai != null && entity.MenuPai.Id != 0)
            {
                if (string.IsNullOrEmpty(entity.Url))
                {
                    throw new BusinessRuleException("Informe a URL da Página!");
                }
            }

            if (!string.IsNullOrEmpty(entity.Url) && _menuServico.ExisteMenuDuplicado(entity))
                throw new BusinessRuleException("Já existe o Menu cadastrado!");

            var menuRetorno = BuscarPorId(entity.Id) ?? entity;

            menuRetorno.Id = entity.Id;
            menuRetorno.Descricao = entity.Descricao;
            menuRetorno.Url = entity.Url;
            menuRetorno.Posicao = entity.Posicao;
            menuRetorno.Ativo = entity.Ativo;
            menuRetorno.MenuPai = entity.MenuPai?.Id == null ? null : BuscarPorId(entity.MenuPai.Id);

            Servico.Salvar(menuRetorno);
        }

        public bool VerificaMenuJaAssociado(int idMenu)
        {
            return _menuServico.VerificaMenuJaAssociado(idMenu);
        }

        public new void Excluir(Menu entity)
        {
            if (entity.Id > 0 && VerificaMenuJaAssociado(entity.Id))
                throw new BusinessRuleException("Menu já está sendo utilizado por algum perfil!");

            Servico.Excluir(entity);
        }
        public new void ExcluirPorId(int id)
        {
            if (id > 0 && VerificaMenuJaAssociado(id))
                throw new BusinessRuleException("Menu já está sendo utilizado por algum perfil!");

            Servico.ExcluirPorId(id);
        }
    }
}