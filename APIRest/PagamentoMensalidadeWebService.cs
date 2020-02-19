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
    public class PagamentoMensalidadeWebService
    {
        private static RestClient _api = new RestClient(ConfigurationManager.AppSettings["API_SOFTPARK"]);
        public static string Cadastrar(PagamentoMensalidadeSoftparkViewModel pagamentoMensalidadeVM, TokenWS token)
        {
            var pedido = new RestRequest("api/PagamentoMensalidade/Customers", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(pagamentoMensalidadeVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static string Editar(PagamentoMensalidadeSoftparkViewModel pagamentoMensalidadeVM, TokenWS token)
        {
            var pedido = new RestRequest($"api/PagamentoMensalidade/Update?Id={pagamentoMensalidadeVM.Id}", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(pagamentoMensalidadeVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static IEnumerable<PagamentoMensalidadeSoftparkViewModel> Listar(TokenWS token)
        {
            var pedido = new RestRequest("api/PagamentoMensalidade/GetAll", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return JsonConvert.DeserializeObject<IEnumerable<PagamentoMensalidadeSoftparkViewModel>>(testeRetorno.Content).ToList();
        }

        public static PagamentoMensalidadeSoftparkViewModel BuscarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/PagamentoMensalidade/GetById?Id=" + id + "", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return JsonConvert.DeserializeObject<PagamentoMensalidadeSoftparkViewModel>(testeRetorno.Content);
        }

        public static string DeletarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/PagamentoMensalidade/Delete?Id=" + id + "", Method.POST, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return testeRetorno.Content;
        }
    }
}
