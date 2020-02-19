using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
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
    public class ChequeRecebidoController : GenericController<ChequeRecebido>
    {
        public List<ChequeRecebidoViewModel> ListaChequeRecebidos
        {
            get
            {
                return _chequeRecebidoAplicacao?.BuscarDadosSimples()?.ToList() ?? new List<ChequeRecebidoViewModel>();
            }
        }
        
        public List<BancoViewModel> ListaBanco
        {
            get
            {
                return Mapper.Map<List<Banco>, List<BancoViewModel>>(_bancoAplicacao.Buscar().OrderBy(x => x.Descricao).ToList());
            }
        }

        public IList<ChaveValorViewModel> ListaStatusCheque
        {
            get
            {
                return Enum.GetValues(typeof(StatusCheque)).Cast<StatusCheque>().Select(e => new ChaveValorViewModel
                {
                    Id = (int)e,
                    Descricao = e.ToDescription()
                }).ToList();
            }
        }

        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IBancoAplicacao _bancoAplicacao;
        private readonly IChequeRecebidoAplicacao _chequeRecebidoAplicacao;

        public ChequeRecebidoController(IChequeRecebidoAplicacao chequeRecebidoAplicacao,
                                         IClienteAplicacao clienteAplicacao,
                                         IBancoAplicacao bancoAplicacao)
        {
            Aplicacao = chequeRecebidoAplicacao;
            _chequeRecebidoAplicacao = chequeRecebidoAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _bancoAplicacao = bancoAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();

            TempData.Clear();

            return View("Index");
        }
        
        //public JsonResult Supervisores()
        //{
        //    return Json(ListaCliente);
        //}

        //[HttpPost]
        //public void SupervisorSelecionados(string json)
        //{
        //    ListaCliente = JsonConvert.DeserializeObject<List<ClienteViewModel>>(json);
        //}

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ChequeRecebidoViewModel model)
        {
            try
            {

                var lancamentosSalvar = (List<LancamentoCobrancaViewModel>)TempData["LancamentosClienteSalvar"];

                if (lancamentosSalvar == null || !lancamentosSalvar.Any())
                {
                    throw new BusinessRuleException("É necessário que tenha ao menos um lançamento de cobrança na lista.");
                }

                model.ListaLancamentoCobranca = lancamentosSalvar;
                model.Valor = Convert.ToDecimal(model.ValorFormatado);

                var entidade = Aplicacao.BuscarPorId(model.Id) ?? model.ToEntity();

                entidade = model.ToEntity();

                entidade.ListaLancamentoCobranca = new List<ChequeRecebidoLancamentoCobranca>();

                foreach (var item in lancamentosSalvar)
                {
                    entidade.ListaLancamentoCobranca.Add(new ChequeRecebidoLancamentoCobranca { LancamentoCobranca = item.ToEntity() });
                }


                foreach (var item in entidade.ListaLancamentoCobranca)
                {
                    item.ChequeRecebido = entidade;
                }


                Aplicacao.Salvar(entidade);

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Index", model);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", model);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var chequeDm = Aplicacao.BuscarPorId(id);
            var cheque = new ChequeRecebidoViewModel(chequeDm);

            TempData["LancamentosClienteSalvar"] = cheque.ListaLancamentoCobranca;

            TempData.Keep();

            cheque.ValorFormatado = chequeDm.Valor.ToString();

            return View("Index", cheque);
        }

        public ActionResult BuscarChequesRecebidos()
        {
            var cheques = _chequeRecebidoAplicacao.BuscarDadosSimples();

            foreach (var cheque in cheques)
            {
                if (cheque.Cliente.Pessoa.Nome == null)
                {
                    cheque.Cliente.Pessoa.Nome = cheque.Cliente.NomeFantasia;
                }
            }

            return PartialView("_GridChequeRecebido", cheques);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarLancamento(int idCliente, bool limpaLancamentoSalvar, int idCheque)
        {
            if (limpaLancamentoSalvar)
                TempData["LancamentosClienteSalvar"] = null;

            var lista = _chequeRecebidoAplicacao.BuscarLancamentosPorCliente(idCliente, idCheque).ToList();

            TempData["LancamentosCliente"] = lista;

            var listaRetorno = new List<ChaveValorViewModel>();

            if (lista.Any())
            {
                listaRetorno.AddRange(lista.Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = $"{x.Id} - {x.ValorContrato} - {x.DataVencimento.ToShortDateString()}"
                }).ToList());
            }

            TempData.Keep();

            return Json(listaRetorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdicionarLancamento(int idLancamento)
        {

            var lancamentosVM = (List<LancamentoCobrancaViewModel>)TempData["LancamentosCliente"];
            var lancamentosSalvarVM = (List<LancamentoCobrancaViewModel>)TempData["LancamentosClienteSalvar"];

            if (lancamentosSalvarVM == null)
                lancamentosSalvarVM = new List<LancamentoCobrancaViewModel>();

            if (lancamentosSalvarVM.Any(x => x.Id == idLancamento))
            {
                TempData.Keep();
                return Json(new
                {
                    Sucesso = true,
                    Mensagem = "Lançamento já foi adicionado"
                }, JsonRequestBehavior.AllowGet);
            }

            lancamentosSalvarVM.Add(lancamentosVM.FirstOrDefault(x => x.Id == idLancamento));

            TempData["LancamentosClienteSalvar"] = lancamentosSalvarVM;

            TempData.Keep();

            return PartialView("_GridLancamentos", lancamentosSalvarVM);
        }

        public ActionResult RemoverLancamento(int idLancamento)
        {
            var lancamentoVM = (List<LancamentoCobrancaViewModel>)TempData["LancamentosClienteSalvar"];
            var lancamento = lancamentoVM.Find(x => x.Id == idLancamento);
            lancamentoVM.Remove(lancamento);
            TempData["LancamentosClienteSalvar"] = lancamentoVM;

            TempData.Keep();

            return PartialView("_GridLancamentos", lancamentoVM);
        }
    }
}
