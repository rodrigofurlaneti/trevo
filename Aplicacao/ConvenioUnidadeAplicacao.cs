using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IConvenioUnidadeAplicacao : IBaseAplicacao<ConvenioUnidade>
    {

    }
    public class ConvenioUnidadeAplicacao : BaseAplicacao<ConvenioUnidade, IConvenioUnidadeServico>, IConvenioUnidadeAplicacao
    {

    }
}
