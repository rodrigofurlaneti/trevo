using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IModeloAplicacao : IBaseAplicacao<Modelo>
    {
        
    }

    public class ModeloAplicacao : BaseAplicacao<Modelo, IModeloServico>, IModeloAplicacao
    {
        
    }
}