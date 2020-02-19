using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPeriodoAplicacao : IBaseAplicacao<Periodo>
    {

    }
        
    public class PeriodoAplicacao : BaseAplicacao<Periodo,IPeriodoServico>, IPeriodoAplicacao
    {
    }
}
