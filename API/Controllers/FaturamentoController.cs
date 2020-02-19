using API.Filters;
using API.Models;
using Aplicacao;
using Aplicacao.ViewModels;
using Dominio;
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
    public class FaturamentoController : ControllerBase
    {
        private readonly IFaturamentoAplicacao _faturamentoAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IUsuarioServico _usuarioServico;

        public FaturamentoController(
            IFaturamentoAplicacao faturamentoAplicacao, 
            IUnidadeAplicacao unidadeAplicacao,
            IUsuarioServico usuarioServico)
        {
            _faturamentoAplicacao = faturamentoAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _usuarioServico = usuarioServico;
        }

        [HttpGet]
        [SwaggerOperation("GetById")]
        [Route("api/v1/Faturamentos/GetByIdAndUnidade")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter um faturamento específico", typeof(FaturamentoViewModel))]
        public HttpResponseMessage GetByIdAndUnidade(int id, int unidadeId)
        {
            try
            {
                var faturamento = _faturamentoAplicacao.PrimeiroPor(x => x.IdSoftpark.HasValue && x.IdSoftpark.Value == id && 
                                                                         x.Unidade != null && x.Unidade.Id == unidadeId);

                if (faturamento == null)
                {
                    JsonResult.Message = "Faturamento Não Existe";
                    return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
                }

                var faturamentoViewModel = new FaturamentoViewModel(faturamento);

                JsonResult.Status = true;
                JsonResult.Object = faturamentoViewModel;
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
        [Route("api/v1/Faturamentos/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os faturamentos", typeof(List<FaturamentoViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                var faturamentosViewModel = _faturamentoAplicacao.Buscar().Select(x => new FaturamentoViewModel(x)).ToList();

                foreach (var faturamento in faturamentosViewModel)
                {
                    if (faturamento.Estacionamento != null)
                        faturamento.Estacionamento.Id = faturamento.Estacionamento.Id;
                }
                JsonResult.Object = faturamentosViewModel;
                if (faturamentosViewModel.Count > 0) { JsonResult.Message = "Foram encontrados " + faturamentosViewModel.Count.ToString() + " faturamentos. "; } else { JsonResult.Message = "Não foram encontrados faturamentos."; };
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
        [SwaggerOperation("billings")]
        [Route("api/v1/Faturamentos/Billings")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Cadastro de Faturamento", typeof(FaturamentoViewModel))]
        public HttpResponseMessage Billings([FromBody]FaturamentoViewModel faturamentoViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    JsonResult.Message = String.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.ErrorMessage));

                    if (string.IsNullOrEmpty(JsonResult.Message))
                        JsonResult.Message = String.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.Exception.Message));

                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
                }

                var unidade = _unidadeAplicacao.BuscarPorId(faturamentoViewModel.Estacionamento.Id);
                var usuario = _usuarioServico.BuscarPorId(faturamentoViewModel.Operador.Id);

                if (usuario == null)
                {
                    JsonResult.Message = "Usuário Não Existe";
                    return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
                }

                var mensagemDeErro = Validar(faturamentoViewModel.Id, unidade);
                if (mensagemDeErro != null)
                    return mensagemDeErro;
                    
                var faturamento = faturamentoViewModel.ToEntity();
                faturamento.Unidade = unidade;
                faturamento.Usuario = usuario;

                var faturamentoExistenteComIdSoftpark = faturamento.IdSoftpark.HasValue ? _faturamentoAplicacao
                                                         .PrimeiroPor(x => x.IdSoftpark.HasValue &&
                                                         x.IdSoftpark.Value == faturamento.IdSoftpark.Value &&
                                                         x.Unidade != null && faturamento.Unidade != null &&
                                                         x.Unidade.Id == faturamento.Unidade.Id) : null;

                if (faturamentoExistenteComIdSoftpark != null)
                {
                    faturamento.Id = faturamentoExistenteComIdSoftpark.Id;
                }

                _faturamentoAplicacao.Salvar(faturamento);

                JsonResult.Status = true;
                JsonResult.Object = faturamento;
                return Request.CreateResponse(HttpStatusCode.Created, JsonResult.Message = "Faturamento Cadastrado!");
            }
            catch (Exception ex)
            {
                JsonResult.Status = false;
                JsonResult.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult.Message);
            }
        }

        private HttpResponseMessage Validar(int id, Unidade unidade)
        {
            if (id <= 0)
            {
                JsonResult.Message = "Id não pode ser 0.";
                return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
            }

            if (unidade == null)
            {
                JsonResult.Message = "Unidade Não Existe";
                return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
            }

            return null;
        }
    }
}
