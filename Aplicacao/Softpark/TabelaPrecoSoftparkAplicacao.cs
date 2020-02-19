using Aplicacao.Softpark.Base;
using Aplicacao.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Aplicacao
{
    public interface ITabelaPrecoSoftparkAplicacao : IBaseSoftparkAplicacao<TabelaPrecoSoftparkViewModel>
    {
    }

    public class TabelaPrecoSoftparkAplicacao : BaseSoftparkAplicacao<TabelaPrecoSoftparkViewModel>, ITabelaPrecoSoftparkAplicacao
    {
        private readonly IEstacionamentoSoftparkAplicacao _estacionamentoSoftparkAplicacao;

        public override string Tela => "TabelaPreco";

        public TabelaPrecoSoftparkAplicacao(IEstacionamentoSoftparkAplicacao estacionamentoSoftparkAplicacao)
        {
            _estacionamentoSoftparkAplicacao = estacionamentoSoftparkAplicacao;
        }
    }
}
