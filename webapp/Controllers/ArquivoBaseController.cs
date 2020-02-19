//using Aplicacao;
//using Aplicacao.ViewModels;
//using AutoMapper;
//using Core.Exceptions;
//using Core.Extensions;
//using Entidade;
//using Entidade.Uteis;
//using Newtonsoft.Json;
//using Portal.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;

//namespace Portal.Controllers
//{
//    public class ArquivoBaseController : GenericController<ArquivoBase>
//    {
//        private readonly IArquivoBaseAplicacao _arquivoBaseAplicacao;

//        public ArquivoBaseImpressaoViewModel ItemImpressao
//        {
//            get => (ArquivoBaseImpressaoViewModel)Session["ItemImpressao"] ?? new ArquivoBaseImpressaoViewModel();
//            set => Session["ItemImpressao"] = value;
//        }

//        public List<ArquivoBaseDetalheViewModel> ListaFuncionarioItens
//        {
//            get => (List<ArquivoBaseDetalheViewModel>)Session["ListaFuncionarioItens"] ?? new List<ArquivoBaseDetalheViewModel>();
//            set => Session["ListaFuncionarioItens"] = value;
//        }

//        public List<ArquivoBaseDetalheViewModel> ItensSelecionados
//        {
//            get => (List<ArquivoBaseDetalheViewModel>)Session["ItensSelecionados"] ?? new List<ArquivoBaseDetalheViewModel>();
//            set => Session["ItensSelecionados"] = value;
//        }

//        public ArquivoBaseController(
//            IArquivoBaseAplicacao arquivoBaseAplicacao
//        )
//        {
//            Aplicacao = arquivoBaseAplicacao;
//            _arquivoBaseAplicacao = arquivoBaseAplicacao;
//        }

//        [CheckSessionOut]
//        public override ActionResult Index()
//        {
//            ListaFuncionarioItens = new List<ArquivoBaseDetalheViewModel>();
//            ItensSelecionados = new List<ArquivoBaseDetalheViewModel>();
//            return View("Index");
//        }

//        [CheckSessionOut]
//        [HttpPost]
//        public ActionResult SalvarDados(ArquivoBaseViewModel model)
//        {
//            try
//            {
//                model.ArquivoBasesDetalhes = ListaFuncionarioItens;
//                var arquivoBase = Mapper.Map<ArquivoBase>(model);
//                _arquivoBaseAplicacao.Salvar(arquivoBase);
//                ModelState.Clear();
//                CriarDadosModalSucesso("Registro salvo com sucesso");
//            }
//            catch (BusinessRuleException br)
//            {
//                CriarDadosModalAviso(br.Message);
//                return View("Index", model);
//            }
//            catch (Exception ex)
//            {
//                CriarDadosModalErro("Ocorreu um erro ao salvar: " + ex.Message);
//                return View("Index", model);
//            }
//            return View("Index");
//        }

//        public override ActionResult Edit(int id)
//        {
//            ItensSelecionados = new List<ArquivoBaseDetalheViewModel>();
//            var arquivoBase = _arquivoBaseAplicacao.BuscarPorId(id);
//            var arquivoBaseVM = Mapper.Map<ArquivoBaseViewModel>(arquivoBase);
//            ListaFuncionarioItens = arquivoBaseVM.ArquivoBasesDetalhes;
//            return View("Index", arquivoBaseVM);
//        }

//        public ActionResult AdicionarFuncionarioItem(int materialId, string valor, int quantidade, string valorTotal)
//        {
//            if (ListaFuncionarioItens.Any(x => x.Material?.Id == materialId))
//                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Já foi adicionado informações para este item");

//            var materialVM = Mapper.Map<MaterialViewModel>(_materialAplicacao.BuscarPorId(materialId));
//            var item = new ArquivoBaseDetalheViewModel(materialVM, valor, quantidade, valorTotal);
//            var listaFuncionarioItens = ListaFuncionarioItens;

//            listaFuncionarioItens.Add(item);
//            ListaFuncionarioItens = listaFuncionarioItens;

//            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridItensDetalhes", ListaFuncionarioItens);
//            return Json(new
//            {
//                Grid = grid,
//            });
//        }

//        public ActionResult RemoverFuncionarioItem(int materialId)
//        {
//            var item = ListaFuncionarioItens.FirstOrDefault(x => x.Material.Id == materialId);
//            var listaFuncionarioItens = ListaFuncionarioItens;

//            listaFuncionarioItens.Remove(item);
//            ListaFuncionarioItens = listaFuncionarioItens;

//            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridItensDetalhes", ListaFuncionarioItens);
//            return Json(new
//            {
//                Grid = grid,
//            });
//        }

//        public ActionResult EditarFuncionarioItem(int materialId)
//        {
//            var item = ListaFuncionarioItens.FirstOrDefault(x => x.Material.Id == materialId);
//            var listaFuncionarioItens = ListaFuncionarioItens;

//            listaFuncionarioItens.Remove(item);
//            ListaFuncionarioItens = listaFuncionarioItens;

//            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridItensDetalhes", ListaFuncionarioItens);
//            return Json(new
//            {
//                Grid = grid,
//                Item = RemoverLoopDoJson(item)
//            });
//        }

//        public void AlternarItemSelecionado(int materialId)
//        {
//            var itensSelecionado = ItensSelecionados;

//            if (itensSelecionado.Any(x => x.Material.Id == materialId))
//            {
//                ItensSelecionados.RemoveAll(x => x.Material.Id == materialId);
//            }
//            else
//            {
//                var itemSelecionado = ListaFuncionarioItens.FirstOrDefault(x => x.Material.Id == materialId);
//                itensSelecionado.Add(itemSelecionado);
//            }
//            ItensSelecionados = ItensSelecionados;
//        }

//        public void AltenarSelecionarTudo(bool selecionado)
//        {
//            if (selecionado)
//                ItensSelecionados = ListaFuncionarioItens;
//            else
//                ItensSelecionados = new List<ArquivoBaseDetalheViewModel>();
//        }

//        public PartialViewResult BuscarArquivoBase()
//        {
//            var itens = _arquivoBaseAplicacao.Buscar();
//            var itensVM = Mapper.Map<List<ArquivoBaseViewModel>>(itens);

//            return PartialView("_Grid", itensVM);
//        }

//        public HttpStatusCodeResult ArmazenarDadosImpressao(int? funcionarioId, DateTime? dataEntrega, DateTime? dataDevolucao)
//        {
//            if (funcionarioId == null)
//                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Informe o Funcionário");

//            if (!dataEntrega.HasValue)
//                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Informe a Data de Entrega");

//            if (!ItensSelecionados.Any())
//                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Nenhum item Selecionado");

//            var funcionarioVM = Mapper.Map<FuncionarioViewModel>(_funcionarioAplicacao.BuscarPorId(funcionarioId.Value));
//            ItemImpressao = new ArquivoBaseImpressaoViewModel(funcionarioVM, dataEntrega, dataDevolucao, ItensSelecionados);

//            return new HttpStatusCodeResult(HttpStatusCode.OK);
//        }

//        public ActionResult Impressao()
//        {
//            return PartialView("Impressao", ItemImpressao);
//        }
//    }
//}