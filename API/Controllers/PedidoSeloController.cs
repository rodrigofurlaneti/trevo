using API.Filters;
using API.Models;
using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Entidade.Uteis;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace API.Controllers
{
    public class PedidoSeloController : ControllerBase
    {
        private readonly IPedidoSeloAplicacao _pedidoSeloAplicacao;

        public PedidoSeloController(IPedidoSeloAplicacao pedidoSeloAplicacao)
        {
            _pedidoSeloAplicacao = pedidoSeloAplicacao;
        }

        [HttpGet]
        [PerformanceFilter]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/PedidoSelo/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os PedidoSelos", typeof(List<PedidoSeloViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                List<PedidoSelo> listaPedidoSelos = new List<PedidoSelo>(_pedidoSeloAplicacao.Buscar());
                var PedidoSelos = AutoMapper.Mapper.Map<List<PedidoSelo>, List<PedidoSeloViewModel>>(listaPedidoSelos);
                JsonResult.Object = PedidoSelos;
                return Request.CreateResponse(HttpStatusCode.OK, JsonResult);
            }
            catch (Exception ex)
            {
                JsonResult.Status = false;
                JsonResult.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
        }

        [HttpGet]
        [PerformanceFilter]
        [SwaggerOperation("GetById")]
        [Route("api/v1/PedidoSelo/GetById")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os PedidoSelos", typeof(List<PedidoSeloViewModel>))]
        public HttpResponseMessage GetById(int Id)
        {
            try
            {
                if (Id == 0)
                    throw new Exception("Valor Id igual a zero");

                JsonResult.Status = true;
                var objPedidoSelos = _pedidoSeloAplicacao.BuscarPorId(Id);
                var PedidoSelos = AutoMapper.Mapper.Map<PedidoSelo, PedidoSeloViewModel>(objPedidoSelos);
                JsonResult.Object = PedidoSelos;

                return Request.CreateResponse(HttpStatusCode.OK, JsonResult);
            }
            catch (Exception ex)
            {
                JsonResult.Status = false;
                JsonResult.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [PerformanceFilter]
        [SwaggerOperation("AprovacaoPedido")]
        [Route("api/v1/PedidoSelo/AprovacaoPedido")]
        [SwaggerResponse(HttpStatusCode.Created, "<b>Nota: </b>Aprovação de Pedido Selo", typeof(string))]
        public HttpResponseMessage AprovacaoPedido(string chave)
        {
            var htmlAprovacaoPedidoSelo = ConfigurationManager.AppSettings["HTML_APROVACAO_PEDIDO_SELO"];

            try
            {
                if (!ModelState.IsValid)
                {
                    JsonResult.Message = string.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.ErrorMessage));
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
                }
                JsonResult.Status = true;

                var dados = _pedidoSeloAplicacao.DescriptografarChave(chave);
                var pedido = _pedidoSeloAplicacao.BuscarPorId(dados.Id);
                _pedidoSeloAplicacao.Salvar(pedido, 1, AcaoWorkflowPedido.Aprovar, dados.Descricao);
                
                var pedidoRetorno = _pedidoSeloAplicacao.BuscarPorId(pedido.Id);

                if (pedidoRetorno.PedidoSeloHistorico.Any(x => x.StatusPedidoSelo == StatusPedidoSelo.AprovadoPeloCliente))
                {
                    htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[DIV_ALERT]", "success");
                    htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[MSG_APROVACAO]", "Pedido Selo APROVADO com Sucesso!");
                }
                else
                {
                    htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[DIV_ALERT]", "warning");
                    htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[MSG_APROVACAO]", "O pedido não pode ser APROVADO, pois já foi REPROVADO anteriormente!");
                }

                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created,
                    Content = new StringContent(htmlAprovacaoPedidoSelo)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }
            catch (Exception ex)
            {
                htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[DIV_ALERT]", "danger");
                htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[MSG_APROVACAO]", ex.Message);

                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(htmlAprovacaoPedidoSelo)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [PerformanceFilter]
        [SwaggerOperation("ReprovacaoPedido")]
        [Route("api/v1/PedidoSelo/ReprovacaoPedido")]
        [SwaggerResponse(HttpStatusCode.Created, "<b>Nota: </b>Aprovação de Pedido Selo", typeof(string))]
        public HttpResponseMessage ReprovacaoPedido(string chave)
        {
            var htmlAprovacaoPedidoSelo = ConfigurationManager.AppSettings["HTML_APROVACAO_PEDIDO_SELO"];

            try
            {
                if (!ModelState.IsValid)
                {
                    JsonResult.Message = string.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.ErrorMessage));
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
                }
                JsonResult.Status = true;

                var dados = _pedidoSeloAplicacao.DescriptografarChave(chave);
                var pedido = _pedidoSeloAplicacao.BuscarPorId(dados.Id);
                _pedidoSeloAplicacao.Salvar(pedido, 1, AcaoWorkflowPedido.Reprovar, dados.Descricao);
                
                var pedidoRetorno = _pedidoSeloAplicacao.BuscarPorId(pedido.Id);

                if (pedidoRetorno.PedidoSeloHistorico.Any(x => x.StatusPedidoSelo == StatusPedidoSelo.ReprovadoPeloCliente))
                {
                    htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[DIV_ALERT]", "success");
                    htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[MSG_APROVACAO]", "Pedido Selo REPROVADO com Sucesso!");
                }
                else
                {
                    htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[DIV_ALERT]", "warning");
                    htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[MSG_APROVACAO]", "O pedido não pode ser REPROVADO, pois já foi APROVADO anteriormente!");
                }
                
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created,
                    Content = new StringContent(htmlAprovacaoPedidoSelo)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }
            catch (Exception ex)
            {
                htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[DIV_ALERT]", "danger");
                htmlAprovacaoPedidoSelo = htmlAprovacaoPedidoSelo.Replace("[MSG_APROVACAO]", ex.Message);

                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(htmlAprovacaoPedidoSelo)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [PerformanceFilter]
        [SwaggerOperation("GerarLancamentoCobranca")]
        [Route("api/v1/PedidoSelo/GerarLancamentoCobranca")]
        [SwaggerResponse(HttpStatusCode.Created, "<b>Nota: </b>Gerar Lançamento de Cobrança", typeof(string))]
        public HttpResponseMessage GerarLancamentoCobranca(IdVM vm)
        {
            var idPedido = vm.Id;
            try
            {
                JsonResult.Status = true;
                _pedidoSeloAplicacao.GerarBoleto(idPedido, 1);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult.Message = ex.Message);
            }
        }
    }
}