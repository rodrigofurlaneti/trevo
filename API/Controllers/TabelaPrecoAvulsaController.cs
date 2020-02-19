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
    public class TabelaPrecoAvulsaController : ControllerBase
    {
        private readonly ITabelaPrecoAvulsaAplicacao _tabelaPrecoAvulsaAplicacao;

        public TabelaPrecoAvulsaController(ITabelaPrecoAvulsaAplicacao tabelaPrecoAvulsaAplicacao)
        {
            _tabelaPrecoAvulsaAplicacao = tabelaPrecoAvulsaAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("GetById")]
        [Route("api/v1/TabelaPrecoAvulsa/GetById")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter uma tabela de preço avulsa", typeof(TabelaPrecoAvulsaViewModel))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var tabelaPrecoAvulsa = _tabelaPrecoAvulsaAplicacao.BuscarPorId(id);

                var tabelaPrecoAvulsaVM = AutoMapper.Mapper.Map<TabelaPrecoAvulsa, TabelaPrecoAvulsaViewModel>(tabelaPrecoAvulsa);

                if (tabelaPrecoAvulsaVM == null)
                {
                    JsonResult.Message = "Tabela Preço Avulsa Não Existe";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
                }

                JsonResult.Status = true;
                JsonResult.Object = tabelaPrecoAvulsaVM;
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
        [Route("api/v1/TabelaPrecoAvulsa/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter unica tabela de preço avulsa", typeof(List<TabelaPrecoAvulsaViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                var tabelaPrecosAvulsa = _tabelaPrecoAvulsaAplicacao.Buscar().ToList();

                var tabelaPrecosAvulsaVM = AutoMapper.Mapper.Map<List<TabelaPrecoAvulsa>, List<TabelaPrecoAvulsaViewModel>>(tabelaPrecosAvulsa);
                JsonResult.Object = tabelaPrecosAvulsaVM;
                if (tabelaPrecosAvulsa.Count > 0) { JsonResult.Message = "Foram encontrados " + tabelaPrecosAvulsa.Count.ToString() + " tabela de preços avulsos. "; } else { JsonResult.Message = "Não foram encontrados tabela de preços avulsos."; };
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
        [SwaggerOperation("SinglePrice")]
        [Route("api/v1/TabelaPrecoAvulsa/SinglePrice")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Cadastro de Tabela de Preço Avulso", typeof(TabelaPrecoAvulsaViewModel))]
        public HttpResponseMessage SinglePrice([FromBody]TabelaPrecoAvulsaViewModel tabelaPrecoAvulsa)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    JsonResult.Message = String.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.ErrorMessage));
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
                }
                JsonResult.Status = true;
                tabelaPrecoAvulsa.Id = 0;
                tabelaPrecoAvulsa.DataInsercao = DateTime.Now;


                var tabelaPrecoAvulsaDM = AutoMapper.Mapper.Map<TabelaPrecoAvulsaViewModel, TabelaPrecoAvulsa>(tabelaPrecoAvulsa);

                _tabelaPrecoAvulsaAplicacao.Salvar(tabelaPrecoAvulsaDM);


                return Request.CreateResponse(HttpStatusCode.Created, JsonResult.Message = "Preço avulso Cadastrado!");

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
