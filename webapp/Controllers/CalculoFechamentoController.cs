using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class CalculoFechamentoController : GenericController<CalculoFechamento>
    {
        private readonly ICalculoFechamentoAplicacao calculoFechamentoAplicacao;
        private readonly IConsolidaAjusteFaturamentoAplicacao consolidaAjusteFaturamentoAplicacao;
        private readonly IEmpresaAplicacao empresaUnidadeAplicacao;
        private readonly IUnidadeAplicacao unidadeAplicacao;

        public CalculoFechamentoController(ICalculoFechamentoAplicacao calculoFechamentoAplicacao,
                                           IConsolidaAjusteFaturamentoAplicacao consolidaAjusteFaturamentoAplicacao,
                                           IEmpresaAplicacao empresaUnidadeAplicacao,
                                           IUnidadeAplicacao unidadeAplicacao)
        {
            this.calculoFechamentoAplicacao = calculoFechamentoAplicacao;
            this.consolidaAjusteFaturamentoAplicacao = consolidaAjusteFaturamentoAplicacao;
            this.empresaUnidadeAplicacao = empresaUnidadeAplicacao;
            this.unidadeAplicacao = unidadeAplicacao;

            ViewBag.ListaMes = new SelectList(
              Enum.GetValues(typeof(Mes)).Cast<Mes>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
              "Id",
              "Descricao");

            ViewBag.ListaAnos = new SelectList(Utils.UtilsServico.ListaAnosAnteriores(DateTime.Now.Year, 5));

        }

        public List<EmpresaViewModel> ListaEmpresaUnidade => AutoMapper.Mapper.Map<List<Empresa>, List<EmpresaViewModel>>(empresaUnidadeAplicacao.Buscar().ToList());

        // GET: CalculoFechamento
        [CheckSessionOut]
        public override ActionResult Index()
        {
            ViewBag.ListaUnidade = new List<UnidadeViewModel>();
            ViewBag.ListaAnos = new SelectList(Utils.UtilsServico.ListaAnosAnteriores(DateTime.Now.Year, 5));
            return View();
        }

        [HttpPost]
        public JsonResult BuscarUnidades(int IdEmpresa)
        {
            var unidades = AutoMapper.Mapper.Map<List<Entidade.Unidade>, List<UnidadeViewModel>>(unidadeAplicacao.BuscarPor(x => x.Empresa.Id == IdEmpresa && x.Nome != null).ToList());
            return Json(unidades);
        }

        [HttpPost]
        public JsonResult BuscarAjusteFaturamento(ConsolidaAjusteFaturamentoViewModel consolidaAjusteFaturamentoVM)
        {
            string message = string.Empty;
            var tipo = TipoModal.Success;
            var prefeituraComplementarMaiorIgualDespesaF = false;
            var prefeituraMaiorIgualCartaoF = false;
            var despesaTotalF = string.Empty;
            var despesaValorFinalF = string.Empty;
            var faturamentoMesF = string.Empty;
            var faturamentoFinalF = string.Empty;
            var valorNotaEmissaoF = string.Empty;
            try
            {
                var consolidaAjusteFaturamentoDM = consolidaAjusteFaturamentoAplicacao.BuscarPor(x => x.Empresa.Id == consolidaAjusteFaturamentoVM.Empresa.Id)
                                                   .Where(x => x.Mes == (int)consolidaAjusteFaturamentoVM.Mes && x.Ano == consolidaAjusteFaturamentoVM.Ano).FirstOrDefault();
                if (consolidaAjusteFaturamentoVM.Unidade.Id != 0)
                {
                    consolidaAjusteFaturamentoDM = consolidaAjusteFaturamentoAplicacao.BuscarPor(x => x.Unidade.Id == consolidaAjusteFaturamentoVM.Unidade.Id)
                                                   .Where(x => x.Mes == (int)consolidaAjusteFaturamentoVM.Mes && x.Ano == consolidaAjusteFaturamentoVM.Ano).FirstOrDefault();
                }

                if (consolidaAjusteFaturamentoDM != null)
                {

                    var calculoFechamentoDM = BuscarCalculoFechamento(consolidaAjusteFaturamentoDM);
                    var calculoFechamentoVM = AutoMapper.Mapper.Map<CalculoFechamento, CalculoFechamentoViewModel>(calculoFechamentoDM);
                    calculoFechamentoVM.PrefeituraComplementarMaiorIgualDespesa = false;
                    calculoFechamentoVM.ValorComplementarEmitido = false;
                    despesaTotalF = calculoFechamentoVM.AjusteFaturamento.ConsolidaDespesa.DespesaTotal.ToString("F");
                    despesaValorFinalF = calculoFechamentoVM.AjusteFaturamento.ConsolidaDespesa.DespesaValorFinal.ToString("F");
                    faturamentoMesF = calculoFechamentoVM.AjusteFaturamento.ConsolidaFaturamento.FaturamentoMes.ToString("F");
                    faturamentoFinalF = calculoFechamentoVM.AjusteFaturamento.ConsolidaFaturamento.FaturamentoFinal.ToString("F");
                    valorNotaEmissaoF = calculoFechamentoVM.ValorNotaEmissao.ToString("F");

                    if (calculoFechamentoVM.AjusteFaturamento.ConsolidaFaturamento.FaturamentoMes >=
                        calculoFechamentoVM.AjusteFaturamento.ConsolidaFaturamento.FaturamentoCartao)
                    {
                        calculoFechamentoVM.PrefeituraMaiorIgualCartao = true;
                        prefeituraMaiorIgualCartaoF = true;
                    }
                    var valorTeste = calculoFechamentoVM.AjusteFaturamento.ConsolidaFaturamento.FaturamentoFinal + calculoFechamentoVM.ValorNotaEmissao;

                    if (valorTeste >= calculoFechamentoVM.AjusteFaturamento.ConsolidaDespesa.DespesaValorFinal)
                    {
                        calculoFechamentoVM.PrefeituraComplementarMaiorIgualDespesa = true;
                        prefeituraComplementarMaiorIgualDespesaF = true;
                    }


                    Session["CalculoFechamentoVM"] = calculoFechamentoVM;
                }

                else
                {
                    message = $"Não Possue dados com esses Filtros!";
                    tipo = TipoModal.Danger;
                }
            }
            catch (Exception ex)
            {

                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }

            return new JsonResult
            {
                Data = new
                {
                    message,
                    tipo = tipo.ToDescription(),
                    despesaTotalF,
                    despesaValorFinalF,
                    faturamentoMesF,
                    faturamentoFinalF,
                    valorNotaEmissaoF,
                    prefeituraComplementarMaiorIgualDespesaF,
                    prefeituraMaiorIgualCartaoF
                },
                ContentType = "application/json",
                MaxJsonLength = int.MaxValue
            };

        }

        private CalculoFechamento BuscarCalculoFechamento(ConsolidaAjusteFaturamento consolidaAjusteFaturamentoDM)
        {
            var calculoFechamentoDM = new CalculoFechamento();

            calculoFechamentoDM.AjusteFaturamento = consolidaAjusteFaturamentoDM;

            calculoFechamentoDM.ValorNotaEmissao = (consolidaAjusteFaturamentoDM.ConsolidaFaturamento.FaturamentoMes + consolidaAjusteFaturamentoDM.ConsolidaFaturamento.FaturamentoFinal);

            return calculoFechamentoDM;

        }

        [HttpPost]
        public ActionResult SalvarDados(CalculoFechamentoViewModel calculoFechamentoVM)
        {
            try
            {
                calculoFechamentoVM = (CalculoFechamentoViewModel)Session["CalculoFechamentoVM"];

                var calculoFechamentoDM = AutoMapper.Mapper.Map<CalculoFechamentoViewModel, CalculoFechamento>(calculoFechamentoVM);

                var teste = calculoFechamentoAplicacao.BuscarPor(x => x.ValorComplementarEmitido == calculoFechamentoDM.ValorComplementarEmitido)
                    .Where(x => x.AjusteFaturamento.Mes == calculoFechamentoDM.AjusteFaturamento.Mes
                        && x.AjusteFaturamento.Empresa.Id == calculoFechamentoDM.AjusteFaturamento.Empresa.Id
                        && x.AjusteFaturamento.Ano == calculoFechamentoDM.AjusteFaturamento.Ano).FirstOrDefault();

                if (teste !=null)
                {
                    DadosModal = new DadosModal
                    {
                        Titulo = "Atenção",
                        Mensagem = "Ja foi realizado o Fechamento com essas Informações",
                        TipoModal = TipoModal.Danger
                    };
                }
                else
                {
                    calculoFechamentoAplicacao.Salvar(calculoFechamentoDM);
                    ModelState.Clear();
                    DadosModal = new DadosModal
                    {
                        Titulo = "Sucesso",
                        Mensagem = "Registro salvo com sucesso",
                        TipoModal = TipoModal.Success
                    };
                }
                ViewBag.ListaUnidade = new List<UnidadeViewModel>();
                ViewBag.ListaAnos = new SelectList(Utils.UtilsServico.ListaAnosAnteriores(DateTime.Now.Year, 5));
                return View("Index");


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
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + new BusinessRuleException(ex.Message).Message,
                    TipoModal = TipoModal.Danger
                };
            }
            return View("Index");
        }

        [HttpPost]
        public void AtualizarValorComplementar()
        {

            var calculoFechamentoVM = (CalculoFechamentoViewModel)Session["CalculoFechamentoVM"];
            calculoFechamentoVM.ValorComplementarEmitido = true;

            Session["CalculoFechamentoVM"] = calculoFechamentoVM;
        }
    }
}