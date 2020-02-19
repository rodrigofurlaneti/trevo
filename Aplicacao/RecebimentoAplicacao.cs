using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IRecebimentoAplicacao : IBaseAplicacao<Recebimento>
    {
    }

    public class RecebimentoAplicacao : BaseAplicacao<Recebimento, IRecebimentoServico>, IRecebimentoAplicacao
    {
    }
}