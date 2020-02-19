using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;

namespace Dominio
{
    public interface IIntegracaoServico : IBaseServico<Integracao>
    {
    }

    public class IntegracaoServico : BaseServico<Integracao, IIntegracaoRepositorio>, IIntegracaoServico
    {
    }
}