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
    public class TabelaPrecoMensalWebService
    {
        private static RestClient _api = new RestClient(ConfigurationManager.AppSettings["API_SOFTPARK"]);
        public static string Cadastrar(TabelaPrecoMensalSoftparkViewModel tabelaPrecoMensalVM, TokenWS token)
        {
            var pedido = new RestRequest("api/TabelaPrecoMensal/Customers", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(tabelaPrecoMensalVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static string Editar(TabelaPrecoMensalSoftparkViewModel tabelaPrecoMensalVM, TokenWS token)
        {
            var pedido = new RestRequest($"api/TabelaPrecoMensal/Update?Id={tabelaPrecoMensalVM.Id}", Method.POST, DataFormat.Json);

            pedido.AddJsonBody(JsonConvert.SerializeObject(tabelaPrecoMensalVM));

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusDescription);

            return testeRetorno.Content;
        }

        public static IEnumerable<TabelaPrecoMensalSoftparkViewModel> Listar(TokenWS token)
        {
            var pedido = new RestRequest("api/TabelaPrecoMensal/GetAll", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return JsonConvert.DeserializeObject<IEnumerable<TabelaPrecoMensalSoftparkViewModel>>(testeRetorno.Content).ToList();
        }

        public static TabelaPrecoMensalSoftparkViewModel BuscarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/TabelaPrecoMensal/GetById?Id=" + id + "", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return JsonConvert.DeserializeObject<TabelaPrecoMensalSoftparkViewModel>(testeRetorno.Content);
        }

        public static string DeletarPorId(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/TabelaPrecoMensal/Delete?Id=" + id + "", Method.POST, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var testeRetorno = _api.Execute(pedido);

            if (testeRetorno.StatusCode != HttpStatusCode.OK)
                throw new Exception(testeRetorno.StatusCode.ToString());

            return testeRetorno.Content;
        }
    }
}
