using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IControleFeriasAplicacao : IBaseAplicacao<ControleFerias>
    {
    }

    public class ControleFeriasAplicacao : BaseAplicacao<ControleFerias, IControleFeriasServico>, IControleFeriasAplicacao
    {
    }
}