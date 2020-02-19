using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface ICalendarioRHAplicacao : IBaseAplicacao<CalendarioRH>
    {
    }

    public class CalendarioRHAplicacao : BaseAplicacao<CalendarioRH, ICalendarioRHServico>, ICalendarioRHAplicacao
    {
    }
}