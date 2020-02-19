using Aplicacao.ViewModels;
using Core.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Aplicacao.Softpark.Base
{
    public abstract class BaseSoftparkAplicacao<T> : IBaseSoftparkAplicacao<T> where T : IBaseSoftparkViewModel
    {
        protected static RestClient _api = new RestClient(ConfigurationManager.AppSettings["API_SOFTPARK"]);
        protected TokenSoftparkViewModel Token => BuscarToken();

        public abstract string Tela { get; }

        public TokenSoftparkViewModel BuscarToken()
        {
            var pedido = new RestRequest("Token", Method.POST, DataFormat.Json);
            pedido.Parameters.Clear();
            pedido.AddParameter("grant_type", "password");
            pedido.AddParameter("username", "ronaldo");
            pedido.AddParameter("password", "p@ssw0rd");
            var retorno = _api.Execute(pedido);

            if (((int)retorno.StatusCode) >= 400)
                throw new SoftparkIntegrationException(retorno.ErrorMessage ?? retorno.StatusDescription);

            return JsonConvert.DeserializeObject<TokenSoftparkViewModel>(retorno.Content);
        }

        public virtual string Salvar(T vm)
        {
            return TentarNovamenteSeHouverException(() =>
            {
                var pedido = new RestRequest($"api/{Tela}/Customers", Method.POST, DataFormat.Json);
                var json = JsonConvert.SerializeObject(vm, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                pedido.AddJsonBody(json.Replace("\\",""));

                pedido.AddParameter("Authorization", "Bearer " + Token?.AccessToken, ParameterType.HttpHeader);

                var retorno = _api.Execute(pedido);

                if (retorno.StatusCode != HttpStatusCode.OK)
                    throw (!string.IsNullOrEmpty(retorno.ErrorMessage) ?  new SoftparkIntegrationException(retorno.ErrorMessage) : new SoftparkIntegrationException());

                return retorno.Content;
            });
        }

        public virtual void SalvarOuEditar(T vm)
        {
            var obj = BuscarPorId(vm.Id);

            if (obj == null || obj.Id == 0)
                Salvar(vm);
            else
                Editar(vm);
        }

        public virtual T BuscarPorId(int id)
        {
            return TentarNovamenteSeHouverException(() =>
            {
                var pedido = new RestRequest($"api/{Tela}/GetById?Id=" + id + "", Method.GET, DataFormat.Json);
                pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

                var retorno = _api.Execute(pedido);

                if (retorno.StatusCode == HttpStatusCode.InternalServerError)
                    throw new SoftparkIntegrationException();

                if (retorno.StatusCode != HttpStatusCode.OK)
                    return default(T);

                return JsonConvert.DeserializeObject<T>(retorno.Content);
            });
        }

        public virtual IEnumerable<T> Listar()
        {
            return TentarNovamenteSeHouverException(() =>
            {
                var pedido = new RestRequest($"api/{Tela}/GetAll", Method.GET, DataFormat.Json);
                pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

                var retorno = _api.Execute(pedido);

                if (retorno.StatusCode != HttpStatusCode.OK)
                    throw new SoftparkIntegrationException();

                return JsonConvert.DeserializeObject<IEnumerable<T>>(retorno.Content).ToList();
            });
        }

        public virtual string ExcluirPorId(int id)
        {
            return TentarNovamenteSeHouverException(() =>
            {
                var pedido = new RestRequest($"api/{Tela}/Delete?Id=" + id + "", Method.POST, DataFormat.Json);
                pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

                var retorno = _api.Execute(pedido);

                if (retorno.StatusCode != HttpStatusCode.OK)
                    throw new SoftparkIntegrationException();

                return retorno.Content;
            });
        }

        public virtual string Editar(T vm)
        {
            return TentarNovamenteSeHouverException(() =>
            {
                var pedido = new RestRequest($"api/{Tela}/Update?Id={vm.Id}", Method.POST, DataFormat.Json);

                var json = JsonConvert.SerializeObject(vm, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                pedido.AddJsonBody(json);

                pedido.AddParameter("Authorization", "Bearer " + Token.AccessToken, ParameterType.HttpHeader);

                var retorno = _api.Execute(pedido);

                if (retorno.StatusCode != HttpStatusCode.OK)
                    throw new SoftparkIntegrationException();

                return retorno.Content;
            });
        }

        protected TReturn TentarNovamenteSeHouverException<TReturn>(Func<TReturn> funcao)
        {
            int maxTentativas = 3;
            TimeSpan intervalo = TimeSpan.FromSeconds(2);
            var tentativas = 0;
            while (true)
            {
                try
                {
                    tentativas++;
                    return funcao();
                }
                catch (SoftparkIntegrationException ex)
                {
                    if (tentativas >= maxTentativas)
                        throw;

                    Task.Delay(intervalo).Wait();
                }
            }
        }
    }
}
