using API.Filters;
using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;


namespace API.Controllers
{
   // [RoutePrefix("api/Unidades")]
    public class UnidadeController : ControllerBase
    {

        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public UnidadeController(IUnidadeAplicacao unidadeAplicacao)
        {
            _unidadeAplicacao = unidadeAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/Unidade/GetAll")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todas as unidades", typeof(List<UnidadeViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var unidades = _unidadeAplicacao.Buscar().ToList();

                List<Unidade> listaUnidades = new List<Unidade>(_unidadeAplicacao.Buscar());
                var unidadesVM = AutoMapper.Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(listaUnidades);

                JsonResult.Status = true;
                JsonResult.Object = unidadesVM;
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
        [Route("api/v1/Unidade/GetById/")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter a unidade específica", typeof(UnidadeViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var unidade = _unidadeAplicacao.BuscarPorId(id);
                var unidadeVM = AutoMapper.Mapper.Map<Unidade, UnidadeViewModel>(unidade);
                JsonResult.Status = true;
                JsonResult.Object = unidadeVM;
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
        [SwaggerOperation("GetAllEstacionamentos")]
        [Route("api/v1/Unidade/GetAllEstacionamentos")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos estacionamentos", typeof(EstacionamentoSoftparkViewModel))]
        public HttpResponseMessage GetAllEstacionamentos(int id)
        {
            try
            {
                var unidades = _unidadeAplicacao.Buscar();
                var estacionamentos = unidades?.Select(x => new EstacionamentoSoftparkViewModel(x)).ToList();

                JsonResult.Object = estacionamentos;
                JsonResult.Status = true;
                return Request.CreateResponse(HttpStatusCode.OK, JsonResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
        }
    }
}
