using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Portal.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Portal.Controllers
{
	public class PrecoParametroSeloController : GenericController<PrecoParametroSelo>
	{

		public List<PrecoParametroSelo> ListaPrecoParametroSelo => _precoParametroSeloAplicacao?.Buscar()?.OrderBy(x => x.Unidade.Nome).ToList() ?? new List<PrecoParametroSelo>();

		private readonly IUnidadeAplicacao _unidadeAplicacao;
		private readonly IPerfilAplicacao _perfilAplicacao;
		private readonly IPrecoParametroSeloAplicacao _precoParametroSeloAplicacao;
		private readonly ITipoSeloAplicacao _tipoSeloAplicacao;



		public PrecoParametroSeloController(IUnidadeAplicacao unidadeAplicacao
								, IPerfilAplicacao perfilAplicacao
								, IPrecoParametroSeloAplicacao precoParametroSeloAplicacao
								, ITipoSeloAplicacao tipoSeloAplicacao)
		{
			_unidadeAplicacao = unidadeAplicacao;
			_perfilAplicacao = perfilAplicacao;
			_precoParametroSeloAplicacao = precoParametroSeloAplicacao;
			_tipoSeloAplicacao = tipoSeloAplicacao;


			ViewBag.ListaTipoDesconto = new SelectList(
			   Enum.GetValues(typeof(TipoDesconto)).Cast<TipoDesconto>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
			   "Id",
			   "Descricao");

		}

		public List<TipoSeloViewModel> ListaTipoSelo => AutoMapper.Mapper.Map<List<TipoSelo>, List<TipoSeloViewModel>>(_tipoSeloAplicacao.Buscar().Where(x => x.ParametroSelo != ParametroSelo.HoraInicial && x.ParametroSelo != ParametroSelo.HoraAdicional).ToList());

		// GET: Unidade
		public override ActionResult Index()
		{
			ModelState.Clear();

			PopularListasViewBag();
			return View("Index");

		}


		public ActionResult BuscarLista()
		{
			var precoParametroSelos = AutoMapper.Mapper.Map<List<PrecoParametroSelo>, List<PrecoParametroSeloViewModel>>(ListaPrecoParametroSelo);

			return PartialView("_GridPrecoParametroSelos", precoParametroSelos);
		}



		private void PopularListasViewBag()
		{
			ViewBag.ListaUnidade = _unidadeAplicacao.ListarOrdenadas();
			ViewBag.ListaPerfil = EntidadesServico.ListarPerfil(_perfilAplicacao);
			//ViewBag.ListaTabelaPreco = EntidadesServico.ListarPreco(_precoAplicacao);
		}

		// POST: Unidade/Create


		private bool ValidarDados(PrecoParametroSeloViewModel precoParametroSeloVM)
		{
			if (precoParametroSeloVM.Perfil == null) return false;
			if (precoParametroSeloVM.Unidade == null && !precoParametroSeloVM.TodasUnidades) return false;
			if (precoParametroSeloVM.DescontoMaximoValor == null) return false;
			if (precoParametroSeloVM.DescontoTabelaPreco == null) return false;
			if (precoParametroSeloVM.TipoPreco == null) return false;

			return true;
		}

		[CheckSessionOut]
		[HttpPost]
		public ActionResult SalvarDados(PrecoParametroSeloViewModel precoParametroSeloVM)
		{
			try
			{
				if (!ValidarDados(precoParametroSeloVM))
				{
					DadosModal = new DadosModal
					{
						Titulo = "Atenção",
						Mensagem = "Existem campos que devem ser preenchidos!",
						TipoModal = TipoModal.Danger
					};

					return View("Index");
				}

				if (precoParametroSeloVM.Id == 0 && !precoParametroSeloVM.TodasUnidades)
				{
					var retorno = _precoParametroSeloAplicacao.BuscarPor(x => x.Unidade.Id == precoParametroSeloVM.Unidade.Id && x.TipoPreco.Id == precoParametroSeloVM.TipoPreco.Id);

					if (retorno.Any())
					{
						DadosModal = new DadosModal
						{
							Titulo = "Atenção",
							Mensagem = "Já existe este Tipo Preço e Unidade parametrizados.",
							TipoModal = TipoModal.Danger
						};

						this.PopularListasViewBag();
						return View("Index");
					}
				}
				
				string descontoTabelaPreco = precoParametroSeloVM.DescontoTabelaPreco.Replace(".", "");
                string descontoCustoTabelaPreco = precoParametroSeloVM.DescontoCustoTabelaPreco.Replace(".", "");
                string descontoMaximoValor = precoParametroSeloVM.DescontoMaximoValor.Replace(".", "");

				var unidades = new List<int>();

				if (precoParametroSeloVM.TodasUnidades)
				{
					unidades = _unidadeAplicacao.Buscar().Select(u => u.Id).ToList();
				}
				else
				{
					unidades.Add(precoParametroSeloVM.Unidade.Id);
				}

				foreach (var unidadeId in unidades)
				{
					var precoParametroSeloDM = new PrecoParametroSelo();

					precoParametroSeloDM.Id = precoParametroSeloVM.Id;
					precoParametroSeloDM.DataInsercao = DateTime.Now;
					precoParametroSeloDM.DescontoTabelaPreco = Convert.ToDecimal(descontoTabelaPreco);
                    precoParametroSeloDM.DescontoCustoTabelaPreco = Convert.ToDecimal(descontoCustoTabelaPreco);
                    precoParametroSeloDM.DescontoMaximoValor = Convert.ToDecimal(descontoMaximoValor);
					precoParametroSeloDM.TipoPreco = precoParametroSeloVM.TipoPreco;
					precoParametroSeloDM.Unidade = new Unidade { Id = unidadeId };
					precoParametroSeloDM.Perfil = _perfilAplicacao.BuscarPorId(precoParametroSeloVM.Perfil.Id);

					_precoParametroSeloAplicacao.Salvar(precoParametroSeloDM);
				}

				DadosModal = new DadosModal
				{
					Titulo = "Sucesso",
					Mensagem = "Registro salvo com sucesso",
					TipoModal = TipoModal.Success
				};

			}
			catch (Exception ex)
			{
				DadosModal = new DadosModal
				{
					Titulo = "Atenção",
					Mensagem = "Ocorreu um erro ao salvar: " + ex.Message,
					TipoModal = TipoModal.Danger
				};

			}
			this.PopularListasViewBag();
			return View("Index");

		}


		// GET: Unidade/Edita/id
		public override ActionResult Edit(int id)
		{
			if (id > 0)
			{
				var precoParametroSeloDM = _precoParametroSeloAplicacao.BuscarPorId(id);

				var precoParametroSeloVM = AutoMapper.Mapper.Map<PrecoParametroSelo, PrecoParametroSeloViewModel>(precoParametroSeloDM);

				precoParametroSeloVM.Id = id;
				precoParametroSeloVM.DescontoMaximoValor = Convert.ToString(Math.Round(Convert.ToDecimal(precoParametroSeloVM.DescontoMaximoValor), 2));
				precoParametroSeloVM.DescontoTabelaPreco = Convert.ToString(Math.Round(Convert.ToDecimal(precoParametroSeloVM.DescontoTabelaPreco), 2));
                precoParametroSeloVM.DescontoCustoTabelaPreco = Convert.ToString(Math.Round(Convert.ToDecimal(precoParametroSeloVM.DescontoCustoTabelaPreco), 2));

                this.PopularListasViewBag();

				return View("Index", precoParametroSeloVM);

			}

			return View("Index");
		}


		[CheckSessionOut]
		public override ActionResult Delete(int id)
		{
			try
			{
				GerarDadosModal("Remover registro", "Deseja remover este registro?", TipoModal.Danger, "ConfirmarDelete",
					"Sim, Desejo remover!", id);
			}
			catch (BusinessRuleException br)
			{
				GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
			}
			catch (Exception ex)
			{
				GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
			}

			this.PopularListasViewBag();
			return View("Index");
		}

		[CheckSessionOut]
		public override ActionResult ConfirmarDelete(int id)
		{
			Dictionary<string, object> jsonResult = new Dictionary<string, object>();
			try
			{
				_precoParametroSeloAplicacao.ExcluirPorId(id);

				ModelState.Clear();
				GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);
			}
			catch (BusinessRuleException br)
			{
				GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
				jsonResult.Add("Status", "Error");
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				var tipoModal = TipoModal.Danger;

				if (ex.Message.IndexOf("DELETE") > 0)
				{
					message = "O preço parâmetro de selo esta sendo usado em outras associações de tabelas do sistema";
					tipoModal = TipoModal.Warning;
				}

				GerarDadosModal("Atenção", message, tipoModal);

				jsonResult.Add("Status", "Error");
			}

			this.PopularListasViewBag();
			return View("Index");
		}





	}
}
