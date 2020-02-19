using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class TabelaPrecoAvulsoController : GenericController<TabelaPrecoAvulso>
    {
        public IList<ChaveValorViewModel> ListaUnidade
        {
            get { return (List<ChaveValorViewModel>)TempData["ListaUnidadeTabelaPrecoAvulso"] ?? new List<ChaveValorViewModel>(); }
            set { TempData["ListaUnidadeTabelaPrecoAvulso"] = value; }
        }

        private readonly ITabelaPrecoAvulsoAplicacao _tabelaPrecoAvulsoAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public TabelaPrecoAvulsoController(
            ITabelaPrecoAvulsoAplicacao tabelaPrecoAvulsoAplicacao,
            IUnidadeAplicacao unidadeAplicacao)
        {
            _tabelaPrecoAvulsoAplicacao = tabelaPrecoAvulsoAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }
        
        public override ActionResult Index()
        {
            CarregaCombo();
            return View();
        }

        public override ActionResult Edit(int id)
        {
            CarregaCombo();
            var viewModel = _tabelaPrecoAvulsoAplicacao.CarregarTelaParaEdicao(id);
            return View("Index", viewModel);
        }

        [CheckSessionOut]
        [HttpPost]
        public JsonResult SalvarDados(TabelaPrecoAvulsoViewModel model)
        {
            TempData.Keep();

            Resultado<TabelaPrecoAvulsoViewModel> resultado = new Resultado<TabelaPrecoAvulsoViewModel>();

            try
            {
                var usuarioLogado = HttpContext.User as CustomPrincipal;
                _tabelaPrecoAvulsoAplicacao.Salvar(model, usuarioLogado.UsuarioId);
                ModelState.Clear();

                resultado.Sucesso = true;
                resultado.Redirect = true;
                resultado.TipoModal = TipoModal.Success.ToDescription();
                resultado.Titulo = "Sucesso";
                resultado.Mensagem = "Registro Salvo com Sucesso";
                resultado.Model = model;
            }
            catch(SoftparkIntegrationException sx)
            {
                var modal = CriarModalAvisoComRetornoParaIndex(sx.Message);
                resultado.Sucesso = modal.Sucesso;
                resultado.Redirect = true;
                resultado.TipoModal = modal.TipoModal.ToDescription();
                resultado.Titulo = modal.Titulo;
                resultado.Mensagem = modal.Mensagem;
            }
            catch (BusinessRuleException br)
            {
                resultado.Sucesso = false;
                resultado.TipoModal = TipoModal.Warning.ToDescription();
                resultado.Titulo = "Atenção";
                resultado.Mensagem = br.Message;
            }
            catch (Exception ex)
            {
                resultado.Sucesso = false;
                resultado.TipoModal = TipoModal.Danger.ToDescription();
                resultado.Titulo = "Erro";
                resultado.Mensagem = ex.Message;
            }

            return Json(resultado);
        }

        public ActionResult CarregarPeriodo(int id)
        {
            TempData.Keep();
            var viewModel = _tabelaPrecoAvulsoAplicacao.CarregarGridPeriodo(id);

            return PartialView("_Periodo", viewModel);
        }

        public ActionResult CarregarHoraValor(int id)
        {
            TempData.Keep();
            var viewModel = _tabelaPrecoAvulsoAplicacao.CarregarGridHoraValor(id);

            return PartialView("_HoraValor", viewModel);
        }

        public ActionResult CarregarUnidadeVigencia(int id)
        {
            TempData.Keep();
            var viewModel = _tabelaPrecoAvulsoAplicacao.CarregarGridUnidade(id);

            return PartialView("_UnidadeVigencia", viewModel);
        }

        public ActionResult CarregarGrid()
        {
            TempData.Keep();
            var viewModel = _tabelaPrecoAvulsoAplicacao.CarregarGrid();
            
            return PartialView("_Grid", viewModel);
        }

        public ActionResult AdicionarUnidade(List<TabelaPrecoAvulsoUnidadeViewModel> listaUnidade)
        {
            TempData.Keep();

            var viewModel = new List<TabelaPrecoAvulsoUnidadeViewModel>();
            foreach (var item in listaUnidade.OrderBy(x => x.Unidade.Nome))
                viewModel.Add(new TabelaPrecoAvulsoUnidadeViewModel(item.Unidade, item.HoraInicio, item.HoraFim, item.ValorDiaria));

            return PartialView("_UnidadeVigencia", viewModel);
        }

        public ActionResult AdicionarHoraValor(List<TabelaPrecoAvulsoHoraValorViewModel> listaHoraValor)
        {
            TempData.Keep();

            var viewModel = new List<TabelaPrecoAvulsoHoraValorViewModel>();
            foreach (var item in listaHoraValor.OrderBy(x => x.Hora))
                viewModel.Add(new TabelaPrecoAvulsoHoraValorViewModel(item.Hora, item.Valor));

            return PartialView("_HoraValor", viewModel);
        }

        public ActionResult Excluir(int id)
        {
            TempData.Keep();

            var resultado = new Resultado<TabelaPrecoAvulsoViewModel>();

            try
            {
                _tabelaPrecoAvulsoAplicacao.Excluir(id);

                resultado.Sucesso = true;
                resultado.Redirect = true;
                resultado.TipoModal = TipoModal.Success.ToDescription();
                resultado.Titulo = "Sucesso";
                resultado.Mensagem = "Tabela removida com Sucesso";
            }
            catch (BusinessRuleException br)
            {
                resultado.Sucesso = false;
                resultado.TipoModal = TipoModal.Warning.ToDescription();
                resultado.Titulo = "Atenção";
                resultado.Mensagem = br.Message;
            }
            catch (SoftparkIntegrationException sx)
            {
                var modal = CriarModalAvisoComRetornoParaIndex(sx.Message);
                resultado.Sucesso = modal.Sucesso;
                resultado.Redirect = true;
                resultado.TipoModal = modal.TipoModal.ToDescription();
                resultado.Titulo = modal.Titulo;
                resultado.Mensagem = modal.Mensagem;
            }
            catch (Exception ex)
            {
                resultado.Sucesso = false;
                resultado.TipoModal = TipoModal.Danger.ToDescription();
                resultado.Titulo = "Erro";
                resultado.Mensagem = ex.Message;
            }

            return Json(resultado);
        }

        public ActionResult ExcluirUnidadeDaTabela(int id, int idUnidade)
        {
            TempData.Keep();

            var resultado = new Resultado<TabelaPrecoAvulsoViewModel>
            {
                Sucesso = true,
                TipoModal = TipoModal.Success.ToDescription(),
                Titulo = "Sucesso",
                Mensagem = "Unidade removida da tabela com Sucesso"
            };

            try
            {
                var usuarioLogado = HttpContext.User as CustomPrincipal;
                _tabelaPrecoAvulsoAplicacao.Excluir(id, idUnidade, usuarioLogado.UsuarioId);
            }
            catch (BusinessRuleException br)
            {
                resultado.Sucesso = false;
                resultado.TipoModal = TipoModal.Warning.ToDescription();
                resultado.Titulo = "Atenção";
                resultado.Mensagem = br.Message;
            }
            catch (SoftparkIntegrationException sx)
            {
                var modal = CriarModalAvisoComRetornoParaIndex(sx.Message);
                resultado.Sucesso = modal.Sucesso;
                resultado.Redirect = true;
                resultado.TipoModal = modal.TipoModal.ToDescription();
                resultado.Titulo = modal.Titulo;
                resultado.Mensagem = modal.Mensagem;
            }
            catch (Exception ex)
            {
                resultado.Sucesso = false;
                resultado.TipoModal = TipoModal.Danger.ToDescription();
                resultado.Titulo = "Erro";
                resultado.Mensagem = "Erro ao remover unidade da Tabela";
            }

            return Json(resultado);
        }

        private void CarregaCombo()
        {
            TempData.Clear();

            var lista = _unidadeAplicacao.ListarOrdenadas()
                .Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = x.Nome
                })
                .ToList();

            ListaUnidade = lista;

            TempData.Keep();
        }
    }
}