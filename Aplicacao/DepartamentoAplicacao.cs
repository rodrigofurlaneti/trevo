using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IDepartamentoAplicacao : IBaseAplicacao<Departamento>
    {
    }

    public class DepartamentoAplicacao : BaseAplicacao<Departamento, IDepartamentoServico>, IDepartamentoAplicacao
    {
    }
}