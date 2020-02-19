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
    public class TabelaPrecoAvulsoWebService
    {
        private static RestClient _api = new RestClient(ConfigurationManager.AppSettings["API_SOFTPARK"]);
        public static string Cadastrar(TabelaPrecoSoftparkViewModel tabelaPrecoAvulso, TokenWS token)
        {
            var pedido = new RestRequest("api/TabelaPreco/Customers", Method.POST, DataFormat.Json);

            var json = JsonConvert.SerializeObject(tabelaPrecoAvulso, Formatting.Indented,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }
                        );

            pedido.AddJsonBody(json);

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return testeRetorno.Content;
        }

        public static TabelaPrecoSoftparkViewModel BuscarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/TabelaPreco/GetById?Id=" + id + "", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return JsonConvert.DeserializeObject<TabelaPrecoSoftparkViewModel>(testeRetorno.Content);
        }

        public static IEnumerable<TabelaPrecoSoftparkViewModel> Listar(TokenWS token)
        {
            var pedido = new RestRequest("api/TabelaPreco/GetAll", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return JsonConvert.DeserializeObject<IEnumerable<TabelaPrecoSoftparkViewModel>>(testeRetorno.Content).ToList();
        }

        public static string Editar(TabelaPrecoSoftparkViewModel tabelaPrecoVM, TokenWS token)
        {
            var pedido = new RestRequest($"api/TabelaPreco/Update?Id={tabelaPrecoVM.Id}", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(tabelaPrecoVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static string DeletarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/TabelaPreco/Delete?Id=" + id + "", Method.POST, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return testeRetorno.Content;
        }
    }
}
