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
    public class VeiculoController : ControllerBase
    {

        private readonly IVeiculoAplicacao _veiculoAplicacao;

        public VeiculoController(IVeiculoAplicacao _veiculoAplicacao)
        {
            this._veiculoAplicacao = _veiculoAplicacao;
        }

        [HttpGet]
        [PerformanceFilter]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/Veiculo/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os Veiculos", typeof(List<VeiculoViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                List<Veiculo> listaVeiculos = new List<Veiculo>(_veiculoAplicacao.Buscar());
                var Veiculos = AutoMapper.Mapper.Map<List<Veiculo>, List<VeiculoViewModel>>(listaVeiculos);
                JsonResult.Object = Veiculos;
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
        [Route("api/v1/Veiculo/GetById")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os Veiculos", typeof(List<VeiculoViewModel>))]
        public HttpResponseMessage GetById(int Id)
        {
            try
            {

                if(Id == 0)
                {
                    throw new Exception("Valor Id igual a zero");
                }

                JsonResult.Status = true;

                var objVeiculos = _veiculoAplicacao.BuscarPorId(Id);

                var Veiculos = AutoMapper.Mapper.Map<Veiculo, VeiculoViewModel>(objVeiculos);

                JsonResult.Object = Veiculos;
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