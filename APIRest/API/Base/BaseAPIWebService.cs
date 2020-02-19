using Aplicacao.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebServices.Token;

namespace APIRest.API
{
    public abstract class BaseWebService<T> where T : IBaseSoftparkViewModel
    {
        protected static RestClient _api = new RestClient("http://grupotrevoapi.4world.com.br/");

        public static TokenWS BuscarToken()
        {
            var pedido = new RestRequest("Token", Method.POST, DataFormat.Json);
            pedido.Parameters.Clear();
            pedido.AddParameter("grant_type", "client_credentials");
            pedido.AddParameter("client_id", "111.111.111-11");
            pedido.AddParameter("client_secret", "teste");
            var retorno = _api.Execute(pedido);
            return JsonConvert.DeserializeObject<TokenWS>(retorno.Content);
        }

        public static List<T> Listar(string tela)
        {
            var token = BuscarToken();
            var pedido = new RestRequest($"api/v1/{tela}/GetAll", Method.GET, DataFormat.Json);
            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var retorno = _api.Execute(pedido);

            if (retorno.StatusCode != HttpStatusCode.OK)
                throw new Exception("Erro na integração com o softpark, por favor contate o suporte.");

            var obj = JsonConvert.DeserializeObject<dynamic>(retorno.Content);
            var lista = (IEnumerable<T>)JsonConvert.DeserializeObject<IEnumerable<T>>(obj.Object.ToString());
            return lista.ToList();
        }

        public static string Salvar(T vm, string tela)
        {
            var token = BuscarToken();
            var pedido = new RestRequest($"api/v1/{tela}/Movements", Method.POST, DataFormat.Json);
            var json = JsonConvert.SerializeObject(vm, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            pedido.AddJsonBody(json);

            pedido.AddParameter("Authorization", "Bearer " + token.AccessToken, ParameterType.HttpHeader);

            var retorno = _api.Execute(pedido);

            if (retorno.StatusCode != HttpStatusCode.Created)
                throw new Exception("Erro na integração com o softpark, por favor contate o suporte.");

            return retorno.Content;
        }

        //public virtual void SalvarOuEditar(T vm)
        //{
        //    var obj = BuscarPorId(vm.Id);

        //    if (obj == null || obj.Id == 0)
        //        Salvar(vm);
        //    else
        //        Editar(vm);
        //}

        //public virtual T BuscarPorId(int id)
        //{
        //    return TentarNovamenteSeHouverException(() =>
        //    {
        //        var pedido = new RestRequest($"api/{Tela}/GetById?Id=" + id + "", Method.GET, DataFormat.Json);
        //        pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

        //        var retorno = _api.Execute(pedido);

        //        if (retorno.StatusCode == HttpStatusCode.InternalServerError)
        //            throw new SoftparkIntegrationException("Erro na integração com o softpark, por favor contate o suporte.");

        //        if (retorno.StatusCode != HttpStatusCode.OK)
        //            return default(T);

        //        return JsonConvert.DeserializeObject<T>(retorno.Content);
        //    });
        //}

        //public virtual string ExcluirPorId(int id)
        //{
        //    return TentarNovamenteSeHouverException(() =>
        //    {
        //        var pedido = new RestRequest($"api/{Tela}/Delete?Id=" + id + "", Method.POST, DataFormat.Json);
        //        pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

        //        var retorno = _api.Execute(pedido);

        //        if (retorno.StatusCode != HttpStatusCode.OK)
        //            throw new SoftparkIntegrationException("Erro na integração com o softpark, por favor contate o suporte.");

        //        return retorno.Content;
        //    });
        //}

        //public virtual string Editar(T vm)
        //{
        //    return TentarNovamenteSeHouverException(() =>
        //    {
        //        var pedido = new RestRequest($"api/{Tela}/Update?Id={vm.Id}", Method.POST, DataFormat.Json);

        //        var json = JsonConvert.SerializeObject(vm, Formatting.None, new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        });

        //        pedido.AddJsonBody(json);

        //        pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

        //        var retorno = _api.Execute(pedido);

        //        if (retorno.StatusCode != HttpStatusCode.OK)
        //            throw new SoftparkIntegrationException("Erro na integração com a Softpark, por favor contate o suporte.");

        //        return retorno.Content;
        //    });
        //}

        //protected TReturn TentarNovamenteSeHouverException<TReturn>(Func<TReturn> funcao)
        //{
        //    int maxTentativas = 3;
        //    TimeSpan intervalo = TimeSpan.FromSeconds(2);
        //    var tentativas = 0;
        //    while (true)
        //    {
        //        try
        //        {
        //            tentativas++;
        //            return funcao();
        //        }
        //        catch (SoftparkIntegrationException ex)
        //        {
        //            if (tentativas >= maxTentativas)
        //                throw;

        //            Task.Delay(intervalo).Wait();
        //        }
        //    }
        //}
    }
}
