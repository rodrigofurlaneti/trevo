using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Entidade;
using Entidade.Uteis;
using OfficeOpenXml;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class RelatoriosFinanceirosController : GenericController<LancamentoCobranca>
    {
        public IEnumerable<ChaveValorViewModel> ListaTipoRelatorioFinanceiro
        {
            get
            {
                var lista = Enum.GetValues(typeof(TipoRelatorioFinanceiro)).Cast<TipoRelatorioFinanceiro>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() })?.ToList();
                lista.Insert(0, new ChaveValorViewModel(0, "Selecione..."));
                return lista;
            }
        }

        public IEnumerable<ChaveValorViewModel> ListaTipoServico
        {
            get
            {
                return Enum.GetValues(typeof(TipoServico)).Cast<TipoServico>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() });
            }
        }

        public List<UnidadeViewModel> ListaUnidade
        {
            get
            {
                return _unidadeAplicacao.ListarOrdenadoSimplificado()?.Select(x => new UnidadeViewModel { Id = x.Id, Codigo = x.Codigo, Nome = x.Nome })?.ToList() ?? new List<UnidadeViewModel>();
            }
        }

        public bool QuebraPaginaPorUnidade { get; set; }

        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;
        private readonly ISeloAplicacao _seloAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public RelatoriosFinanceirosController(ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao,
                                                ISeloAplicacao seloAplicacao,
                                                IUnidadeAplicacao unidadeAplicacao)
        {
            Aplicacao = lancamentoCobrancaAplicacao;
            _lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
            _seloAplicacao = seloAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();

            return View("Index");
        }

        [HttpPost]
        public JsonResult FiltroTipoServico(TipoRelatorioFinanceiro tipoRelatorio)
        {
            var listaRetorno = Enum.GetValues(typeof(TipoServico))
                .Cast<TipoServico>()
                .Where(x => tipoRelatorio == TipoRelatorioFinanceiro.PagamentosEmAberto
                            || tipoRelatorio == TipoRelatorioFinanceiro.PagamentosEfetuados
                            ? x != TipoServico.Avulso
                            : true)
                .Select(e => new ChaveValorViewModel
                {
                    Id = (int)e,
                    Descricao = e.ToDescription()
                })
                .ToList();

            return Json(listaRetorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult RelatorioModal(string dataInicio, string dataFim, TipoRelatorioFinanceiro tipoRelatorio, TipoServico tipoServico, string unidade)
        {
            QuebraPaginaPorUnidade = true;

            var dados = string.Empty;
            var dadosRetornoConsulta = new object();
            var unidadeFiltro = string.IsNullOrEmpty(unidade) ? 0 : Convert.ToInt32(unidade);
            try
            {
                switch (tipoRelatorio)
                {
                    case TipoRelatorioFinanceiro.PagamentosEmAberto:
                        dadosRetornoConsulta = _lancamentoCobrancaAplicacao.BuscarPagamentosEmAbertoRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro)?.ToList();
                        switch (tipoServico)
                        {
                            case TipoServico.Mensalista:
                            case TipoServico.CartaoAcesso:
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEmAbertoContratoMensalista", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                break;
                            case TipoServico.Convenio:
                                QuebraPaginaPorUnidade = false;
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEmAbertoPedidoSelo", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                break;
                            case TipoServico.Locacao:
                                QuebraPaginaPorUnidade = false;
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEmAbertoPedidoLocacao", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                break;
                            case TipoServico.Avulso:
                                throw new BusinessRuleException("Relatório solicitado não está disponível!");
                            case TipoServico.Evento:
                            case TipoServico.SeguroReembolso:
                            case TipoServico.Outros:
                            default:
                                QuebraPaginaPorUnidade = false;
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEmAberto", new KeyValuePair<TipoServico, List<DadosPagamentoVO>>(tipoServico, (List<DadosPagamentoVO>)dadosRetornoConsulta) );
                                break;
                        }
                        break;
                    case TipoRelatorioFinanceiro.PagamentosEfetuados:
                        dadosRetornoConsulta = _lancamentoCobrancaAplicacao.BuscarPagamentosPagosRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro)?.ToList();
                        switch (tipoServico)
                        {
                            case TipoServico.Mensalista:
                            case TipoServico.CartaoAcesso:
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuadosContratoMensalista", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                break;
                            case TipoServico.Convenio:
                                QuebraPaginaPorUnidade = false;
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuadosPedidoSelo", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                break;
                            case TipoServico.Locacao:
                                QuebraPaginaPorUnidade = false;
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuadosPedidoLocacao", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                break;
                            case TipoServico.Avulso:
                                throw new BusinessRuleException("Relatório solicitado não está disponível!");
                            case TipoServico.Evento:
                            case TipoServico.SeguroReembolso:
                            case TipoServico.Outros:
                            default:
                                QuebraPaginaPorUnidade = false;
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuados", new KeyValuePair<TipoServico, List<DadosPagamentoVO>>(tipoServico, (List<DadosPagamentoVO>)dadosRetornoConsulta));
                                break;
                        }
                        break;
                    case TipoRelatorioFinanceiro.PagamentosEmAbertoEfetuados:
                        var listaDadosPagamentos = _lancamentoCobrancaAplicacao.BuscarPagamentosEmAbertoRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro, true)?.ToList();
                        listaDadosPagamentos.AddRange(_lancamentoCobrancaAplicacao.BuscarPagamentosPagosRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro)?.ToList());
                        dadosRetornoConsulta = listaDadosPagamentos;
                        switch (tipoServico)
                        {
                            case TipoServico.Mensalista:
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosGeralContratoMensalista", new KeyValuePair<TipoServico, List<DadosPagamentoVO>>(tipoServico, (List<DadosPagamentoVO>)dadosRetornoConsulta));
                                break;
                            case TipoServico.Convenio:
                                //dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuadosPedidoSelo", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                //break;
                                throw new BusinessRuleException($"Tipo de Relatório Não Implementado! Tipo: [{tipoRelatorio.ToDescription()}]");
                            case TipoServico.Locacao:
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosGeralPedidoLocacao", new KeyValuePair<TipoServico, List<DadosPagamentoVO>>(tipoServico, (List<DadosPagamentoVO>)dadosRetornoConsulta));
                                break;
                            case TipoServico.Evento:
                            case TipoServico.Avulso:
                            case TipoServico.SeguroReembolso:
                            case TipoServico.Outros:
                            case TipoServico.CartaoAcesso:
                            default:
                                //dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuados", new KeyValuePair<TipoServico, List<DadosPagamentoVO>>(tipoServico, (List<DadosPagamentoVO>)dadosRetornoConsulta));
                                //break;
                                throw new BusinessRuleException($"Tipo de Relatório Não Implementado! Tipo: [{tipoRelatorio.ToDescription()}]");
                        }
                        break;
                    case TipoRelatorioFinanceiro.LancamentosPagosDivergentes:
                        dadosRetornoConsulta = _lancamentoCobrancaAplicacao.BuscarPagamentosPagosRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro)?.ToList();

                        switch (tipoServico)
                        {
                            case TipoServico.Mensalista:
                            case TipoServico.Convenio:
                            case TipoServico.Locacao:
                            case TipoServico.Evento:
                            case TipoServico.Avulso:
                            case TipoServico.SeguroReembolso:
                            case TipoServico.Outros:
                            case TipoServico.CartaoAcesso:
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosDivergencias", new KeyValuePair<TipoServico, List<DadosPagamentoVO>>(tipoServico, (List<DadosPagamentoVO>)dadosRetornoConsulta));
                                break;
                            default:
                                throw new BusinessRuleException($"Tipo de Relatório Não Implementado! Tipo: [{tipoRelatorio.ToDescription()}]");
                        }
                        break;
                    case TipoRelatorioFinanceiro.LancamentosClientes:
                        var listaDados = _lancamentoCobrancaAplicacao.BuscarPagamentosEmAbertoRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro)?.ToList();
                        listaDados.AddRange(_lancamentoCobrancaAplicacao.BuscarPagamentosPagosRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro)?.ToList());
                        dadosRetornoConsulta = listaDados;
                        switch (tipoServico)
                        {
                            case TipoServico.Mensalista:
                            case TipoServico.Convenio:
                            case TipoServico.Locacao:
                            case TipoServico.Evento:
                            case TipoServico.Avulso:
                            case TipoServico.SeguroReembolso:
                            case TipoServico.Outros:
                            case TipoServico.CartaoAcesso:
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_LancamentosCliente", new KeyValuePair<TipoServico, List<DadosPagamentoVO>>(tipoServico, (List<DadosPagamentoVO>)dadosRetornoConsulta));
                                break;
                            default:
                                throw new BusinessRuleException($"Tipo de Relatório Não Implementado! Tipo: [{tipoRelatorio.ToDescription()}]");
                        }
                        break;
                    case TipoRelatorioFinanceiro.PagamentosEfetuadosConferencia:
                        dadosRetornoConsulta = _lancamentoCobrancaAplicacao.BuscarPagamentosEfetuadosConferenciaRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro)?.ToList();
                        switch (tipoServico)
                        {
                            case TipoServico.Mensalista:
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuadosConferenciaContratoMensalista", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                break;
                            case TipoServico.Convenio:
                                QuebraPaginaPorUnidade = false;
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuadosConferenciaPedidoSelo", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                break;
                            case TipoServico.Locacao:
                                QuebraPaginaPorUnidade = false;
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuadosConferenciaPedidoLocacao", (List<DadosPagamentoVO>)dadosRetornoConsulta);
                                break;
                            case TipoServico.CartaoAcesso:
                            case TipoServico.Avulso:
                            case TipoServico.Evento:
                            case TipoServico.SeguroReembolso:
                            case TipoServico.Outros:
                            default:
                                QuebraPaginaPorUnidade = false;
                                dados = RazorHelper.RenderRazorViewToString(ControllerContext, "_PagamentosEfetuadosConferencia", new KeyValuePair<TipoServico, List<DadosPagamentoVO>>(tipoServico, (List<DadosPagamentoVO>)dadosRetornoConsulta));
                                break;
                        }
                        break;
                    default:
                        throw new BusinessRuleException($"Tipo de Relatório Não Implementado! Tipo: [{tipoRelatorio.ToDescription()}]");
                }
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao gerar o relatório: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            
            return new JsonResult
            {
                Data = new
                {
                    Dados = dados,
                },
                ContentType = "application/json",
                MaxJsonLength = int.MaxValue
            };
        }
        
        [HttpPost]
        [CheckSessionOut]
        public JsonResult RelatorioDownload(string dataInicio, string dataFim, TipoRelatorioFinanceiro tipoRelatorio, TipoServico tipoServico, string unidade)
        {
            var unidadeFiltro = string.IsNullOrEmpty(unidade) ? 0 : Convert.ToInt32(unidade);
            var workbook = new ExcelPackage();
            workbook.Workbook.Worksheets.Add("Relatorio");
            workbook.Workbook.Worksheets.MoveToStart("Relatorio");

            var ws = workbook.Workbook.Worksheets[1];
            ws.Name = "Relatorio";
            var linha = 2;

            switch (tipoRelatorio)
            {
                case TipoRelatorioFinanceiro.PagamentosEfetuadosExcel:
                //case TipoRelatorioFinanceiro.LancamentosCobrancaEmAberto:
                    ws.Cells["A1"].Value = "Unidade";
                    ws.Cells["B1"].Value = "Tipo Serviço";
                    ws.Cells["C1"].Value = "Quantidade Total";
                    ws.Cells["D1"].Value = "Valor Total";
                    ws.Cells["E1"].Value = "Cliente";

                    var dadosRetornoConsulta = tipoRelatorio == TipoRelatorioFinanceiro.PagamentosEfetuadosExcel 
                                                ? _lancamentoCobrancaAplicacao.BuscarLancamentosPagosRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro)
                                                : _lancamentoCobrancaAplicacao.BuscarLancamentosEmAbertoRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), tipoServico, unidadeFiltro);
                    foreach (var item in dadosRetornoConsulta)
                    {
                        ws.Cells[$"A{linha}"].Value = item.Unidade;
                        ws.Cells[$"B{linha}"].Value = item.TipoServico;
                        ws.Cells[$"C{linha}"].Value = item.QuantidadeTotal;
                        ws.Cells[$"D{linha}"].Value = item.ValorTotal;
                        ws.Cells[$"E{linha}"].Value = item.Cliente;

                        linha++;
                    }

                    break;
                case TipoRelatorioFinanceiro.SelosPagos:
                //case TipoRelatorioFinanceiro.SelosEmAberto:
                    ws.Cells["A1"].Value = "Unidade";
                    ws.Cells["B1"].Value = "Convênio";
                    ws.Cells["C1"].Value = "Cliente";
                    ws.Cells["D1"].Value = "Data Pagamento";
                    ws.Cells["E1"].Value = "Quantidade Selos";
                    ws.Cells["F1"].Value = "Período";
                    ws.Cells["G1"].Value = "Valor Pago";

                    var dadosConsulta = tipoRelatorio == TipoRelatorioFinanceiro.SelosPagos
                                            ? _seloAplicacao.BuscarSelosPagosRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), unidadeFiltro)
                                            : _seloAplicacao.BuscarSelosEmAbertoRelatorio(DateTime.Parse(dataInicio), DateTime.Parse(dataFim), unidadeFiltro);
                    foreach (var item in dadosConsulta)
                    {
                        ws.Cells[$"A{linha}"].Value = item.Unidade;
                        ws.Cells[$"B{linha}"].Value = item.Convenio;
                        ws.Cells[$"C{linha}"].Value = item.Cliente;
                        ws.Cells[$"D{linha}"].Value = item.DataPagamento <= System.Data.SqlTypes.SqlDateTime.MinValue.Value ? string.Empty : item.DataPagamento.ToString("dd/MM/yyyy");
                        ws.Cells[$"E{linha}"].Value = item.QuantidadeSelos;
                        ws.Cells[$"F{linha}"].Value = item.Periodo;
                        ws.Cells[$"G{linha}"].Value = item.ValorPago;

                        linha++;
                    }
                    break;
                default:
                    throw new BusinessRuleException($"Tipo de Relatório Não Implementado! Tipo: [{tipoRelatorio.ToDescription()}]");
            }

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = $"Relatorio_{tipoRelatorio.ToDescription()}_periodo_{dataInicio.Replace("/", "")}_a_{dataFim.Replace("/", "")}.xlsx" }
            };
        }

        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, $"application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
    }
}