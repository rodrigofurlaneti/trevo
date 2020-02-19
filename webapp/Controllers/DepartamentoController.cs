using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Entidade;
using Newtonsoft.Json;

namespace Portal.Controllers
{
	public class DepartamentoController : GenericController<Departamento>
	{
		private readonly IFuncionarioAplicacao _funcionarioAplicacao;
		private readonly IMenuAplicacao _menuAplicacao;
		private readonly IPessoaAplicacao _pessoaAplicacao;

		public DepartamentoController(IDepartamentoAplicacao marcaAplicacao,
			IFuncionarioAplicacao funcionarioAplicacao,
			IPessoaAplicacao pessoaAplicacao,
			IMenuAplicacao menuAplicacao
		)
		{
			Aplicacao = marcaAplicacao;
			_funcionarioAplicacao = funcionarioAplicacao;
			_pessoaAplicacao = pessoaAplicacao;
			_menuAplicacao = menuAplicacao;
		}

		public List<DepartamentoViewModel> ListaDepartamentos { get; set; }
		public List<FuncionarioViewModel> ListaFuncionario => _funcionarioAplicacao.Buscar().Select(x => new FuncionarioViewModel(x)).ToList();

        public List<MenuViewModel> ListaMenusDepartamentos
		{
			get => (List<MenuViewModel>) Session["ListaMenusDepartamentos"] ?? new List<MenuViewModel>();
			set => Session["ListaMenusDepartamentos"] = value;
		}

		[CheckSessionOut]
		public override ActionResult Index()
		{
			ModelState.Clear();

			ListaMenusDepartamentos = _menuAplicacao.Buscar().Where(x => x.MenuPai == null).Select(x => new MenuViewModel(x)).OrderBy(x => x.Descricao).ToList();
			return View("Index");
		}

		[CheckSessionOut]
		[HttpPost]
		public ActionResult SalvarDados(DepartamentoViewModel model, List<int> responsaveisIds)
		{
			try
			{
				var departamento = Mapper.Map<Departamento>(model);

                departamento.AddResponsaveis(responsaveisIds);

				Aplicacao.Salvar(departamento);

				ModelState.Clear();
				Session["ImgAvatar"] = null;

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
            var departamento = Aplicacao.BuscarPorId(id);
            var departamentoViewModel = Mapper.Map<DepartamentoViewModel>(departamento);

            ViewBag.ResponsaveisSelecionados = departamentoViewModel.DepartamentoResponsaveis.Select(x => x.Funcionario.Id).ToList();

			return View("Index", departamentoViewModel);
		}

		public ActionResult BuscarDepartamentos()
		{
			var ListaDepartamentosDM = Aplicacao?.Buscar()?.ToList() ?? new List<Departamento>();

			ListaDepartamentos = Mapper.Map<List<DepartamentoViewModel>>(ListaDepartamentosDM);

			return PartialView("_GridDepartamentos", ListaDepartamentos);
		}
	}
}