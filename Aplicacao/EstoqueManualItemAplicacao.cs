using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IEstoqueManualItemAplicacao : IBaseAplicacao<EstoqueManualItem>
    {

    }
    public class EstoqueManualItemAplicacao : BaseAplicacao<EstoqueManualItem, IEstoqueManualItemServico>, IEstoqueManualItemAplicacao
    {

    }
}
