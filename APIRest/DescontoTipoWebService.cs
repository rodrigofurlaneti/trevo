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
    public class DescontoTipoWebService
    {
        private static RestClient _api = new RestClient(ConfigurationManager.AppSettings["API_SOFTPARK"]);
        public static string Cadastrar(DescontoTipoViewModel descontoTipoVM, TokenWS token)
        {
            var pedido = new RestRequest("api/DescontoTipo/Customers", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(descontoTipoVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);
            return testeRetorno.Content;
        }

        public static string Editar(DescontoTipoViewModel descontoTipoVM, TokenWS token)
        {
            var pedido = new RestRequest($"api/DescontoTipo/Update?Id={descontoTipoVM.Id}", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(descontoTipoVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static IEnumerable<DescontoTipoViewModel> Listar(TokenWS token)
        {
            var pedido = new RestRequest("api/DescontoTipo/GetAll", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw testeRetorno.ErrorException;

            return JsonConvert.DeserializeObject<IEnumerable<DescontoTipoViewModel>>(testeRetorno.Content).ToList();
        }

        public static DescontoTipoViewModel BuscarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/DescontoTipo/GetById?Id=" + id + "", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw testeRetorno.ErrorException;

            return JsonConvert.DeserializeObject<DescontoTipoViewModel>(testeRetorno.Content);
        }

        public static string DeletarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/DescontoTipo/Delete?Id=" + id + "", Method.POST, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw testeRetorno.ErrorException;

            return testeRetorno.Content;
        }
    }
}
