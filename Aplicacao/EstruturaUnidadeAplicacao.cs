using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IEstruturaUnidadeAplicacao : IBaseAplicacao<EstruturaUnidade>
    {

    }
    public class EstruturaUnidadeAplicacao : BaseAplicacao<EstruturaUnidade,IEstruturaUnidadeServico>, IEstruturaUnidadeAplicacao
    {
    }
}
