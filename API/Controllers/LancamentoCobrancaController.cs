using API.Filters;
using Aplicacao;
using Swagger.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Aplicacao.ViewModels;
using Entidade;

namespace API.Controllers
{
    public class LancamentoCobrancaController : ControllerBase
    {
        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;

        public LancamentoCobrancaController(ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao)
        {
            _lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
        }


        [HttpGet]
        [SwaggerOperation("GetById")]
        [Route("api/v1/Cobrancas/GetById")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter uma cobranca específica", typeof(LancamentoCobrancaViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var lancamentoCobrancaDM = _lancamentoCobrancaAplicacao.BuscarPorId(id);
                var lancamentoCobrancaVM =  AutoMapper.Mapper.Map<LancamentoCobranca, LancamentoCobrancaViewModel>(lancamentoCobrancaDM);

                if (lancamentoCobrancaVM == null)
                {
                    JsonResult.Message = "Cobrança Não Existe";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
                }

                JsonResult.Status = true;
                JsonResult.Object = lancamentoCobrancaVM;
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
        [SwaggerOperation("GetAll")]
        [Route("api/v1/Cobrancas/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todas as cobranças", typeof(List<LancamentoCobrancaViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                var lancamentosCobrancasDM = _lancamentoCobrancaAplicacao.Buscar().ToList();
                //var lancamentosCobrancasVM = AutoMapper.Mapper.Map<List<LancamentoCobranca>, List<LancamentoCobrancaViewModel>>(lancamentosCobrancasDM);
                JsonResult.Object = lancamentosCobrancasDM;
                if (lancamentosCobrancasDM.Count > 0) { JsonResult.Message = "Foram encontrados " + lancamentosCobrancasDM.Count.ToString() + " cobranças. "; } else { JsonResult.Message = "Não foram encontrados cobranças."; };
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
