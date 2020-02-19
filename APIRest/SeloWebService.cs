using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacao.ViewModels;
using Entidade;
using WebServices.Token;

namespace WebServices
{
    public class SeloWebService
    {
        private static RestClient _api = new RestClient("http://grupotrevoapi.4world.com.br");
        public static string EnviarLote(Selo selo, TokenWS token)
        {
            var pedido = new RestRequest("api/v1/LoteSelo/SendLot", Method.POST, DataFormat.Json);
            pedido.AddXmlBody(selo);

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);
            return _api.Execute(pedido).Content;
        }
        public static string BloquearLote(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/v1/LoteSelo/BlockLot?Id=" + id + "", Method.PUT, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            return _api.Execute(pedido).Content;
        }
        public static string  DesbloquearLote(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/v1/LoteSelo/UnlockLot?Id=" + id + "", Method.POST, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            return _api.Execute(pedido).Content;
        }
        public static string BloquearSelo(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/v1/Selo/BlockStamp?Id=" + id + "", Method.PUT, DataFormat.Json);

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            return _api.Execute(pedido).Content;
        }
        public static string DesbloquearSelo(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/v1/Selo/UnlockStamp?Id=" + id + "", Method.PUT, DataFormat.Json);

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            return _api.Execute(pedido).Content;
        }

    }
}
