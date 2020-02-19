using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Entidade.Uteis;

namespace Portal.Controllers
{
    public class PropostaController : GenericController<Proposta>
    {
        public IList<ClienteViewModel> ListaClientes
        {
            get => (List<ClienteViewModel>)TempData["ListaClientes"] ?? new List<ClienteViewModel>();
	        set => TempData["ListaClientes"] = value;
        }

        public IList<UnidadeViewModel> ListaFiliais
        {
            get => (List<UnidadeViewModel>)TempData["ListaFiliais"] ?? new List<UnidadeViewModel>();
	        set => TempData["ListaFiliais"] = value;
        }
        
        public IList<string> ListaEmails
        {
            get => (List<string>)TempData["ListaEmails"] ?? new List<string>();
	        set => TempData["ListaEmails"] = value;
        }

        public IList<PropostaGridViewModel> ListaPropostas
        {
            get => (List<PropostaGridViewModel>)TempData["ListaPropostas"] ?? new List<PropostaGridViewModel>();
	        set => TempData["ListaPropostas"] = value;
        }

        private readonly IPropostaAplicacao _propostaAplicacao;
        private readonly IPedidoSeloAplicacao _pedidoSeloAplicacao;
	    private readonly IClienteAplicacao _clienteAplicacao;

	    public PropostaController(
            IPropostaAplicacao propostaAplicacao,
            IPedidoSeloAplicacao pedidoSeloAplicacao,
            IClienteAplicacao clienteAplicacao)
        {
            Aplicacao = propostaAplicacao;
            _propostaAplicacao = propostaAplicacao;
            _pedidoSeloAplicacao = pedidoSeloAplicacao;
	        _clienteAplicacao = clienteAplicacao;
        }

        [HttpGet]
        [CheckSessionOut]
        public ActionResult GerarPdf()
        {
            var idPedido = 36;
            var pedido = _pedidoSeloAplicacao.BuscarPorId(idPedido);
            var pdf = _propostaAplicacao.GerarPdfByte(pedido, ControllerContext);
            return File(pdf, "application/pdf", "teste.pdf");
        }

        [HttpGet]
        [CheckSessionOut]
        public override ActionResult Index()
        {
            PrepararTela();
            return View("Index", new PropostaViewModel());
        }

        [HttpGet]
        [CheckSessionOut]
        public override ActionResult Edit(int id)
        {
            var viewModel = _propostaAplicacao.PrepararViewModelEdicao(id);
            PrepararTela(viewModel);
            return View("Index", viewModel);
        }

        [HttpPost]
        [CheckSessionOut]
        public ActionResult SalvarDados(PropostaViewModel viewModel)
        {
            TempData.Keep();
            try
            {
                _propostaAplicacao.Salvar(viewModel);
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
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", viewModel);
            }

            return View("Index");
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarTelefone(int idEmpresa)
        {
            TempData.Keep();
            var telefone = _propostaAplicacao.BuscarTelefoneHelper(idEmpresa);
            return Json(telefone, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarEndereco(int idFilial)
        {
            TempData.Keep();
            var endereco = _propostaAplicacao.BuscarEnderecoHelper(idFilial);
            return Json(endereco, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarHorarioFuncionamento(int idFilial)
        {
            TempData.Keep();
            var horarioFuncionamento = _propostaAplicacao.BuscarHorarioFuncionamentoHelper(idFilial);
            return Json(horarioFuncionamento, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarEmail(int idEmpresa)
        {
            TempData.Keep();
            var emails = _propostaAplicacao.BuscarEmailsHelper(idEmpresa);
            ListaEmails = emails;
            TempData.Keep();
            return Json(emails, JsonRequestBehavior.AllowGet);
        }

	    [HttpPost]
	    [CheckSessionOut]
	    public JsonResult BuscarCliente(string descricao)
	    {
		    var lista = _clienteAplicacao.BuscarPor(c => c.Pessoa.Nome.Contains(descricao) || c.NomeFantasia.Contains(descricao));
			
			return Json(lista.Select(c => new
			{
				c.Id, 
				Descricao = c.TipoPessoa == TipoPessoa.Fisica ? c.Pessoa.Nome : c.NomeFantasia
			}));
	    }

		private void PrepararTela()
        {
            TempData.Clear();
           // ListaClientes = _propostaAplicacao.ListaClientes();
            ListaFiliais = _propostaAplicacao.ListaUnidade();
            ListaPropostas = _propostaAplicacao.PopularGrid();
            TempData.Keep();
        }

        private void PrepararTela(PropostaViewModel viewModel)
        {
            TempData.Clear();
            //ListaClientes = _propostaAplicacao.ListaClientes();
            ListaFiliais = _propostaAplicacao.ListaUnidade();
            ListaEmails = _propostaAplicacao.BuscarEmailsHelper(viewModel.Empresa.Id);
            ListaPropostas =  _propostaAplicacao.PopularGrid();
            TempData.Keep();
        }
    }
}