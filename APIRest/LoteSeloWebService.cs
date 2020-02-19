using Aplicacao.ViewModels;
using Entidade;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebServices.Token;

namespace WebServices
{
    public class LoteSeloWebService
    {
        private static RestClient _api = new RestClient(ConfigurationManager.AppSettings["API_SOFTPARK"]);
        public static string Cadastrar(LoteSeloViewModel LoteSeloVM, TokenWS token)
        {
            var pedido = new RestRequest("api/LoteSelo/Customers", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(LoteSeloVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static string Editar(LoteSeloViewModel LoteSeloVM, TokenWS token)
        {
            var pedido = new RestRequest($"api/LoteSelo/Update?Id={LoteSeloVM.Id}", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(LoteSeloVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static IEnumerable<LoteSeloViewModel> Listar(TokenWS token)
        {
            var pedido = new RestRequest("api/LoteSelo/GetAll", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw testeRetorno.ErrorException;

            return JsonConvert.DeserializeObject<IEnumerable<LoteSeloViewModel>>(testeRetorno.Content).ToList();
        }

        public static LoteSeloViewModel BuscarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/LoteSelo/GetById?Id=" + id + "", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw testeRetorno.ErrorException;

            return JsonConvert.DeserializeObject<LoteSeloViewModel>(testeRetorno.Content);
        }

        public static string DeletarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/LoteSelo/Delete?Id=" + id + "", Method.POST, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw testeRetorno.ErrorException;

            return testeRetorno.Content;
        }
    }
}
