using API.Filters;
using API.Models;
using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Entidade.Uteis;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class SeloController : ControllerBase
    {
        private readonly ISeloAplicacao _seloAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public SeloController(ISeloAplicacao seloAplicacao, IUnidadeAplicacao unidadeAplicacao)
        {
            _seloAplicacao = seloAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        [HttpGet]
        [SwaggerOperation("ValidarSelo")]
        [Route("api/v1/Selos/ValidarSelo")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Validar Selo", typeof(bool))]
        public HttpResponseMessage ValidarSelo(int id, int unidadeId)
        {
            try
            {
                var selo = _seloAplicacao.BuscarPorId(id);

                if (selo == null)
                {
                    JsonResult.Message = "Selo Não Existe";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
                }

                var seloValido = selo.EmissaoSelo.PedidoSelo.Unidade.Id == unidadeId &&
                    selo.Validade.Value < DateTime.Now &&
                    selo.EmissaoSelo.StatusSelo == StatusSelo.Ativo;

                return Request.CreateResponse(HttpStatusCode.OK, new { Valido = seloValido });
            }
            catch (Exception ex)
            {
                JsonResult.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
        }
    }
}
