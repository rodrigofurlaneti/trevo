using API.Filters;
using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class HorarioPrecoController : ControllerBase
    {
        private readonly IPrecoAplicacao _horarioPrecoAplicacao;

        public HorarioPrecoController(IPrecoAplicacao horarioPrecoAplicacao)
        {
            _horarioPrecoAplicacao = horarioPrecoAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/HorarioPreco/GetAll")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os preços no horário", typeof(List<PrecoViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {

                List<Preco> listaHorarioPrecos = new List<Preco>(_horarioPrecoAplicacao.Buscar());
                var horarioPrecosVM = AutoMapper.Mapper.Map<List<Preco>, List<PrecoViewModel>>(listaHorarioPrecos);

                JsonResult.Status = true;
                JsonResult.Object = horarioPrecosVM;
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
        [Route("api/v1/HorarioPreco/GetById/")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter um preço no horário específico", typeof(PrecoViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var horarioPreco = _horarioPrecoAplicacao.BuscarPorId(id);
                var horarioPrecoVM = AutoMapper.Mapper.Map<Preco, PrecoViewModel>(horarioPreco);


                JsonResult.Status = true;
                JsonResult.Object = horarioPrecoVM;
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