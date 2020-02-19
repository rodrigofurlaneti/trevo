using API.Filters;
using API.Models;
using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;

namespace API.Controllers
{
    
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoAplicacao _pagamentoAplicacao;

        public PagamentoController(IPagamentoAplicacao pagamentoAplicacao)
        {
            _pagamentoAplicacao = pagamentoAplicacao;
        }

        [HttpGet]
        [PerformanceFilter]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/Pagamentos/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os pagamentos", typeof(List<PagamentoViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                List<Pagamento> listaPagamentos = new List<Pagamento>(_pagamentoAplicacao.Buscar());
                var pagamentos = AutoMapper.Mapper.Map<List<Pagamento>, List<PagamentoViewModel>>(listaPagamentos);
                JsonResult.Object = pagamentos;
                if (pagamentos.Count > 0) { JsonResult.Message = "Foram encontrados " + pagamentos.Count.ToString() + "pagamentos. "; } else { JsonResult.Message = "Não foram encontrados pagamentos."; };
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
        [SwaggerOperation("GetByParameters")]
        [Route("api/v1/Pagamentos/GetByParameters")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os pagamentos de acordo com parametros", typeof(List<PagamentoViewModel>))]
        public HttpResponseMessage GetByParameters([FromUri] ResourceQueryPagamentos searchOptions)
        {
            try
            {
                JsonResult.Status = true;
                if (searchOptions == null)
                {
                    JsonResult.Message = "Não foram enviados parâmetros para a realização da consulta";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
                }
                List<Pagamento> listaPagamentos = new List<Pagamento>(_pagamentoAplicacao.Buscar());
                var queryable = listaPagamentos.AsQueryable();
                if (searchOptions.DataPagamento.HasValue)
                {
                    queryable = queryable.Where(x => x.DataPagamento.Date == searchOptions.DataPagamento.Value.Date);
                }
                if (searchOptions.DataEnvio.HasValue)
                {
                    queryable = queryable.Where(x => x.DataEnvio.Date == searchOptions.DataEnvio.Value.Date);
                }
                listaPagamentos = queryable.ToList();
                if (listaPagamentos.Count > 0) { JsonResult.Message = "Foram encontrados " + listaPagamentos.Count.ToString() + "pagamentos. "; } else { JsonResult.Message = "Não foram encontrados pagamentos com os parâmetros informados."; };
                var pagamentos = AutoMapper.Mapper.Map<List<Pagamento>, List<PagamentoViewModel>>(listaPagamentos);
                JsonResult.Object = pagamentos;
                return Request.CreateResponse(HttpStatusCode.OK, JsonResult);
            }
            catch (Exception ex)
            {
                JsonResult.Status = false;
                JsonResult.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
        }
    }
}