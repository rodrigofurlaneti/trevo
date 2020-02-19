using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Portal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ControleFeriasController : GenericController<ControleFerias>
    {
        private readonly IControleFeriasAplicacao _controleFeriasAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;

        public ControleFeriasController(
            IControleFeriasAplicacao controleFeriasAplicacao,
            IFuncionarioAplicacao funcionarioAplicacao
        )
        {
            Aplicacao = controleFeriasAplicacao;
            _controleFeriasAplicacao = controleFeriasAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ControleFeriasViewModel model)
        {
            try
            {
                var controleFerias = Mapper.Map<ControleFerias>(model);
                _controleFeriasAplicacao.Salvar(controleFerias);
                ModelState.Clear();
                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro("Ocorreu um erro ao salvar: " + ex.Message);
                return View("Index", model);
            }
            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var controleFerias = _controleFeriasAplicacao.BuscarPorId(id);
            var controleFeriasVM = Mapper.Map<ControleFeriasViewModel>(controleFerias);
            return View("Index", controleFeriasVM);
        }

        public JsonResult BuscarControleFerias(int pagina = 1, BuscarGridControleFeriasViewModel dto = null)
        {
            var listaControleFerias = new List<ControleFerias>();
            PaginacaoGenericaViewModel paginacao = null;
            var quantidadePorPagina = 10;

            var meses = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat
              .MonthNames
              .TakeWhile(m => m != String.Empty)
              .Select((monthName, index) =>
                  new ChaveValorViewModel
                  {
                      Id = index + 1,
                      Descricao = monthName
                  }
            ).ToList();


            if (dto == null || 
                (string.IsNullOrEmpty(dto.NomeFuncionario) &&
                string.IsNullOrEmpty(dto.NomeUnidade) &&
                string.IsNullOrEmpty(dto.Mes) &&
                string.IsNullOrEmpty(dto.Ano) &&
                !dto.DataInicial.HasValue &&
                !dto.DataFinal.HasValue &&
                string.IsNullOrEmpty(dto.Trabalhada) &&
                !dto.TrabalhoDe.HasValue &&
                !dto.TrabalhoAte.HasValue))
            {
                var quantidadeFuncionarios = _controleFeriasAplicacao.Contar();
                paginacao = new PaginacaoGenericaViewModel(quantidadePorPagina, pagina, quantidadeFuncionarios);
                listaControleFerias = _controleFeriasAplicacao.BuscarPorIntervaloOrdernadoPorAlias(paginacao.RegistroInicial, paginacao.RegistrosPorPagina, "Funcionario.Pessoa.Nome").ToList();
            }
            else
            {
                var predicate = PredicateBuilder.True<ControleFerias>();

                if(!string.IsNullOrEmpty(dto.NomeFuncionario))
                    predicate = predicate.And(x => x.Funcionario.Pessoa.Nome.ToLower().Contains(dto.NomeFuncionario.ToLower()));

                if (!string.IsNullOrEmpty(dto.NomeUnidade))
                    predicate = predicate.And(x => x.Funcionario.Unidade != null && x.Funcionario.Unidade.Nome.ToLower().Contains(dto.NomeUnidade.ToLower()));

                if (!string.IsNullOrEmpty(dto.Mes))
                {
                    var mesesContem = meses.Where(x => x.Descricao.ToLower().Contains(dto.Mes.ToLower())).Select(x => x.Id).ToList();
                    predicate = predicate.And(x => mesesContem.Contains(x.DataInicial.Month));
                }

                if (!string.IsNullOrEmpty(dto.Ano))
                    predicate = predicate.And(x => x.DataInicial.Year.ToString().ToLower().Contains(dto.Ano));

                if (dto.DataInicial.HasValue)
                    predicate = predicate.And(x => x.DataInicial.Date == dto.DataInicial.Value.Date);

                if (dto.DataFinal.HasValue)
                    predicate = predicate.And(x => x.DataFinal.Date == dto.DataFinal.Value.Date);

                if (!string.IsNullOrEmpty(dto.Trabalhada))
                    predicate = predicate.And(x => (x.AutorizadoTrabalhar ? "Sim" : "Não").ToLower().Contains(dto.Trabalhada.ToLower()));

                listaControleFerias = _controleFeriasAplicacao.BuscarPor(predicate).ToList();
                paginacao = new PaginacaoGenericaViewModel(quantidadePorPagina, pagina, listaControleFerias.Count);

                listaControleFerias = listaControleFerias.OrderBy(x => x.Funcionario.Pessoa.Nome).ToList();
                listaControleFerias = listaControleFerias.ItensDaPagina(paginacao.RegistroInicial, paginacao.RegistrosPorPagina).ToList();
            }

            ViewBag.Paginacao = paginacao;
            var partialPaginacao = RazorHelper.RenderRazorViewToString(ControllerContext, "~/Views/Shared/_PaginacaoGenericaAjax.cshtml", null);

            var listaControleFeriasVM = Mapper.Map<List<ControleFeriasViewModel>>(listaControleFerias);
            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_Grid", listaControleFeriasVM);

            return Json(new {
                Grid = grid,
                PartialPaginacao = partialPaginacao
            });
        }

        public PartialViewResult AutorizarTrabalhar(int controleFeriasId, bool autorizado)
        {
            var controleFerias = _controleFeriasAplicacao.BuscarPorId(controleFeriasId);

            controleFerias.AutorizadoTrabalhar = autorizado;
            _controleFeriasAplicacao.Salvar(controleFerias);

            var listaControleFerias = _controleFeriasAplicacao.Buscar();
            var listaControleFeriasVM = Mapper.Map<List<ControleFeriasViewModel>>(listaControleFerias);

            return PartialView("_Grid", listaControleFeriasVM);
        }

        public JsonResult BuscarFuncionarioUnidade(int funcionarioId)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            return Json(new
            {
                NomeUnidade = funcionario?.Unidade?.Nome
            });
        }



        public ActionResult AtualizarGridPeriodoPermitido(int controleFeriasId)
        {
            var controleFerias = _controleFeriasAplicacao.BuscarPorId(controleFeriasId);
            var listaIntervaloVM = Mapper.Map<List<ControleFeriasPeriodoPermitidoViewModel>>(controleFerias.ListaPeriodoPermitido);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridPeriodoPermitido", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult AdicionarPeriodoPermitido(int controleFeriasId, DateTime dataDe, DateTime dataAte)
        {
            var controleFerias = _controleFeriasAplicacao.BuscarPorId(controleFeriasId);
            controleFerias.ListaPeriodoPermitido = controleFerias.ListaPeriodoPermitido ?? new List<ControleFeriasPeriodoPermitido>();

            if (!(dataDe.Date >= controleFerias.DataInicial.Date && dataDe.Date <= controleFerias.DataFinal.Date &&
               dataAte.Date >= controleFerias.DataInicial.Date && dataAte.Date <= controleFerias.DataFinal.Date))
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, $"As datas precisam estar dentro do periodo de férias do funcionário. {controleFerias.DataInicial.ToShortDateString()} - {controleFerias.DataFinal.ToShortDateString()}");

            controleFerias.ListaPeriodoPermitido.Add(new ControleFeriasPeriodoPermitido
            {
                DataDe = dataDe,
                DataAte = dataAte
            });
            _controleFeriasAplicacao.Salvar(controleFerias);

            var listaIntervaloVM = Mapper.Map<List<ControleFeriasPeriodoPermitidoViewModel>>(controleFerias.ListaPeriodoPermitido);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridPeriodoPermitido", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult RemoverPeriodoPermitido(int controleFeriasId, DateTime dataDe, DateTime dataAte)
        {
            var controleFerias = _controleFeriasAplicacao.BuscarPorId(controleFeriasId);
            var item = controleFerias.ListaPeriodoPermitido.FirstOrDefault(x => x.DataDe.Date == dataDe.Date && x.DataAte.Date == dataAte.Date);
            controleFerias.ListaPeriodoPermitido.Remove(item);
            _controleFeriasAplicacao.Salvar(controleFerias);

            var listaIntervaloVM = Mapper.Map<List<ControleFeriasPeriodoPermitidoViewModel>>(controleFerias.ListaPeriodoPermitido);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridPeriodoPermitido", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult EditarPeriodoPermitido(int controleFeriasId, DateTime dataDe, DateTime dataAte)
        {
            var controleFerias = _controleFeriasAplicacao.BuscarPorId(controleFeriasId);
            var item = controleFerias.ListaPeriodoPermitido.FirstOrDefault(x => x.DataDe.Date == dataDe.Date && x.DataAte.Date == dataAte.Date);
            controleFerias.ListaPeriodoPermitido.Remove(item);
            _controleFeriasAplicacao.Salvar(controleFerias);

            var listaIntervaloVM = Mapper.Map<List<ControleFeriasPeriodoPermitidoViewModel>>(controleFerias.ListaPeriodoPermitido);
            var itemVM = Mapper.Map<ControleFeriasPeriodoPermitidoViewModel>(item);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridPeriodoPermitido", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(itemVM)
            });
        }
    }
}