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

namespace API.Controllers
{
    public class HorarioUnidadeController : ControllerBase
    {
        private readonly IHorarioUnidadeAplicacao _horarioUnidadeAplicacao;

        public HorarioUnidadeController(IHorarioUnidadeAplicacao horarioUnidadeAplicacao)
        {
            _horarioUnidadeAplicacao = horarioUnidadeAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/HorarioUnidade/GetAll")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os horários das unidades", typeof(List<HorarioUnidadeViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                List<HorarioUnidade> listaHorarioUnidades = new List<HorarioUnidade>(_horarioUnidadeAplicacao.Buscar());
                var horarioUnidadesVM = AutoMapper.Mapper.Map<List<HorarioUnidade>, List<HorarioUnidadeViewModel>>(listaHorarioUnidades);

                JsonResult.Status = true;
                JsonResult.Object = horarioUnidadesVM;
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
        [Route("api/v1/HorarioUnidade/GetById/")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter horario de unidade específica", typeof(HorarioUnidadeViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {

                var horarioUnidade = _horarioUnidadeAplicacao.BuscarPorId(id);
                var horarioUnidadeVM = AutoMapper.Mapper.Map<HorarioUnidade, HorarioUnidadeViewModel>(horarioUnidade);

                JsonResult.Status = true;
                JsonResult.Object = horarioUnidadeVM;
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
