using API.Filters;
using Aplicacao;
using Aplicacao.ViewModels;
using Dominio;
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
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMovimentacaoAplicacao _movimentacaoAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly ISeloAplicacao _seloAplicacao;
        private readonly IUsuarioServico _usuarioServico;


        public MovimentacaoController(IMovimentacaoAplicacao movimentacaoAplicacao, IUnidadeAplicacao unidadeAplicacao, 
                                        IClienteAplicacao clienteAplicacao, ISeloAplicacao seloAplicacao,
                                        IUsuarioServico usuarioServico)
        {
            _movimentacaoAplicacao = movimentacaoAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _seloAplicacao = seloAplicacao;
            _usuarioServico = usuarioServico;
        }

        [HttpGet]
        [SwaggerOperation("GetById")]
        [Route("api/v1/Movimentacoes/GetByIdAndUnidade")]
        [PerformanceFilter]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter uma movimentação específica", typeof(MovimentacaoSoftparkViewModel))]
        public HttpResponseMessage GetByIdAndUnidade(int id, int unidadeId)
        {
            try
            {
                var movimentacao = _movimentacaoAplicacao.PrimeiroPor(x => x.IdSoftpark.HasValue && x.IdSoftpark.Value == id && x.Unidade.Id == unidadeId);

                if (movimentacao == null)
                {
                    JsonResult.Message = "Movimentação Não Existe";
                    return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
                }

                var movimentacaoVM = new MovimentacaoSoftparkViewModel(movimentacao);

                JsonResult.Status = true;
                JsonResult.Object = movimentacaoVM;
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
        [Route("api/v1/Movimentacoes/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todas as movimentações", typeof(List<MovimentacaoSoftparkViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                var movimentacoesVM = _movimentacaoAplicacao.Buscar().Select(x => new MovimentacaoSoftparkViewModel(x)).ToList();
                JsonResult.Object = movimentacoesVM;
                if (movimentacoesVM.Count > 0) { JsonResult.Message = "Foram encontrados " + movimentacoesVM.Count.ToString() + " movimentações. "; } else { JsonResult.Message = "Não foram encontrados movimentações."; };
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
        [SwaggerOperation("Movements")]
        [Route("api/v1/Movimentacoes/Movements")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Cadastro de Movimentação", typeof(MovimentacaoSoftparkViewModel))]
        public HttpResponseMessage Movements([FromBody]MovimentacaoSoftparkViewModel movimentacaoVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    JsonResult.Message = String.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.ErrorMessage));

                    if(string.IsNullOrEmpty(JsonResult.Message))
                        JsonResult.Message = String.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.Exception.Message));

                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
                }

                var unidade = _unidadeAplicacao.BuscarPorId(movimentacaoVM.Estacionamento.Id);
                var cliente = _clienteAplicacao.BuscarPorId(movimentacaoVM.ClienteId);
                var usuario = _usuarioServico.BuscarPorId(movimentacaoVM.Operador.Id);

                var validado = Validar(movimentacaoVM.Id, unidade, usuario);
                if (validado != null)   
                    return validado;

                var movimentacao = movimentacaoVM.ToEntity();
                movimentacao.Unidade = unidade;
                movimentacao.Usuario = usuario;
                movimentacao.Cliente = cliente;

                var movimentacaoExistenteComIdSoftpark = movimentacao.IdSoftpark.HasValue ? _movimentacaoAplicacao
                                                         .PrimeiroPor(x => x.IdSoftpark.HasValue && 
                                                         x.IdSoftpark.Value == movimentacao.IdSoftpark.Value &&
                                                         x.Unidade != null && movimentacao.Unidade != null &&
                                                         x.Unidade.Id == movimentacao.Unidade.Id) : null;

                if (movimentacaoExistenteComIdSoftpark != null)
                {
                    movimentacao.Id = movimentacaoExistenteComIdSoftpark.Id;
                }

                if (movimentacao.Usuario != null)
                {
                    movimentacao.Usuario.Unidade = movimentacao.Unidade;
                    movimentacao.Usuario.TemAcessoAoPDV = true;
                }

                if (movimentacao.MovimentacaoSelo != null && movimentacao.MovimentacaoSelo.Any())
                {
                    RegistrarSelo(movimentacao);
                }
                
                JsonResult.Status = true;
                _movimentacaoAplicacao.Salvar(movimentacao);
                return Request.CreateResponse(HttpStatusCode.Created, JsonResult.Message = "Movimentação Cadastrada!");
            }
            catch (Exception ex)
            {
                JsonResult.Status = false;
                JsonResult.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }

        }

        private void RegistrarSelo(Movimentacao movimentacao)
        {
            var selosId = movimentacao.MovimentacaoSelo.Select(x => x.Selo.Id).ToList();
            var selos = _seloAplicacao.BuscarPor(x => selosId.Contains(x.Id));
            var movimentacoesSelosIdsSoftpark = movimentacao.MovimentacaoSelo.Where(x => x.IdSoftpark.HasValue).Select(x => x.IdSoftpark.Value).ToList();
            var movimentacoesSelos = _movimentacaoAplicacao.BuscarMovimentacoesSelosPelosIds(movimentacoesSelosIdsSoftpark);

            for (int i = 0; i < movimentacao.MovimentacaoSelo.Count; i++)
            {
                var seloId = movimentacao.MovimentacaoSelo[i].Selo.Id;
                movimentacao.MovimentacaoSelo[i] = movimentacoesSelos.FirstOrDefault(x => x.IdSoftpark == movimentacao.MovimentacaoSelo[i].Id) ?? movimentacao.MovimentacaoSelo[i];
                movimentacao.MovimentacaoSelo[i].Selo = selos.FirstOrDefault(x => x.Id == seloId);
                movimentacao.MovimentacaoSelo[i].Movimentacao = movimentacao;
                movimentacao.MovimentacaoSelo[i].DataInsercao = DateTime.Now;
            }

            movimentacao.MovimentacaoSelo = movimentacao.MovimentacaoSelo.Where(x => x.Selo != null).ToList();
        }

        private HttpResponseMessage Validar(int id, Unidade unidade, Usuario usuario)
        {
            if (id <= 0)
            {
                JsonResult.Message = "Id não pode ser 0.";
                return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
            }

            if (unidade == null)
            {
                JsonResult.Message = "Unidade não existe";
                return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
            }

            if (usuario == null)
            {
                JsonResult.Message = "Usuário Não Existe";
                return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
            }

            return null;
        }
    }
}
