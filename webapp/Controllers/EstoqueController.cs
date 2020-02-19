using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class EstoqueController : GenericController<Estoque>
    {
        public List<Estoque> ListaEstoque => _estoqueAplicacao?.Buscar()?.ToList() ?? new List<Estoque>();

        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IEstoqueAplicacao _estoqueAplicacao;

        public List<ChaveValorViewModel> _tipoendereco = new List<ChaveValorViewModel>()
            {
               new ChaveValorViewModel{ Id = 1, Descricao = "Residencial" },
               new ChaveValorViewModel{ Id = 2, Descricao = "Comercial" }
            };

        public EstoqueController(IEstoqueAplicacao EstoqueAplicacao,
                                               IUnidadeAplicacao unidadeAplicacao)
        {
            Aplicacao = EstoqueAplicacao;
            _estoqueAplicacao = EstoqueAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        public List<UnidadeViewModel> ListaUnidade => AutoMapper.Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(_unidadeAplicacao.Buscar().ToList());

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(EstoqueViewModel entity)
        {
            try
            {
                _estoqueAplicacao.SalvarDados(entity);

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
                ModelState.Clear();
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Index", entity);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", entity);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var objEstoque = new EstoqueViewModel(Aplicacao.BuscarPorId(id));


            return View("Index", objEstoque);
        }

        public JsonResult VerificarSeExisteEstoquePrincipal()
        {
            var estoque = _estoqueAplicacao.PrimeiroPor(x => x.EstoquePrincipal == true);
            return Json(new
            {
                Existe = estoque != null,
                Estoque = estoque?.Nome
            }, JsonRequestBehavior.AllowGet);
        }
    }
}