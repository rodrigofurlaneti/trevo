using Entidade;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServices.Token;

namespace APIRest
{
   
    public class UsuarioWebService
    {
        private static RestClient _api = new RestClient("http://grupotrevoapi.4world.com.br");

        public static string  Cadastrar(Usuario usuario, TokenWS token)
        {
            var pedido = new RestRequest("api/v1/Usuario/User", Method.POST, DataFormat.Json);
            pedido.AddXmlBody(usuario);

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            return _api.Execute(pedido).Content;
        }

        public static string Excluir(int id, TokenWS token)
        {
            var pedido = new RestRequest("api/v1/Usuario/Delete?Id=" + id + "", Method.POST, DataFormat.Json);

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            return _api.Execute(pedido).Content;
        }

    }
}
