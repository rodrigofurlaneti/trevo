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
    public class PessoaController : ControllerBase
    {
        // GET: Pessoa
        private readonly IPessoaAplicacao _pessoaAplicacao;

        public PessoaController(IPessoaAplicacao pessoaAplicacao)
        {
            _pessoaAplicacao = pessoaAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/Pessoa/GetAll")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todas as pessoas", typeof(List<PessoaViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                List<Pessoa> listaPessoas = new List<Pessoa>(_pessoaAplicacao.Buscar());
                var pessoasVM = AutoMapper.Mapper.Map<List<Pessoa>, List<PessoaViewModel>>(listaPessoas);

                JsonResult.Status = true;
                JsonResult.Object = pessoasVM;
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
        [Route("api/v1/Pessoa/GetById/")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter pessoa específica", typeof(PessoaViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var pessoa = _pessoaAplicacao.BuscarPorId(id);
                var pessoaVM = AutoMapper.Mapper.Map<Pessoa, PessoaViewModel>(pessoa);


                JsonResult.Status = true;
                JsonResult.Object = pessoaVM;
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