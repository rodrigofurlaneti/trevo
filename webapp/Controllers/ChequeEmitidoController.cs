using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Portal.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ChequeEmitidoController : GenericController<ChequeEmitido>
    {
        public List<ChequeEmitido> ListaChequeEmitidos => Aplicacao?.Buscar()?.ToList() ?? new List<ChequeEmitido>();

        public List<Fornecedor> ListaFornecedor => _fornecedorAplicacao.Buscar().ToList();

        public List<Banco> ListaBanco => _bancoAplicacao.Buscar().ToList();

        public IList<ChaveValorViewModel> ListaStatusCheque => _chequeEmitidoAplicacao.ListaStatusCheque();

        private readonly IFornecedorAplicacao _fornecedorAplicacao;
        private readonly IBancoAplicacao _bancoAplicacao;
        private readonly IChequeEmitidoAplicacao _chequeEmitidoAplicacao;
        private readonly IChequeEmitidoContaPagarAplicacao _chequeEmitidoContaPagarAplicacao;

        public ChequeEmitidoController(IChequeEmitidoAplicacao chequeEmitidoAplicacao,
                                         IFornecedorAplicacao FornecedorAplicacao,
                                         IBancoAplicacao bancoAplicacao,
                                         IChequeEmitidoContaPagarAplicacao chequeEmitidoContaPagarAplicacao)
        {
            Aplicacao = chequeEmitidoAplicacao;
            _chequeEmitidoAplicacao = chequeEmitidoAplicacao;
            _fornecedorAplicacao = FornecedorAplicacao;
            _bancoAplicacao = bancoAplicacao;
            _chequeEmitidoContaPagarAplicacao = chequeEmitidoContaPagarAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();

            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ChequeEmitidoViewModel model)
        {
            try
            {
                var lancamentosSalvar = (List<ContasAPagarViewModel>)TempData["LancamentosFornecedorSalvar"];

                if(lancamentosSalvar == null || !lancamentosSalvar.Any())
                {
                    throw new BusinessRuleException("É necessário que tenha ao menos uma conta pagar na lista.");
                }

                model.ListaContaPagar = lancamentosSalvar;
                model.Valor = Convert.ToDecimal(model.ValorFormatado);

                var entidade = model.ToEntity();

                entidade.ListaContaPagar = new List<ChequeEmitidoContaPagar>();

                foreach (var item in lancamentosSalvar)
                {
                    entidade.ListaContaPagar.Add(new ChequeEmitidoContaPagar { ContaPagar = item.ToEntity() });
                }

                foreach (var item in entidade.ListaContaPagar)
                {
                    item.ChequeEmitido = entidade;
                }

                _chequeEmitidoAplicacao.Salvar(entidade);

                ModelState.Clear();

                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro(ex.Message);
                return View("Index", model);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var chequeDm = Aplicacao.BuscarPorId(id);

            var cheque = new ChequeEmitidoViewModel(chequeDm);

            TempData["LancamentosFornecedorSalvar"] = cheque.ListaContaPagar;

            TempData.Keep();

            cheque.ValorFormatado = chequeDm.Valor.ToString();

            return View("Index", cheque);
        }

        public ActionResult BuscarChequesEmitidos()
        {
            var cheques = Aplicacao.Buscar().Select(x => new ChequeEmitidoViewModel(x)).ToList();

            foreach (var cheque in cheques)
            {
                if (cheque.Fornecedor.Nome == null)
                {
                    cheque.Fornecedor.Nome = cheque.Fornecedor.NomeFantasia;
                }
            }

            return PartialView("_GridChequeEmitido", cheques);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarLancamento(int idFornecedor, bool limpaLancamentoSalvar)
        {
            if(limpaLancamentoSalvar)
                TempData["LancamentosFornecedorSalvar"] = null;

            var contasPagar = _chequeEmitidoAplicacao.BuscarContasPagarPorFornecedor(idFornecedor).Where(x => x.FormaPagamento == FormaPagamento.Cheque).ToList();

            var contasPagarId = _chequeEmitidoAplicacao.Buscar().SelectMany(x => x.ListaContaPagar).Select(x => x.ContaPagar.Id).ToList();
            contasPagar = contasPagar.Where(x => !contasPagarId.Contains(x.Id)).ToList();

            TempData["LancamentosFornecedor"] = contasPagar;

            var listaRetorno = new List<ChaveValorViewModel>();

            if(contasPagar.Any())
            {
                listaRetorno.AddRange(contasPagar.Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = $"{x.Id} - {x.ValorFormatado} - {x.DataVencimento.ToShortDateString()}"
                }).ToList());
            }

            TempData.Keep();

            return Json(listaRetorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdicionarLancamento(int idLancamento)
        {
            
            var lancamentosVM = (List<ContasAPagarViewModel>)TempData["LancamentosFornecedor"];
            var lancamentosSalvarVM = (List<ContasAPagarViewModel>)TempData["LancamentosFornecedorSalvar"];

            if (lancamentosSalvarVM == null)
                lancamentosSalvarVM = new List<ContasAPagarViewModel>();

            if (lancamentosSalvarVM.Any(x => x.Id == idLancamento))
            {
                TempData.Keep();
                return Json(new
                {
                    Sucesso = true,
                    Mensagem = "Conta pagar já foi adicionado"
                }, JsonRequestBehavior.AllowGet);
            }

            lancamentosSalvarVM.Add(lancamentosVM.FirstOrDefault(x => x.Id == idLancamento));

            TempData["LancamentosFornecedorSalvar"] = lancamentosSalvarVM;

            TempData.Keep();

            return PartialView("_GridContaPagar", lancamentosSalvarVM);
        }

        public ActionResult RemoverLancamento(int idLancamento)
        {
            var lancamentoVM = (List<ContasAPagarViewModel>)TempData["LancamentosFornecedorSalvar"];
            var lancamento = lancamentoVM.Find(x => x.Id == idLancamento);
            lancamentoVM.Remove(lancamento);
            TempData["LancamentosFornecedorSalvar"] = lancamentoVM;

            TempData.Keep();

            return PartialView("_GridContaPagar", lancamentoVM);
        }
    }
}
