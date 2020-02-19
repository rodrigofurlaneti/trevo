using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Linq;
using System.Web;

namespace Dominio
{
    public interface IItemFuncionarioServico : IBaseServico<ItemFuncionario>
    {
        
    }

    public class ItemFuncionarioServico : BaseServico<ItemFuncionario, IItemFuncionarioRepositorio>, IItemFuncionarioServico
    {
        private readonly IItemFuncionarioRepositorio _itemFuncionarioRepositorio;
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IEstoqueManualServico _estoqueManualServico;

        public ItemFuncionarioServico(
            IItemFuncionarioRepositorio itemFuncionarioRepositorio,
            IFuncionarioRepositorio funcionarioRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            IEstoqueManualServico estoqueManualServico
            )
        {
            _itemFuncionarioRepositorio = itemFuncionarioRepositorio;
            _funcionarioRepositorio = funcionarioRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _estoqueManualServico = estoqueManualServico;
        }

        public override void Salvar(ItemFuncionario itemFuncionario)
        {
            var funcionario = _funcionarioRepositorio.GetById(itemFuncionario.Funcionario.Id);

            _itemFuncionarioRepositorio.AtualizarEstoque(itemFuncionario, funcionario.ItemFuncionario);
            CriarNotificacoesSeEstoqueBaixo(itemFuncionario);

            funcionario.ItemFuncionario = itemFuncionario;

            if (itemFuncionario.ItemFuncionariosDetalhes.Any())
            {
                _funcionarioRepositorio.Save(funcionario);
            }
            else if (itemFuncionario.Id > 0)
            {
                Repositorio.Clear();
                Repositorio.DeleteById(itemFuncionario.Id);
            }
        }

        private void CriarNotificacoesSeEstoqueBaixo(ItemFuncionario itemFuncionario)
        {
            var usuarioId = (int)(HttpContext.Current.User as dynamic).UsuarioId;
            var usuario = _usuarioRepositorio.GetById(usuarioId);

            var listaMaterial = itemFuncionario?.ItemFuncionariosDetalhes?.Select(x => x.Material)?.DistinctBy(x => x.Id)?.ToList() ?? new System.Collections.Generic.List<Material>();
            foreach (var material in listaMaterial)
            {
                _estoqueManualServico.CriarNotificacaoSeEstoqueBaixo(material, usuario);
            }
        }
    }
}