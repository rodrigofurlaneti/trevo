using Aplicacao.Base;
using Aplicacao.ViewModels;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{

    public interface IChequeEmitidoContaPagarAplicacao : IBaseAplicacao<ChequeEmitidoContaPagar>
    {
    }

    public class ChequeEmitidoContaPagarAplicacao : BaseAplicacao<ChequeEmitidoContaPagar, IChequeEmitidoContaPagarServico>, IChequeEmitidoContaPagarAplicacao
    {

    }
}
