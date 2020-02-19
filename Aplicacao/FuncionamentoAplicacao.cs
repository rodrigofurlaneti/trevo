using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IFuncionamentoAplicacao : IBaseAplicacao<Funcionamento>
    {

    }
    public class FuncionamentoAplicacao : BaseAplicacao<Funcionamento,IFuncionamentoServico>,IFuncionamentoAplicacao
    {

    }
}
