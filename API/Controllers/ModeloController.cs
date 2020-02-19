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
    public class ModeloController : ControllerBase
    {
        private readonly IModeloAplicacao _modeloAplicacao;
        private readonly IMarcaAplicacao _marcaoAplicacao;
        public ModeloController(IModeloAplicacao modeloAplicacao, IMarcaAplicacao marcaoAplicacao)
        {
            _modeloAplicacao = modeloAplicacao;
            _marcaoAplicacao = marcaoAplicacao;
    }

        [HttpGet]
        [SwaggerOperation("GetById")]
        [Route("api/v1/Modelos/GetById")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter uma modelo específico", typeof(ModeloViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var modeloDM = _modeloAplicacao.BuscarPorId(id);
                var modeloVM = AutoMapper.Mapper.Map<Modelo, ModeloViewModel>(modeloDM);

                if (modeloVM == null)
                {
                    JsonResult.Message = "Modelo Não Existe";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
                }

                JsonResult.Status = true;
                JsonResult.Object = modeloVM;
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
        [Route("api/v1/Modelos/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os modelos", typeof(List<ModeloViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                var modelosDM = _modeloAplicacao.Buscar().ToList();
                var modelosVM = AutoMapper.Mapper.Map<List<Modelo>, List<ModeloViewModel>>(modelosDM);
                JsonResult.Object = modelosVM;

                if (modelosVM.Count > 0) { JsonResult.Message = "Foram encontrados " + modelosVM.Count.ToString() + " Modelos. "; } else { JsonResult.Message = "Não foram encontrados modelos."; };
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
        [SwaggerOperation("Models")]
        [Route("api/v1/Modelos/Models")]
        [SwaggerResponse(HttpStatusCode.Created, "<b>Nota: </b>Cadastro de Marca de Veiculo", typeof(ModeloViewModel))]
        public HttpResponseMessage Models([FromBody]ModeloViewModel modelo)
        {
            try
            {
                var dadoValidado = ValidarDados(modelo);
                if (dadoValidado != null)
                    return dadoValidado;
               
                JsonResult.Status = true;
                modelo.Id = 0;
                modelo.DataInsercao = DateTime.Now;
                _modeloAplicacao.Salvar(modelo.ToEntity());

                return Request.CreateResponse(HttpStatusCode.Created, JsonResult.Message = "Modelo Cadastrado!");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult.Message = ex.InnerException.Message);
            }
        }

        private HttpResponseMessage ValidarDados(ModeloViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                JsonResult.Message = String.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.ErrorMessage));
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
            }

            var modeloDM = _modeloAplicacao.BuscarPor(x => x.Descricao.ToLower() == modelo.Descricao.ToLower()).FirstOrDefault();
            if (modeloDM != null)
            {
                var modeloVM = AutoMapper.Mapper.Map<Modelo, ModeloViewModel>(modeloDM);
                JsonResult.Message = "Modelo ja Cadastrado!";
                JsonResult.Object = modeloVM;
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
            }
            var marcaDM = _marcaoAplicacao.BuscarPorId(modelo.Marca.Id);
            if (marcaDM == null)
            {
                JsonResult.Message = "Marca não existe em nosso sistema, inserir Id correto!";
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
            }


            return null;
        }
    }
}
