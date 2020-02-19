using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Portal.Helpers;
using Portal.Models;

namespace Portal.Controllers
{
	public class LeituraCNABController : GenericController<LeituraCNAB>
	{
		private const string NomeContaFinanceira = "ITAU";
		private readonly IContaFinanceiraAplicacao _contaFinanceiraAplicacao;
		private readonly ILeituraCNABAplicacao _leituraCnabAplicacao;

        public List<ContaFinanceira> ListaContaFinanceira => _contaFinanceiraAplicacao.Buscar().ToList();

        public LeituraCNABController(ILeituraCNABAplicacao leituraCnabAplicacao, IContaFinanceiraAplicacao contaFinanceiraAplicacao)
		{
			Aplicacao = leituraCnabAplicacao;
			_leituraCnabAplicacao = leituraCnabAplicacao;
			_contaFinanceiraAplicacao = contaFinanceiraAplicacao;
		}

		[CheckSessionOut]
		public override ActionResult Index()
		{
			ModelState.Clear();

			return View("Index");
		}

		[CheckSessionOut]
		public ActionResult BuscarCNAB(LeituraCNABFiltroViewModel filtro)
		{
			var cnabs = _leituraCnabAplicacao.BuscarPor(x =>
				x.DataInsercao >= filtro.DataDe.AddHours(0).AddMinutes(0).AddSeconds(0) &&
				x.DataInsercao <= filtro.DataAte.AddHours(23).AddMinutes(59).AddSeconds(59)
			);

			TempData["CNAB"] = cnabs;
			var model = Mapper.Map<List<LeituraCNABViewModel>>(cnabs);

			return PartialView("_GridLeituraCNAB", model);
		}

		[CheckSessionOut]
		public ActionResult DetalhesCNAB(int idLeituraCNAB)
		{
			var listaLeituraCnab = (List<LeituraCNAB>) TempData["CNAB"];
			var model = new List<LeituraCNABDetalhesViewModel>();
			var leituraCNAB = listaLeituraCnab.FirstOrDefault(x => x.Id == idLeituraCNAB);

			if (leituraCNAB != null && leituraCNAB.ListaLancamentos != null && leituraCNAB.ListaLancamentos.Any())
				model = leituraCNAB.ListaLancamentos.Select(x => new LeituraCNABDetalhesViewModel
					{
						CpfCnpj = !string.IsNullOrEmpty(x.LancamentoCobranca.Cliente.Pessoa.DocumentoCpf)
							? x.LancamentoCobranca.Cliente.Pessoa.DocumentoCpf
							: x.LancamentoCobranca.Cliente.Pessoa.DocumentoCnpj,
						NomeCliente = x.LancamentoCobranca.Cliente.Pessoa.Nome,
						NumeroDocumento = x.LancamentoCobranca.Id.ToString(),
						Status = x.LancamentoCobranca.StatusLancamentoCobranca,
						Valor = x.LancamentoCobranca.ValorContrato.ToString()
					})
					.ToList();

			TempData.Keep("CNAB");

			return PartialView("_DetalhesCNAB", model);
		}

		[HttpPost]
		[CheckSessionOut]
		public JsonResult UploadLeituraCNAB(string fileBase64, string fileName, int contaFinanceiraId, bool previa)
		{
			try
			{
				var fileBytes = Convert.FromBase64String(fileBase64.Substring(fileBase64.IndexOf("base64,") + "base64,".Length));

				if (string.IsNullOrEmpty(fileBase64) || string.IsNullOrEmpty(fileName))
					throw new BusinessRuleException("Não identificado arquivo para efetuar upload!");

				var contaFinanceira = _contaFinanceiraAplicacao.BuscarPorId(contaFinanceiraId);

				if (contaFinanceira == null)
                    throw new BusinessRuleException("Informe uma conta financeira");

				var dadosRetorno = ArquivoRetorno.LeituraCNAB400(fileBytes, contaFinanceira);
                dadosRetorno.Key.Arquivo = fileBytes;
                dadosRetorno.Key.ContaFinanceira = contaFinanceira;
                dadosRetorno.Key.NomeArquivo = fileName;
				var retornoLeituraCnab = _leituraCnabAplicacao.SalvarLeituraCNAB(dadosRetorno.Key, dadosRetorno.Value, UsuarioLogado.UsuarioId, previa);

                var modal = RazorHelper.RenderRazorViewToString(ControllerContext, "_ModalPreviaLeituraCnab", new KeyValuePair<string, List<LeituraCNABPreviaViewModel>>( fileName, retornoLeituraCnab ));

                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue,
                    Data = new
                    {
                        ModalPreviaLeitura = modal
                    }
                };
				//return Json(new Resultado<object>
				//{
				//	Sucesso = false,
				//	TipoModal = TipoModal.Success.ToDescription(),
				//	Titulo = "Upload CNAB",
				//	Mensagem = "Atualizado as Informações dos Lançamentos pelo Arquivo de CNAB."
				//}, JsonRequestBehavior.AllowGet);
			}
			catch (BusinessRuleException ex)
			{
				return Json(new Resultado<object>
				{
					Sucesso = false,
					TipoModal = TipoModal.Warning.ToDescription(),
					Titulo = "Upload CNAB",
					Mensagem = ex.Message
				}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new Resultado<object>
				{
					Sucesso = false,
					TipoModal = TipoModal.Danger.ToDescription(),
					Titulo = "Upload CNAB",
					Mensagem = "Ocorreu um erro ao pesquisar: " + ex.Message
				}, JsonRequestBehavior.AllowGet);
			}
		}
	}
}