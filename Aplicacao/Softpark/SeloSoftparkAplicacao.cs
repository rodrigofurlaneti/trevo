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
    public interface ISeloSoftparkAplicacao : IBaseSoftparkAplicacao<EmissaoSeloSoftparkViewModel>
    {
        string AtualizarSelos(List<SeloSoftparkViewModel> selos);
        string CancelarLoteSelo(int idEmissao, string numeroLote);
    }

    public class SeloSoftparkAplicacao : BaseSoftparkAplicacao<EmissaoSeloSoftparkViewModel>, ISeloSoftparkAplicacao
    {
        public override string Tela => "Selo";

        public string AtualizarSelos(List<SeloSoftparkViewModel> selos)
        {
            return TentarNovamenteSeHouverException(() =>
            {
                var pedido = new RestRequest($"api/{Tela}/AllCustomers", Method.POST, DataFormat.Json);
                var json = JsonConvert.SerializeObject(selos, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                pedido.AddParameter("application/json", json, ParameterType.RequestBody);

                pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

                var retorno = _api.Execute(pedido);

                if (retorno.StatusCode != HttpStatusCode.OK)
                    throw new SoftparkIntegrationException();

                return retorno.Content;
            });
        }

        public string CancelarLoteSelo(int idEmissao, string numeroLote)
        {
            return TentarNovamenteSeHouverException(() =>
            {
                var pedido = new RestRequest($"api/{Tela}/InativaLoteSelo?emissaoId={idEmissao}&numeroLote={numeroLote}", Method.POST);

                pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

                var retorno = _api.Execute(pedido);

                if (retorno.StatusCode != HttpStatusCode.OK)
                    throw new SoftparkIntegrationException();

                return retorno.Content;
            });
        }
    }
}
