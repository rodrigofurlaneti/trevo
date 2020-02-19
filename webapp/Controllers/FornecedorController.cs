using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;

namespace Portal.Controllers
{
    public class FornecedorController : GenericController<Fornecedor>
    {
        private readonly IFornecedorAplicacao _fornecedorAplicacao;
        private readonly IBancoAplicacao _bancoAplicacao;

        private ContatoController _contato => new ContatoController();
        public List<Fornecedor> ListaFornecedores => _fornecedorAplicacao?.Buscar()?.ToList() ?? new List<Fornecedor>();
        public List<Banco> ListaBanco => _bancoAplicacao.Buscar().ToList();

        public FornecedorController(IFornecedorAplicacao fornecedoreAplicacao, IBancoAplicacao bancoAplicacao)
        {
            Aplicacao = fornecedoreAplicacao;
            _fornecedorAplicacao = fornecedoreAplicacao;
            _bancoAplicacao = bancoAplicacao;

            ViewBag.TipoPessoaSelectList = new SelectList(
                Enum.GetValues(typeof(TipoPessoa)).Cast<TipoPessoa>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
                "Id",
                "Descricao");
        }

        public override ActionResult Index()
        {
            Session["contatos"] = new List<ContatoViewModel>();
            return base.Index();
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(FornecedorViewModel fornecedor, EnderecoViewModel endereco)
        {
            try
            {
                fornecedor.Enderecos.Clear();
                fornecedor.Enderecos.Add(endereco);
                fornecedor.Contatos = Session["contatos"] != null ? (List<ContatoViewModel>)Session["contatos"] : new List<ContatoViewModel>();
                _fornecedorAplicacao.Salvar(fornecedor.ViewModelToEntity());

                ModelState.Clear();
                Session["contatos"] = new List<ContatoViewModel>();

                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
                return View("Index", fornecedor);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro("Ocorreu um erro ao salvar: " + ex.Message);
                return View("Index", fornecedor);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var fornecedor = _fornecedorAplicacao.BuscarPorId(id);
            Session["contatos"] = ContatoViewModel.ContatoViewModelList(fornecedor.Contatos?.Select(x => x.Contato).ToList());

            ViewBag.TipoPessoaSelectList = new SelectList(
                Enum.GetValues(typeof(TipoPessoa)).Cast<TipoPessoa>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
                "Id",
                "Descricao",
                (int)fornecedor.TipoPessoa);

            return View("Index", new FornecedorViewModel(fornecedor));
        }

        public JsonResult BuscarContatos(int id)
        {
            var fornecedor = _fornecedorAplicacao.BuscarPorId(id);
            var contatos = ContatoViewModel.ContatoViewModelList(fornecedor.Contatos?.Select(x => x.Contato).ToList());
            Session["contatos"] = contatos;

            return Json(
                new { contatos }
            );
        }

        public JsonResult BuscarContatosEmSessao()
        {
            var contatos = Session["contatos"] != null ? (List<ContatoViewModel>)Session["contatos"] : new List<ContatoViewModel>();

            return Json(
                new { contatos }
            );
        }
    }
}