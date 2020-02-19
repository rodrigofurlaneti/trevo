using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Enums;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using System.Net;

namespace Portal.Controllers
{
    public class MaterialController : GenericController<Material>
    {
        private readonly IMaterialAplicacao _materialAplicacao;
        private readonly ITipoMaterialAplicacao _tipoMaterialAplicacao;
        private readonly IFornecedorAplicacao _fornecedorAplicacao;

        public List<TipoMaterialViewModel> ListaTipoMaterial => _tipoMaterialAplicacao.BuscarOrdenados();

        public List<FornecedorViewModel> ListaFornecedores
        {
            get => (List<FornecedorViewModel>)Session["Fornecedores"];
            set => Session["Fornecedores"] = value;
        }

        public List<MaterialFornecedorViewModel> ListaMaterialFornecedores
        {
            get => (List<MaterialFornecedorViewModel>)Session["MaterialFornecedores"];
            set => Session["MaterialFornecedores"] = value;
        }

        public MaterialController(
            IMaterialAplicacao materialAplicacao,
            ITipoMaterialAplicacao tipoMaterialAplicacao,
            IFornecedorAplicacao fornecedorAplicacao
            )
        {
            Aplicacao = materialAplicacao;
            _materialAplicacao = materialAplicacao;
            _tipoMaterialAplicacao = tipoMaterialAplicacao;
            _fornecedorAplicacao = fornecedorAplicacao;
        }

        public override ActionResult Index()
        {
            ListaMaterialFornecedores = null;
            CarregarListaFornecedores();

            return base.Index();
        }

        public override ActionResult Edit(int id)
        {
            CarregarListaFornecedores();
            var material = _materialAplicacao.BuscarPorId(id);
            var materialViewModel = Mapper.Map<MaterialViewModel>(material);
            ListaMaterialFornecedores = materialViewModel.MaterialFornecedores;

            return View("Index", materialViewModel);
        }

        [HttpPost]
        public ActionResult SalvarDados(MaterialViewModel materialViewModel)
        {
            CarregarListaFornecedores();
            try
            {
                materialViewModel.MaterialFornecedores = ListaMaterialFornecedores;
                _materialAplicacao.SalvarDados(materialViewModel);

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
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Erro",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
            }

            return View("Index", materialViewModel);
        }

        public ActionResult BuscarMateriais()
        {
            var materiaisViewModel = Mapper.Map<List<MaterialViewModel>>(_materialAplicacao.Buscar());

            return PartialView("_Grid", materiaisViewModel);
        }

        public ActionResult BuscarEstoqueMateriaisPeloMaterialId(int materialId)
        {
            var estoqueMateriaisViewModel = _materialAplicacao.BuscarEstoqueMateriaisPeloMaterialId(materialId);

            return PartialView("_GridEstoque", estoqueMateriaisViewModel);
        }

        public ActionResult BuscarMaterialHistoricoPeloMaterialId(int materialId)
        {
            var materiasHistoricoViewModel = _materialAplicacao.BuscarMaterialHistoricoPeloMaterialId(materialId);

            return PartialView("_ModalMaterialHistorico", materiasHistoricoViewModel);
        }

        public JsonResult BuscarMaterialFornecedores()
        {
            ListaMaterialFornecedores?.ForEach(x => {
                if (x.Material != null)
                    x.Material.MaterialFornecedores = null;
            });
            return Json(ListaMaterialFornecedores);
        }

        public ActionResult AtualizarMaterialFornecedores(List<MaterialFornecedorViewModel> materialFornecedores)
        {
            ListaMaterialFornecedores = materialFornecedores;

            return PartialView("_GridMaterialFornecedor", ListaMaterialFornecedores);
        }

        public JsonResult ChecarSerFornecedorTemEmail(int id)
        {
            return Json(ListaFornecedores.FirstOrDefault(x => x.Id == id).Contatos?.Any(x => x.Tipo == TipoContato.Email));
        }

        public ActionResult BuscarPrecoDoEstoque(int materialId, int estoqueId)
        {
            var estoqueMaterial = _materialAplicacao.BuscarEstoqueMaterial(materialId, estoqueId);

            if(estoqueMaterial.Quantidade == 0)
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, $"O item {estoqueMaterial.Material.Nome} não possui quantidade em estoque. <a target='_blank' href='/estoquemanual/index'>Clique Aqui</a> para dar entrada.");

            return Json(new
            {
                estoqueMaterial.Preco
            }, JsonRequestBehavior.AllowGet);
        }

        private void CarregarListaFornecedores()
        {
            ListaFornecedores = _fornecedorAplicacao.Buscar()?.Select(x => new FornecedorViewModel(x)).ToList() ?? new List<FornecedorViewModel>();
        }
    }
}