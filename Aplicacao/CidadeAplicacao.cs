using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ICidadeAplicacao : IBaseAplicacao<Cidade>
    {
    }

    public class CidadeAplicacao : BaseAplicacao<Cidade, ICidadeServico>, ICidadeAplicacao
    {
    }
}