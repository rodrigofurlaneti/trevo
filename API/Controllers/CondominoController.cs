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
    public class CondominoController : ControllerBase
    {
        private readonly IClienteCondominoAplicacao _condominoAplicacao;

        public CondominoController(IClienteCondominoAplicacao condominoAplicacao)
        {
            _condominoAplicacao = condominoAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/Condomino/GetAll")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todas os condominos", typeof(List<CondominoViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var unidades = _condominoAplicacao.Buscar().ToList();
                List<ClienteCondomino> listaCondominos = new List<ClienteCondomino>(_condominoAplicacao.Buscar());
                var unidadesVM = AutoMapper.Mapper.Map<List<ClienteCondomino>, List<CondominoViewModel>>(listaCondominos);

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
        [Route("api/v1/Condomino/GetById/")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter o condomino específico", typeof(CondominoViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var condomino = _condominoAplicacao.BuscarPorId(id);
                var condominoVM = AutoMapper.Mapper.Map<ClienteCondomino, CondominoViewModel>(condomino);
                JsonResult.Status = true;
                JsonResult.Object = condominoVM;
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
