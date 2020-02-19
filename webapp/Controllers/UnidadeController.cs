using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Portal.Controllers
{
    public class UnidadeController : GenericController<Unidade>
    {

        public List<CheckListAtividadeViewModel> ListaCheckList => _checkListAtividadeAplicacao.BuscarAtivos();

        public List<UnidadeCheckListAtividadeTipoAtividadeViewModel> ListaUnidadeCheckListTipoAtividade
        {
            get { return (List<UnidadeCheckListAtividadeTipoAtividadeViewModel>)Session["ListaUnidadeCheckListAtividadeTipoAtividade"] ?? new List<UnidadeCheckListAtividadeTipoAtividadeViewModel>(); }
            set { Session["ListaUnidadeCheckListAtividadeTipoAtividade"] = value; }
        }

        public List<CheckListEstruturaUnidade> ListaEquipamentos
        {
            get { return (List<CheckListEstruturaUnidade>)Session["ListaEquipamentos"] ?? new List<CheckListEstruturaUnidade>(); }
            set { Session["ListaEquipamentos"] = value; }
        }

        public List<TipoUnidadeViewModel> ListaTipoUnidade => _tipoUnidadeAplicacao?

                                                              .Buscar()?.Select(x => new TipoUnidadeViewModel(x))?
                                                              .ToList() ?? new List<TipoUnidadeViewModel>();

        public List<FuncionarioViewModel> ListaSupervisor { get; set; }


        public List<ChaveValorViewModel> selectTipoUnidade = new List<ChaveValorViewModel>()
            {
               new ChaveValorViewModel{ Id = 1, Descricao = "Garagem" },
               new ChaveValorViewModel{ Id = 2, Descricao = "Prédio Comercial" },
               new ChaveValorViewModel{ Id = 3, Descricao = "Condominio" },
            };

        public List<TipoPagamentoViewModel> selectTipoPagamentosVM = new List<TipoPagamentoViewModel>()
            {
               new TipoPagamentoViewModel{ Codigo = "1", Descricao = "Cartão de Crédito" },
               new TipoPagamentoViewModel{ Codigo = "2", Descricao = "Cartão de Débito" },
               new TipoPagamentoViewModel{ Codigo = "3", Descricao = "Dinheiro" },
               new TipoPagamentoViewModel{ Codigo = "4", Descricao = "Cheque" },
               new TipoPagamentoViewModel{ Codigo = "5", Descricao = "TED" },
               new TipoPagamentoViewModel{ Codigo = "6", Descricao = "Depósito" },
               new TipoPagamentoViewModel{ Codigo = "7", Descricao = "Boleto" }
            };


        public List<SelectListItem> selectTipoPagamentos { get; set; }

        public List<EmpresaViewModel> ListaEmpresa => Mapper.Map<List<Empresa>, List<EmpresaViewModel>>(_empresaAplicacao.Buscar().ToList());

        public List<EstruturaGaragemViewModel> ListaEstruturaGaragem { get; set; }

        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ITipoUnidadeAplicacao _tipoUnidadeAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;
        private readonly ITipoPagamentoAplicacao _tipoPagamentoAplicacao;
        private readonly IPrecoAplicacao _tabelaPrecoAplicacao;
        private readonly IEstruturaGaragemAplicacao _estruturaGaragemAplicacao;
        private readonly ICheckListAtividadeAplicacao _checkListAtividadeAplicacao;
        private readonly IEstruturaUnidadeAplicacao _estruturaUnidadeAplicacao;
        private readonly IUnidadeCheckListAtividadeAplicacao _unidadeCheckListAtividadeAplicacao;
        private readonly ICidadeAplicacao _cidadeAplicacao;
        private readonly IEmpresaAplicacao _empresaAplicacao;

        public UnidadeController(IUnidadeAplicacao unidadeAplicacao
                                , ITipoUnidadeAplicacao tipoUnidadeAplicacao
                                , IFuncionarioAplicacao funcionarioAplicacao
                                , ITipoPagamentoAplicacao tipoPagamentoAplicacao
                                , IPrecoAplicacao tabelaPrecoAplicacao
                                , IEstruturaGaragemAplicacao estruturaGaragemAplicacao
                                , ICheckListAtividadeAplicacao checkListAtividadeAplicacao
                                , IEstruturaUnidadeAplicacao estruturaUnidadeAplicacao
                                , IUnidadeCheckListAtividadeAplicacao unidadeCheckListAtividadeAplicacao
                                , ICidadeAplicacao cidadeAplicacao
                                , IEmpresaAplicacao empresaAplicacao)
        {
            Aplicacao = unidadeAplicacao;
            this._unidadeAplicacao = unidadeAplicacao;
            this._tipoUnidadeAplicacao = tipoUnidadeAplicacao;
            this._funcionarioAplicacao = funcionarioAplicacao;
            this._tipoPagamentoAplicacao = tipoPagamentoAplicacao;
            this._tabelaPrecoAplicacao = tabelaPrecoAplicacao;
            this._estruturaUnidadeAplicacao = estruturaUnidadeAplicacao;
            this._unidadeCheckListAtividadeAplicacao = unidadeCheckListAtividadeAplicacao;
            this._estruturaGaragemAplicacao = estruturaGaragemAplicacao;
            this._checkListAtividadeAplicacao = checkListAtividadeAplicacao;
            this._cidadeAplicacao = cidadeAplicacao;
            this._empresaAplicacao = empresaAplicacao;

            selectTipoPagamentos = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Cartão de Crédito" },
                new SelectListItem { Value = "2", Text = "Cartão de Débito" },
                new SelectListItem { Value = "3", Text = "Dinheiro" },
                new SelectListItem { Value = "4", Text = "Cheque" },
                new SelectListItem { Value = "5", Text = "TED" },
                new SelectListItem { Value = "6", Text = "Depósito" },
                new SelectListItem { Value = "7", Text = "Boleto" }
            };
        }

        private void PopularDados()
        {
            ListaEstruturaGaragem = _estruturaGaragemAplicacao.BuscarAtivos();
            ListaSupervisor = _funcionarioAplicacao?.BuscarComDadosSimples()?
                                                    .Select(x => new FuncionarioViewModel(x))?
                                                    .ToList() ?? new List<FuncionarioViewModel>();
        }

        // GET: Unidade
        public override ActionResult Index()
        {
            ModelState.Clear();
            PopularDados();

            var unidade = new UnidadeViewModel { DiaVencimento = 1, NumeroVaga = 1 };

            unidade.EstruturasUnidade = Helpers.Unidade.ListaUnidadeEstruturaUnidade(this.ListaEstruturaGaragem);

            unidade.CheckListEstruturaUnidade = Helpers.Unidade.ListaCheckListEquipamentoUnidade(this.ListaEstruturaGaragem);

            ListaUnidadeCheckListTipoAtividade = new List<UnidadeCheckListAtividadeTipoAtividadeViewModel>();

            return View("Index", unidade);
        }

        // POST: Unidade/Create
        [CheckSessionOut]
        [HttpPost]
        public ActionResult Salva(UnidadeViewModel unidade, EnderecoViewModel endereco,
                                    List<EstruturaUnidadeViewModel> estruturasUnidade, List<UnidadeCheckListAtividadeViewModel> unidadesCheckLis)
        {
            try
            {
                var tiposPagamentoSelecionados = new List<TipoPagamentoViewModel>();

                for (int i = 0; i < unidade.IdsTipoPagamento.Count(); i++)
                {
                    var tipoPagamentoSelecionado = selectTipoPagamentosVM.Where(x => x.Codigo == unidade.IdsTipoPagamento[i].ToString()).FirstOrDefault();
                    tipoPagamentoSelecionado.DataInsercao = DateTime.Now;
                    tiposPagamentoSelecionados.Add(tipoPagamentoSelecionado);
                }

                unidade.TiposPagamento = tiposPagamentoSelecionados;
                unidade.DataInsercao = DateTime.Now;
                unidade.Endereco = endereco;
                unidade.EstruturasUnidade = estruturasUnidade;
                unidade.UnidadeCheckListAtividades = unidadesCheckLis;
                unidade.UnidadeCheckListTipoAtividades = ListaUnidadeCheckListTipoAtividade;

                var unidadeEntity = Mapper.Map<UnidadeViewModel, Unidade>(unidade);

                //bloco necessário para tratar a cidade
                if (endereco != null)
                {
                    unidadeEntity.Endereco = endereco.ToEntity();

                    if (endereco.Cidade != null && !string.IsNullOrEmpty(endereco.Cidade.Descricao))
                    {
                        var cidade = _cidadeAplicacao.Buscar().Where(x => x.Descricao.ToLower().Trim().Equals(endereco.Cidade.Descricao.ToLower().Trim())).FirstOrDefault();

                        unidadeEntity.Endereco.Cidade = cidade;
                    }
                }

                unidadeEntity.Responsavel = _funcionarioAplicacao.BuscarPorId(Convert.ToInt32(unidade.IdResponsavel));

                if (unidade.IdsTipoPagamento.Count > 0 && unidade.IdsTipoPagamento.Where(x => x == 1 || x == 2).Any())
                {
                    if (unidadeEntity.MaquinaCartao == null)
                        unidadeEntity.MaquinaCartao = new MaquinaCartao();

                    unidade.MaquinaCartao.CNPJ.Tipo = TipoDocumento.Cnpj;
                    unidadeEntity.MaquinaCartao.Responsavel = _funcionarioAplicacao.BuscarPorId(Convert.ToInt32(unidade.MaquinaCartao.IdSupervisorMaquina));
                }
                else
                {
                    unidadeEntity.MaquinaCartao = null;
                }

                if (ListaCheckList != null && ListaCheckList.Count > 0)
                    unidadeEntity.CheckListEstruturaUnidade = ListaEquipamentos;

                unidadeEntity.Empresa = _empresaAplicacao.BuscarPorId(unidade.Empresa.Id);

                if (!ListaUnidadeCheckListTipoAtividade.Any())
                    unidadeEntity.CheckListAtividade = null;

                _unidadeAplicacao.Salvar(unidadeEntity);
                ModelState.Clear();

                ListaUnidadeCheckListTipoAtividade = new List<UnidadeCheckListAtividadeTipoAtividadeViewModel>();
                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (SoftparkIntegrationException sx)
            {
                CriarModalAvisoComRetornoParaIndex(sx.Message);
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro("Ocorreu um erro ao salvar: " + ex.Message);
            }

            return View("Index");
        }

        // GET: Unidade/Edita/id
        public ActionResult Edita(int id)
        {
            ListaUnidadeCheckListTipoAtividade = new List<UnidadeCheckListAtividadeTipoAtividadeViewModel>();

            PopularDados();

            if (id > 0)
            {
                var unidadeDM = _unidadeAplicacao.BuscarPorId(id);

                var unidadeVM = Mapper.Map<Unidade, UnidadeViewModel>(unidadeDM);
                unidadeVM.IdResponsavel = unidadeVM.Responsavel != null ? unidadeVM.Responsavel.Id.ToString() : "";

                if (unidadeVM.MaquinaCartao != null)
                {
                    unidadeVM.MaquinaCartao.IdSupervisorMaquina = unidadeVM.MaquinaCartao.Responsavel != null ? unidadeVM.MaquinaCartao.Responsavel.Id.ToString() : "";
                }

                List<int> idsTiposPagamentos = new List<int>();

                foreach (var item in unidadeVM.TiposPagamento)
                {
                    idsTiposPagamentos.Add(Convert.ToInt16(item.Codigo));

                    foreach (var tipopagamento in selectTipoPagamentos)
                    {
                        if (tipopagamento.Value == item.Codigo)
                        {
                            tipopagamento.Selected = true;
                        }
                    }

                }

                unidadeVM.IdsTipoPagamento = idsTiposPagamentos;

                if (unidadeVM.UnidadeCheckListTipoAtividades != null && unidadeVM.UnidadeCheckListTipoAtividades.Any())
                    ListaUnidadeCheckListTipoAtividade = unidadeVM.UnidadeCheckListTipoAtividades.ToList();
                else if (unidadeVM.CheckListAtividade != null)
                    CarregarCheckListAtividade(unidadeVM.CheckListAtividade.Id);


                //Adicionar a lista de Estrutura Unidade as que também não foram previamente definidas 
                var list = Helpers.Unidade.ListaCheckListEquipamentoUnidade(this.ListaEstruturaGaragem);
                var list2 = list.Where(item => !unidadeVM.CheckListEstruturaUnidade.Any(item2 => item2.EstruturaGaragem == item.EstruturaGaragem));
                ((List<CheckListEstruturaUnidade>)unidadeVM.CheckListEstruturaUnidade).AddRange(list2);

                return View("Index", unidadeVM);
            }

            return View();
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

            return View("Index");
        }

        public JsonResult BuscarUnidade(string unidade)
        {
            var unidades = new List<UnidadeViewModel>();

            if (string.IsNullOrEmpty(unidade))
            {
                unidades = Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(_unidadeAplicacao.ListarOrdenadoSimplificado().ToList());
            }
            else
            {
                unidades = Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(_unidadeAplicacao.ListarOrdenadoSimplificado()?.Where(x => x.Nome.ToLower() == unidade.ToLower())?.ToList());
            }

            Dictionary<string, object> jsonResult = new Dictionary<string, object>();

            jsonResult.Add("Html", Helpers.RazorHelper.RenderRazorViewToString(ControllerContext, "_GridUnidade", unidades));
            jsonResult.Add("Status", "Success");
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AtualizaEquipamentos(List<CheckListEstruturaUnidadeViewModel> listquipamentos)
        {
            var unidadeEntity = Mapper.Map<List<CheckListEstruturaUnidadeViewModel>, List<CheckListEstruturaUnidade>>(listquipamentos);

            ListaEquipamentos = unidadeEntity;

            return PartialView("_GridCheckListEstruturaUnidade", listquipamentos);
        }

        [HttpPost]
        public ActionResult CarregarCheckListAtividade(int id)
        {
            if (id == 0)
            {
                ListaUnidadeCheckListTipoAtividade = new List<UnidadeCheckListAtividadeTipoAtividadeViewModel>();
            }
            else
            {
                var listaCheckList = ListaCheckList.FirstOrDefault(x => x.Id == id);

                var listaUnidadeCheckListTipoAtividade = new List<UnidadeCheckListAtividadeTipoAtividadeViewModel>();

                foreach (var item in listaCheckList.TiposAtividade)
                {
                    listaUnidadeCheckListTipoAtividade.Add(new UnidadeCheckListAtividadeTipoAtividadeViewModel
                    {
                        TipoAtividade = item.TipoAtividade
                    });
                }

                ListaUnidadeCheckListTipoAtividade = listaUnidadeCheckListTipoAtividade;
            }

            return PartialView("_GridUnidadeCheckListAtividade");
        }

        [HttpPost]
        public void AlternarStatusTipoAtividade(int id)
        {
            var unidadeTipoAtividade = ListaUnidadeCheckListTipoAtividade.FirstOrDefault(x => x.TipoAtividade.Id == id);

            unidadeTipoAtividade.Selecionado = !unidadeTipoAtividade.Selecionado;
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarPeloNome(string nome)
        {
            var lista = _unidadeAplicacao.BuscarPor(x => x.Nome.ToLower().Contains(nome.ToLower()));

            return Json(lista.Select(c => new
            {
                c.Id,
                c.Nome
            }));
        }
    }
}
