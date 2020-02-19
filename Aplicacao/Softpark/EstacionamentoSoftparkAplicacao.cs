using Aplicacao.Softpark.Base;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Aplicacao
{
    public interface IEstacionamentoSoftparkAplicacao : IBaseSoftparkAplicacao<EstacionamentoSoftparkViewModel>
    {
        string SalvarTodos(List<EstacionamentoSoftparkViewModel> estacionamentos);
    }

    public class EstacionamentoSoftparkAplicacao : BaseSoftparkAplicacao<EstacionamentoSoftparkViewModel>, IEstacionamentoSoftparkAplicacao
    {
        public override string Tela => "Estacionamento";

        public string SalvarTodos(List<EstacionamentoSoftparkViewModel> estacionamentos)
        {
            return TentarNovamenteSeHouverException(() =>
            {
                var pedido = new RestRequest($"api/{Tela}/AllCustomers", Method.POST, DataFormat.Json);
                var json = JsonConvert.SerializeObject(estacionamentos, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                pedido.AddParameter("application/json", json, ParameterType.RequestBody);

                pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

                var retorno = _api.Execute<List<EstacionamentoSoftparkViewModel>>(pedido);

                if (retorno.StatusCode != HttpStatusCode.OK)
                    throw new SoftparkIntegrationException();

                return retorno.Content;
            });
        }
    }
}
