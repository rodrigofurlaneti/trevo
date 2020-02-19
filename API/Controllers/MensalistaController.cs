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
    public class MensalistaController : ControllerBase
    {

        private readonly IMensalistaAplicacao _mensalistaAplicacao;

        public MensalistaController(IMensalistaAplicacao mensalistaAplicacao)
        {
            _mensalistaAplicacao = mensalistaAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/Mensalista/GetAll")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os mensalistas", typeof(List<Mensalista>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                List<Mensalista> listaMensalistas = new List<Mensalista>(_mensalistaAplicacao.Buscar());
                var pagamentos = AutoMapper.Mapper.Map<List<Mensalista>, List<MensalistaViewModel>>(listaMensalistas);
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

        [HttpGet]
        [SwaggerOperation("GetById")]
        [Route("api/v1/Mensalista/GetById")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os mensalistas", typeof(Mensalista))]
        public HttpResponseMessage GetById(int Id)
        {
            try
            {
                JsonResult.Status = true;
                var mensalista = AutoMapper.Mapper.Map<Mensalista, MensalistaViewModel>(_mensalistaAplicacao.BuscarPorId(Id));
                JsonResult.Object = mensalista;
                if (mensalista == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);

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
