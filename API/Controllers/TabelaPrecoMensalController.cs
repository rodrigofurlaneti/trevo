using API.Filters;
using Aplicacao;
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
    public class TabelaPrecoMensalController : ControllerBase
    {
        private readonly ITabelaPrecoMensalAplicacao _tabelaPrecoMensalAplicacao;
        public TabelaPrecoMensalController(ITabelaPrecoMensalAplicacao tabelaPrecoMensalAplicacao)
        {
            _tabelaPrecoMensalAplicacao = tabelaPrecoMensalAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("GetById")]
        [Route("api/v1/TabelaPrecoMensal/GetById")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter uma tabela de preço mensal", typeof(TabelaPrecoMensal))]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var tabelaPrecoMensal = _tabelaPrecoMensalAplicacao.BuscarPorId(id);

                if (tabelaPrecoMensal == null)
                {
                    JsonResult.Message = "Tabela Preço Mensal Não Existe";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
                }

                JsonResult.Status = true;
                JsonResult.Object = tabelaPrecoMensal;
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
        [Route("api/v1/TabelaPrecoMensal/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter unica tabela de preço mensal", typeof(List<TabelaPrecoMensal>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                var tabelaPrecosMensal = _tabelaPrecoMensalAplicacao.Buscar().ToList();
                JsonResult.Object = tabelaPrecosMensal;
                if (tabelaPrecosMensal.Count > 0) { JsonResult.Message = "Foram encontrados " + tabelaPrecosMensal.Count.ToString() + " tabela de preços mensais. "; } else { JsonResult.Message = "Não foram encontrados tabela de preços mensais."; };
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
        [SwaggerOperation("MonthlyPrice")]
        [Route("api/v1/TabelaPrecoMensal/MonthlyPrice")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Cadastro de Tabela de Preço Avulso", typeof(TabelaPrecoMensal))]
        public HttpResponseMessage MonthlyPrice([FromUri]TabelaPrecoMensal tabelaPrecoMensal)
        {

            try
            {
                JsonResult.Status = true;
                var camposValidados = ValidarCampos(tabelaPrecoMensal);
                if (camposValidados != null)
                    return camposValidados;

                tabelaPrecoMensal.Id = 0;
                tabelaPrecoMensal.DataInsercao = DateTime.Now;
                tabelaPrecoMensal = _tabelaPrecoMensalAplicacao.SalvarComRetorno(tabelaPrecoMensal);

                JsonResult.Object = tabelaPrecoMensal;

                return Request.CreateResponse(HttpStatusCode.OK, JsonResult);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private HttpResponseMessage ValidarCampos(TabelaPrecoMensal tabelaPrecoMensal)
        {
            if (tabelaPrecoMensal == null)
            {
                JsonResult.Message = "Não foram enviados parâmetros para o cadastro,campos Obrigatórios(" +
                    "Nome,Valor)";
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
            else if (tabelaPrecoMensal.Nome == null)
            {
                JsonResult.Message = "Insira o Nome da tabela Mensal(Nome)";
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
            else if (tabelaPrecoMensal.Valor == null)
            {
                JsonResult.Message = "Insira a valor (Valor)";
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
            else if (tabelaPrecoMensal.Valor == 0)
            {
                JsonResult.Message = "Insira o Valor corretamente Ex:(2500.07)";
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
            return null;
        }
    }
}
