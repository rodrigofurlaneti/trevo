using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IItemFuncionarioAplicacao : IBaseAplicacao<ItemFuncionario>
    {
    }

    public class ItemFuncionarioAplicacao : BaseAplicacao<ItemFuncionario, IItemFuncionarioServico>, IItemFuncionarioAplicacao
    {
    }
}