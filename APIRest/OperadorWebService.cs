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
    public class OperadorWebService
    {
        private static RestClient _api = new RestClient(ConfigurationManager.AppSettings["API_SOFTPARK"]);
        public static string Cadastrar(OperadorSoftparkViewModel operadorVM, TokenWS token)
        {
            var pedido = new RestRequest("api/Operador/Customers", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(operadorVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static string Editar(OperadorSoftparkViewModel operadorVM, TokenWS token)
        {
            var pedido = new RestRequest($"api/Operador/Update?Id={operadorVM.Id}", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(operadorVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static IEnumerable<OperadorSoftparkViewModel> Listar(TokenWS token)
        {
            var pedido = new RestRequest("api/Operador/GetAll", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return JsonConvert.DeserializeObject<IEnumerable<OperadorSoftparkViewModel>>(testeRetorno.Content).ToList();
        }

        public static OperadorSoftparkViewModel BuscarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/Operador/GetById?Id=" + id + "", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return JsonConvert.DeserializeObject<OperadorSoftparkViewModel>(testeRetorno.Content);
        }

        public static string DeletarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/Operador/Delete?Id=" + id + "", Method.POST, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return testeRetorno.Content;
        }
    }
}
