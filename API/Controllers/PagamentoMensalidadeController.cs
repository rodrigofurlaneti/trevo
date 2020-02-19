using API.Filters;
using API.Models;
using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Entidade.Uteis;
using Swagger.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Extensions;
using System.Data.SqlTypes;

namespace API.Controllers
{
    public class PagamentoMensalidadeController : ControllerBase
    {
        private readonly IPagamentoMensalidadeAplicacao _pagamentoMensalidadeAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;
        private readonly IPagamentoAplicacao _pagamentoAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;

        public PagamentoMensalidadeController(IPagamentoMensalidadeAplicacao pagamentoMensalidadeAplicacao
                                              , IUnidadeAplicacao unidadeAplicacao, ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao,
                                              IPagamentoAplicacao pagamentoAplicacao,
                                              IClienteAplicacao clienteAplicacao)
        {
            _pagamentoMensalidadeAplicacao = pagamentoMensalidadeAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
            _pagamentoAplicacao = pagamentoAplicacao;
            _clienteAplicacao = clienteAplicacao;
        }

        [HttpPost]
        [PerformanceFilter]
        [SwaggerOperation("MonthlyPayments")]
        [Route("api/v1/PagamentosMensalidades/MonthlyPayments")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Cadastro de pagamento mensalidade", typeof(PagamentoMensalidadeSoftparkViewModel))]
        public HttpResponseMessage MonthlyPayments([FromBody]PagamentoMensalidadeSoftparkViewModel pagamentoMensalidadeVM)
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

                var cliente = pagamentoMensalidadeVM.Condutor.ERPId.HasValue ? _clienteAplicacao.BuscarPorId(pagamentoMensalidadeVM.Condutor.ERPId.Value) : null;
                if (cliente == null)
                    cliente = pagamentoMensalidadeVM.Condutor.ToCliente(null);

                if (cliente.Id == 0)
                    cliente = _clienteAplicacao.SalvarComRetorno(cliente);

                var unidade = _unidadeAplicacao.BuscarPorId(pagamentoMensalidadeVM.Estacionamento.Id);

                var mensagemDeErro = Validar(pagamentoMensalidadeVM, cliente, unidade);
                if (mensagemDeErro != null)
                    return mensagemDeErro;

                var lancamentoCobranca = _lancamentoCobrancaAplicacao
                                         .PrimeiroPor(x => x.Cliente.Id == cliente.Id &&
                                                      x.DataVencimento > SqlDateTime.MinValue.Value && x.DataVencimento.Date == pagamentoMensalidadeVM.DataValidade.Date);

                if (lancamentoCobranca == null)
                {
                    lancamentoCobranca = new LancamentoCobranca
                    {
                        Unidade = unidade,
                        Cliente = cliente,
                        TipoServico = TipoServico.Mensalista,
                        DataGeracao = DateTime.Now,
                        DataVencimento = pagamentoMensalidadeVM.DataValidade,
                        ValorContrato = pagamentoMensalidadeVM.Valor
                    };
                }

                if (lancamentoCobranca.Recebimento == null)
                    lancamentoCobranca.Recebimento = new Recebimento();
                if (lancamentoCobranca.Recebimento.Pagamentos == null)
                    lancamentoCobranca.Recebimento.Pagamentos = new List<Pagamento>();

                if (lancamentoCobranca.Recebimento.Pagamentos
                    .Any(x => x.PagamentoMensalistaId == pagamentoMensalidadeVM.Id &&
                    x.Unidade.Id == pagamentoMensalidadeVM.Estacionamento.Id))
                {
                    JsonResult.Message = "Já existe um pagamento com esse Id para esta Unidade";
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, JsonResult);
                }

                lancamentoCobranca.StatusLancamentoCobranca = StatusLancamentoCobranca.Pago;
                lancamentoCobranca.DataBaixa = pagamentoMensalidadeVM.DataPagamento;

                var pagamento = pagamentoMensalidadeVM.ToPagamento(lancamentoCobranca.Recebimento);
                pagamento.Unidade = unidade;

                lancamentoCobranca.Recebimento.Pagamentos.Add(pagamento);

                lancamentoCobranca.StatusLancamentoCobranca = StatusLancamentoCobranca.Pago;
                lancamentoCobranca.DataBaixa = pagamento.DataPagamento;

                _lancamentoCobrancaAplicacao.Salvar(lancamentoCobranca);

                return Request.CreateResponse(HttpStatusCode.Created, JsonResult.Message = "Pagamento Cadastrado!");
            }
            catch (Exception ex)
            {
                JsonResult.Status = false;
                JsonResult.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
        }

        private HttpResponseMessage Validar(PagamentoMensalidadeSoftparkViewModel pagamentoMensalidadeVM, Cliente cliente, Unidade unidade)
        {
            if (pagamentoMensalidadeVM.Id <= 0)
            {
                JsonResult.Message = "Id não pode ser 0";
                return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
            }

            if (cliente == null)
            {
                JsonResult.Message = "Cliente Não Existe";
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
