using API.Filters;
using API.Models;
using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Swagger.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class RecebimentoController : ControllerBase
    {
        private readonly IRecebimentoAplicacao _recebimentoAplicacao;

        public RecebimentoController(IRecebimentoAplicacao recebimentoAplicacao)
        {
            _recebimentoAplicacao = recebimentoAplicacao;
        }

        [HttpGet]
        [PerformanceFilter]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/Recebimentos/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os recebimentos", typeof(List<RecebimentoViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                List<Recebimento> listaRecebimentos = new List<Recebimento>(_recebimentoAplicacao.Buscar());
                var recebimentos = AutoMapper.Mapper.Map<List<Recebimento>, List<RecebimentoViewModel>>(listaRecebimentos);
                JsonResult.Object = recebimentos;
                if (listaRecebimentos.Count > 0) { JsonResult.Message = "Foram encontrados " + listaRecebimentos.Count.ToString() + "recebimentos. "; } else { JsonResult.Message = "Não foram encontrados recebimentos."; };
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
        [Route("api/v1/Recebimentos/GetByParameters")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os recebimentos de acordo com parametros", typeof(List<RecebimentoViewModel>))]
        public HttpResponseMessage GetByParameters([FromUri] ResourceQueryRecebimentos searchOptions)
        {
            try
            {
                JsonResult.Status = true;
                if (searchOptions == null)
                {
                    JsonResult.Message = "Não foram enviados parâmetros para a realização da consulta";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
                }
                List<Recebimento> listaRecebimentos = new List<Recebimento>(_recebimentoAplicacao.Buscar());
                var queryable = listaRecebimentos.AsQueryable();
                if (searchOptions.StatusRecebimento != null)
                {
                    queryable = queryable.Where(x => x.StatusRecebimento == searchOptions.StatusRecebimento);
                }
                if (searchOptions.DataVencimento.HasValue)
                {
                    queryable = queryable.Where(m => m.LancamentosCobranca.Any(x => x.DataVencimento.Date == searchOptions.DataVencimento.Value.Date));
                }
                listaRecebimentos = queryable.ToList();
                if (listaRecebimentos.Count > 0) { JsonResult.Message = "Foram encontrados " + listaRecebimentos.Count.ToString() + "recebimentos. "; } else { JsonResult.Message = "Não foram encontrados recebimentos com os parâmetros informados."; };
                var recebimentos = AutoMapper.Mapper.Map<List<Recebimento>, List<RecebimentoViewModel>>(listaRecebimentos);
                JsonResult.Object = recebimentos;
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
