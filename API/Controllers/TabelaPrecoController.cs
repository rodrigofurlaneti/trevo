using API.Filters;
using Aplicacao;
using Aplicacao.ViewModels;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class TabelaPrecoController : ControllerBase
    {
        private readonly ITabelaPrecoAplicacao _tabelaPrecoAplicacao;

        public TabelaPrecoController(ITabelaPrecoAplicacao tabelaPrecoAplicacao)
        {
            _tabelaPrecoAplicacao = tabelaPrecoAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/TabelaPreco/GetAll")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os preços", typeof(List<TabelaPrecoViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var precos = _tabelaPrecoAplicacao.Buscar().ToList();

                JsonResult.Status = true;
                JsonResult.Object = precos;
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
        [SwaggerOperation("GetById")]
        [Route("api/v1/TabelaPreco/GetById/")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter preco específico", typeof(TabelaPrecoViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var preco = _tabelaPrecoAplicacao.BuscarPorId(id);

                JsonResult.Status = true;
                JsonResult.Object = preco;
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