using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ITipoUnidadeAplicacao: IBaseAplicacao<TipoUnidade>
    {

    }

    public class TipoUnidadeAplicacao : BaseAplicacao<TipoUnidade, ITipoUnidadeServico>, ITipoUnidadeAplicacao
    {
    }
}
