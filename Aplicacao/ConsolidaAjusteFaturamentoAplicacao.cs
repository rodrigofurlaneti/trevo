using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System;

namespace Aplicacao
{
    public interface IConsolidaAjusteFaturamentoAplicacao : IBaseAplicacao<ConsolidaAjusteFaturamento>
    {
       
    }

    public class ConsolidaAjusteFaturamentoAplicacao : BaseAplicacao<ConsolidaAjusteFaturamento, IConsolidaAjusteFaturamentoServico>, IConsolidaAjusteFaturamentoAplicacao
    {
       
    }
}