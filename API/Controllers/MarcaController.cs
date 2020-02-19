using API.Filters;
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
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaAplicacao _marcaAplicacao;
        public MarcaController(IMarcaAplicacao marcaAplicacao)
        {
            _marcaAplicacao = marcaAplicacao;
        }



        [HttpGet]
        [SwaggerOperation("GetById")]
        [Route("api/v1/Marcas/GetById")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter uma marca específica", typeof(MarcaViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var marcaDM = _marcaAplicacao.BuscarPorId(id);
                var marcaVM =  AutoMapper.Mapper.Map<Marca, MarcaViewModel>(marcaDM);

                if (marcaDM == null)
                {
                    JsonResult.Message = "Marca Não Existe";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
                }

                JsonResult.Status = true;
                JsonResult.Object = marcaVM;
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
        [Route("api/v1/Marcas/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos as marcas", typeof(List<MarcaViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                var marcasDM = _marcaAplicacao.Buscar().ToList();
                var marcasVM = AutoMapper.Mapper.Map<List<Marca>, List<MarcaViewModel>>(marcasDM);
                JsonResult.Object = marcasVM;

                if (marcasVM.Count > 0) { JsonResult.Message = "Foram encontrados " + marcasVM.Count.ToString() + " Marcas. "; } else { JsonResult.Message = "Não foram encontrados marcas."; };
                return Request.CreateResponse(HttpStatusCode.OK, JsonResult);
            }
            catch (Exception ex)
            {
                JsonResult.Status = false;
                JsonResult.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
        }


        [HttpPost]
        [PerformanceFilter]
        [SwaggerOperation("Marks")]
        [Route("api/v1/Marcas/Marks")]
        [SwaggerResponse(HttpStatusCode.Created, "<b>Nota: </b>Cadastro de Marca de Veiculo", typeof(MarcaViewModel))]
        public HttpResponseMessage Marks([FromBody]MarcaViewModel marca)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    JsonResult.Message = String.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.ErrorMessage));
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
                }

                var MarcaExiste = _marcaAplicacao.BuscarPor(x => x.Nome.ToLower() == marca.Nome.ToLower()).FirstOrDefault();
                if (MarcaExiste != null)
                {
                    JsonResult.Message = "Marca ja Cdastrada!";
                    JsonResult.Object = MarcaExiste;
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
                }
                JsonResult.Status = true;
                marca.Id = 0;
                marca.DataInsercao = DateTime.Now;
                _marcaAplicacao.Salvar(marca.ToEntity());

                return Request.CreateResponse(HttpStatusCode.Created, JsonResult.Message = "Cliente Cadastrado!");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult.Message = ex.InnerException.Message);
            }
        }
    }
}
