using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServices.Token;

namespace WebServices
{
    public class TokenWebService
    {
        private static RestClient _api = new RestClient(ConfigurationManager.AppSettings["API_SOFTPARK"]);

        public static TokenWS BuscarToken()
        {
            var pedido = new RestRequest("Token", Method.POST,DataFormat.Json);
            pedido.Parameters.Clear();
            pedido.AddParameter("grant_type", "password");
            pedido.AddParameter("username", "ronaldo");
            pedido.AddParameter("password", "p@ssw0rd");
            return JsonConvert.DeserializeObject<TokenWS>(_api.Execute(pedido).Content);

        }
    }
}
