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
    public interface ITabelaPrecoMensalSoftparkAplicacao : IBaseSoftparkAplicacao<TabelaPrecoMensalSoftparkViewModel>
    {
    }

    public class TabelaPrecoMensalSoftparkAplicacao : BaseSoftparkAplicacao<TabelaPrecoMensalSoftparkViewModel>, ITabelaPrecoMensalSoftparkAplicacao
    {
        private readonly IEstacionamentoSoftparkAplicacao _estacionamentoSoftparkAplicacao;

        public override string Tela => "TabelaPrecoMensal";

        public TabelaPrecoMensalSoftparkAplicacao(IEstacionamentoSoftparkAplicacao estacionamentoSoftparkAplicacao)
        {
            _estacionamentoSoftparkAplicacao = estacionamentoSoftparkAplicacao;
        }
    }
}
