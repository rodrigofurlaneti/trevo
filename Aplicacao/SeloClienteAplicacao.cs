
using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{

    public interface ISeloClienteAplicacao : IBaseAplicacao<SeloCliente>
    {
    }

    public class SeloClienteAplicacao : BaseAplicacao<SeloCliente, ISeloClienteServico>, ISeloClienteAplicacao
    {
        public readonly ISeloClienteServico _seloClienteServico;

        public SeloClienteAplicacao(ISeloClienteServico seloClienteServico)
        {
            _seloClienteServico = seloClienteServico;
        }
    }
}
