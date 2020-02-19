using Aplicacao;
using Aplicacao.ViewModels;
using Core.Extensions;
using Entidade;
using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace Portal.Controllers
{
    public class ConsolidaAjusteFaturamentoController : GenericController<ConsolidaAjusteFaturamento>
    {

        private readonly IUnidadeAplicacao unidadeAplicacao;
        private readonly IContaPagarAplicacao contapagarAplicacao;
        private readonly IRecebimentoAplicacao recebimentoAplicacao;
        private readonly ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao;
        private readonly IPagamentoAplicacao pagamentoAplicacao;
        private readonly IConsolidaAjusteFaturamentoAplicacao consolidaAjusteFaturamentoAplicacao;
        private readonly IEmpresaAplicacao empresaUnidadeAplicacao;

        public ConsolidaAjusteFaturamentoController(IUnidadeAplicacao unidadeAplicacao, IContaPagarAplicacao contapagarAplicacao, 
                                                    IRecebimentoAplicacao recebimentoAplicacao, ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao
                                                    ,IPagamentoAplicacao pagamentoAplicacao,IConsolidaAjusteFaturamentoAplicacao consolidaAjusteFaturamentoAplicacao
                                                    , IEmpresaAplicacao empresaUnidadeAplicacao)
        {
            this.unidadeAplicacao = unidadeAplicacao;
            this.contapagarAplicacao = contapagarAplicacao;
            this.recebimentoAplicacao = recebimentoAplicacao;
            this.lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
            this.pagamentoAplicacao = pagamentoAplicacao;
            this.consolidaAjusteFaturamentoAplicacao = consolidaAjusteFaturamentoAplicacao;
            this.empresaUnidadeAplicacao = empresaUnidadeAplicacao;

            ViewBag.ListaMes = new SelectList(
              Enum.GetValues(typeof(Mes)).Cast<Mes>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
              "Id",
              "Descricao");

            ViewBag.ListaAnos = new SelectList(Utils.UtilsServico.ListaAnosAnteriores(DateTime.Now.Year, 5));

        }

        //public List<UnidadeViewModel> ListaUnidade => AutoMapper.Mapper.Map<List<Entidade.Unidade>, List<UnidadeViewModel>>(unidadeAplicacao.Buscar().Where(x => x.Precos != null).ToList());

        //public List<UnidadeViewModel> ListaUnidade => new List<UnidadeViewModel>();

        public List<EmpresaViewModel> ListaEmpresaUnidade => AutoMapper.Mapper.Map<List<Empresa>, List<EmpresaViewModel>>(empresaUnidadeAplicacao.Buscar().ToList());
        [CheckSessionOut]
        public override ActionResult Index()
        {
            ViewBag.ListaUnidade = new List<UnidadeViewModel>();
            ViewBag.ListaAnos = new SelectList(Utils.UtilsServico.ListaAnosAnteriores(DateTime.Now.Year,5));

            return View();
        }

        [HttpPost]
        public JsonResult BuscarUnidades (int IdEmpresa)
        {
            var unidades = AutoMapper.Mapper.Map<List<Entidade.Unidade>, List<UnidadeDTO>>(unidadeAplicacao.BuscarPor(x => x.Empresa.Id == IdEmpresa && x.Nome != null).ToList());
            return Json(unidades);
        }




        public JsonResult BuscarDadosDespesa(ConsolidaAjusteFaturamentoViewModel consolidaAjusteFaturamentoVM)
        {
            string message = string.Empty;
            var tipo = TipoModal.Success;
            var divGridColaboradorOrigem = string.Empty;
            var divGridColaboradorDestino = string.Empty;
            decimal despesaTotal = 0;
            decimal despesaFixa = 0;
            decimal despesaEscolhida = 0;
            decimal despesaEscolhidaFixa = 0;
            decimal despesaValorFinal = 0;
            int id = 0;

            string despesaTotalF =string.Empty;
            string despesaFixaF = string.Empty;
            string despesaEscolhidaF = string.Empty;
            string despesaEscolhidaFixaF = string.Empty;
            string despesaValorFinalF = string.Empty;

            try
            {
              
                consolidaAjusteFaturamentoVM.Unidade = AutoMapper.Mapper.Map<Entidade.Unidade, UnidadeViewModel>(unidadeAplicacao.BuscarPorId(consolidaAjusteFaturamentoVM.Unidade.Id));
                consolidaAjusteFaturamentoVM.Empresa = AutoMapper.Mapper.Map<Entidade.Empresa, EmpresaViewModel>(empresaUnidadeAplicacao.BuscarPorId(consolidaAjusteFaturamentoVM.Empresa.Id));

                despesaTotal = Math.Round(contapagarAplicacao.CalcularDespesaTotal(consolidaAjusteFaturamentoVM),2);
                despesaFixa = Math.Round(contapagarAplicacao.CalcularDespesaFixa(consolidaAjusteFaturamentoVM),2);
                despesaEscolhida = Math.Round(contapagarAplicacao.CalcularDespesaEscolhida(consolidaAjusteFaturamentoVM),2);
                despesaEscolhidaFixa = despesaFixa + despesaEscolhida;

                despesaTotalF = string.Format(new CultureInfo("pt-BR"), "{0:C}", Convert.ToString(despesaTotal));
                despesaFixaF = string.Format(new CultureInfo("pt-BR"), "{0:C}", Convert.ToString(despesaFixa));
                despesaEscolhidaF = string.Format(new CultureInfo("pt-BR"), "{0:C}", Convert.ToString(despesaEscolhida));
                despesaEscolhidaFixaF = string.Format(new CultureInfo("pt-BR"), "{0:C}", Convert.ToString(despesaEscolhidaFixa));
                despesaValorFinalF = despesaEscolhidaFixaF;

                id = BuscarId(consolidaAjusteFaturamentoVM);



            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }


            return new JsonResult { Data = new {message, tipo = tipo.ToDescription(), despesaTotalF, despesaFixaF,despesaEscolhidaF,despesaEscolhidaFixaF, despesaValorFinalF,id}, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        private int BuscarId(ConsolidaAjusteFaturamentoViewModel consolidaAjusteFaturamentoVM)
        {
            int ret = 0;

            var consolidaAjusteFaturamentoDM = new ConsolidaAjusteFaturamento();


            if (consolidaAjusteFaturamentoVM.Unidade != null)
                consolidaAjusteFaturamentoDM = consolidaAjusteFaturamentoAplicacao.BuscarPor(x => x.Unidade.Id == consolidaAjusteFaturamentoVM.Unidade.Id)
                                                 .Where(x => x.Mes == (int)consolidaAjusteFaturamentoVM.Mes && x.Ano == consolidaAjusteFaturamentoVM.Ano).FirstOrDefault();
            else
                consolidaAjusteFaturamentoDM = consolidaAjusteFaturamentoAplicacao.BuscarPor(x => x.Empresa.Id == consolidaAjusteFaturamentoVM.Empresa.Id)
                                                  .Where(x => x.Mes == (int)consolidaAjusteFaturamentoVM.Mes && x.Ano == consolidaAjusteFaturamentoVM.Ano).FirstOrDefault();

            

            if (consolidaAjusteFaturamentoDM != null) return consolidaAjusteFaturamentoDM.Id;

            return ret;
        }


        public JsonResult BuscarDadosFaturamento(ConsolidaAjusteFaturamentoViewModel consolidaAjusteFaturamentoVM)
        {
            string message = string.Empty;
            var tipo = TipoModal.Success;
            decimal faturamentoMes = 0;
            decimal faturamentoCartao = 0;
            decimal diferenca = 0;
            decimal faturamentoFinal = 0;

            string faturamentoMeslF = string.Empty;
            string faturamentoCartaoF = string.Empty;
            string diferencaF = string.Empty;
            string faturamentoFinalF = string.Empty;

            List<Pagamento> faturamentosCartao = new List<Pagamento>();
            List<Pagamento> faturamentosFinal = new List<Pagamento>();

            DateTime datainicio = new DateTime(consolidaAjusteFaturamentoVM.Ano, (int)consolidaAjusteFaturamentoVM.Mes, 1);
            DateTime datafim = new DateTime(consolidaAjusteFaturamentoVM.Ano, (int)consolidaAjusteFaturamentoVM.Mes, 
                                            DateTime.DaysInMonth(consolidaAjusteFaturamentoVM.Ano, (int)consolidaAjusteFaturamentoVM.Mes));


            try
            {

                var pagamentos = new List<Pagamento>();
                if (consolidaAjusteFaturamentoVM.Unidade.Id != 0)
                {
                    pagamentos = pagamentoAplicacao.BuscarPor(x => x.Unidade.Id == consolidaAjusteFaturamentoVM.Unidade.Id)
                                                                .Where(x => x.DataInsercao >= datainicio && x.DataInsercao <= datafim).ToList();
                }
                else
                {
                    var unidades = unidadeAplicacao.BuscarPor(x => x.Empresa.Id == consolidaAjusteFaturamentoVM.Empresa.Id).ToList();
                    foreach (var unidade in unidades)
                    {
                        var pagamento  = pagamentoAplicacao.BuscarPor(x => x.Unidade.Id == unidade.Id)
                                                                .Where(x => x.DataInsercao >= datainicio && x.DataInsercao <= datafim).ToList();
                        pagamentos.AddRange(pagamento);
                    }
                }

                

                foreach (var pagamento in pagamentos)
                {
                    if (pagamento.FormaPagamento == FormaPagamento.Credito)
                        faturamentosCartao.Add(pagamento);
                    else
                        faturamentosFinal.Add(pagamento);
                }

                faturamentoMes = faturamentosCartao.Sum(x => x.ValorPago);
                faturamentoCartao = faturamentosFinal.Sum(x => x.ValorPago);

                if (faturamentoMes > faturamentoCartao)
                    diferenca =  - faturamentoCartao;
                else
                    diferenca = faturamentoCartao - faturamentoMes;


                faturamentoMeslF = faturamentoMes.ToString("F");
                faturamentoCartaoF = faturamentoCartao.ToString("F");
                diferencaF = diferenca.ToString("F");
                faturamentoFinalF = faturamentoMeslF;

            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }


            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), faturamentoMeslF, faturamentoCartaoF, diferencaF, faturamentoFinalF }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }



        public ActionResult EnviarDadosDespesa(ConsolidaAjusteFaturamentoViewModel consolidaAjusteFaturamentoVM)
        {
            string message = string.Empty;
            var tipo = TipoModal.Success;
            var divDespesa = string.Empty;

            try
            {
                Session["consolidaAjusteFaturamentoVM"] = consolidaAjusteFaturamentoVM;
            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;

            }

           
            divDespesa = "OK";

            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), divDespesa }, ContentType = "application/json", MaxJsonLength = int.MaxValue };

        }


        public ActionResult EnviarDadosFaturamento(ConsolidaAjusteFaturamentoViewModel consolidaAjusteFaturamentoVM)
        {
            string message = string.Empty;
            var tipo = TipoModal.Success;
            var divDespesa = string.Empty;

            try
            {
                Session["consolidaAjusteFaturamentoVM"] = consolidaAjusteFaturamentoVM;
            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;

            }


            divDespesa = "OK";

            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), divDespesa }, ContentType = "application/json", MaxJsonLength = int.MaxValue };

        }

        private bool testeSalvarDados()
        {
            var consolidaAjusteFaturamentoVM = new ConsolidaAjusteFaturamentoViewModel();

            var ConsolidaDespesa = new ConsolidaDespesaViewModel();
            var ConsolidaFaturamento = new ConsolidaFaturamentoViewModel();
            var ConsolidaAjusteFinalFaturamento = new ConsolidaAjusteFinalFaturamentoViewModel();

            consolidaAjusteFaturamentoVM.ConsolidaDespesa = ConsolidaDespesa;
            consolidaAjusteFaturamentoVM.ConsolidaFaturamento = ConsolidaFaturamento;
            consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento = ConsolidaAjusteFinalFaturamento;



            consolidaAjusteFaturamentoVM.ConsolidaDespesa.DespesaTotal = 5000;
            consolidaAjusteFaturamentoVM.ConsolidaDespesa.DespesaEscolhida = 2000;
            consolidaAjusteFaturamentoVM.ConsolidaDespesa.DespesaEscolhidaFixa = 1000;
            consolidaAjusteFaturamentoVM.ConsolidaDespesa.DespesaFixa = 3000;
            consolidaAjusteFaturamentoVM.ConsolidaDespesa.DespesaValorFinal = 4000;

            consolidaAjusteFaturamentoVM.ConsolidaFaturamento.DataInsercao = DateTime.Now;
            consolidaAjusteFaturamentoVM.ConsolidaFaturamento.Diferenca = 2000;
            consolidaAjusteFaturamentoVM.ConsolidaFaturamento.FaturamentoCartao = 4000;
            consolidaAjusteFaturamentoVM.ConsolidaFaturamento.FaturamentoMes = 6000;
            consolidaAjusteFaturamentoVM.ConsolidaFaturamento.FaturamentoFinal = 5000;

            consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento.DespesaFinal = 4000;
            consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento.FaturamentoFinal = 5000;
            consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento.Diferenca = 1000;
            //consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento.AjusteFinalFaturamento = 3000;

            var unidade = new UnidadeViewModel();

            unidade.Id = 42;

            consolidaAjusteFaturamentoVM.Mes = Mes.Junho;
            consolidaAjusteFaturamentoVM.Unidade = unidade;

            Session["consolidaAjusteFaturamentoVM"] = consolidaAjusteFaturamentoVM;



            var consolidaAjusteFaturamentoVMSession = (ConsolidaAjusteFaturamentoViewModel)Session["consolidaAjusteFaturamentoVM"];

            var consolidaAjusteFaturamentoDM = AutoMapper.Mapper.Map<ConsolidaAjusteFaturamentoViewModel, ConsolidaAjusteFaturamento>(consolidaAjusteFaturamentoVM);

            var ConsolidaAjusteFinalFaturamentoDM = new ConsolidaAjusteFinalFaturamento();


            ConsolidaAjusteFinalFaturamentoDM.AjusteFinalFaturamento = consolidaAjusteFaturamentoDM.ConsolidaAjusteFinalFaturamento.AjusteFinalFaturamento;
            ConsolidaAjusteFinalFaturamentoDM.DataInsercao = DateTime.Now;
            ConsolidaAjusteFinalFaturamentoDM.DespesaFinal = consolidaAjusteFaturamentoDM.ConsolidaAjusteFinalFaturamento.DespesaFinal;
            ConsolidaAjusteFinalFaturamentoDM.Diferenca = consolidaAjusteFaturamentoDM.ConsolidaAjusteFinalFaturamento.Diferenca;
            ConsolidaAjusteFinalFaturamentoDM.FaturamentoFinal = consolidaAjusteFaturamentoDM.ConsolidaAjusteFinalFaturamento.FaturamentoFinal;



            consolidaAjusteFaturamentoDM.ConsolidaAjusteFinalFaturamento = ConsolidaAjusteFinalFaturamentoDM;
            consolidaAjusteFaturamentoDM.Unidade = AutoMapper.Mapper.Map<UnidadeViewModel, Entidade.Unidade>(consolidaAjusteFaturamentoVM.Unidade);
            consolidaAjusteFaturamentoDM.Mes = (int)consolidaAjusteFaturamentoVM.Mes;

            consolidaAjusteFaturamentoAplicacao.Salvar(consolidaAjusteFaturamentoDM);

            return true;

        } 

 


        public ActionResult SalvarDados(ConsolidaAjusteFaturamentoViewModel consolidaAjusteFaturamentoVM)
        {
            try
            {
                var consolidaAjusteFaturamentoVMSession = (ConsolidaAjusteFaturamentoViewModel)Session["consolidaAjusteFaturamentoVM"];

                var consolidaAjusteFaturamentoDM = AutoMapper.Mapper.Map<ConsolidaAjusteFaturamentoViewModel, ConsolidaAjusteFaturamento>(consolidaAjusteFaturamentoVMSession);

                //consolidaAjusteFaturamentoDM.Empresa = AutoMapper.Mapper.Map<EmpresaViewModel, Entidade.Empresa>(empresaUnidadeAplicacao.BuscarPorId(consolidaAjusteFaturamentoVMSession.Empresa.Id));

                //var empresa = empresaUnidadeAplicacao.BuscarPorId(empresaUnidadeAplicacao.BuscarPorId(consolidaAjusteFaturamentoVMSession.Empresa.Id);

                consolidaAjusteFaturamentoDM.Empresa = empresaUnidadeAplicacao.BuscarPorId(consolidaAjusteFaturamentoVMSession.Empresa.Id);

                var consolistaTeste = consolidaAjusteFaturamentoAplicacao.BuscarPorId(consolidaAjusteFaturamentoVMSession.Id);

                var ConsolidaAjusteFinalFaturamentoDM = new ConsolidaAjusteFinalFaturamento();

                string ajusteFinalFaturamento = consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento.AjusteFinalFaturamento.Replace(".", "");


                ConsolidaAjusteFinalFaturamentoDM.AjusteFinalFaturamento = Convert.ToDecimal(ajusteFinalFaturamento);
                ConsolidaAjusteFinalFaturamentoDM.DataInsercao = DateTime.Now;
                ConsolidaAjusteFinalFaturamentoDM.DespesaFinal = consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento.DespesaFinal;
                ConsolidaAjusteFinalFaturamentoDM.Diferenca = consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento.Diferenca;
                ConsolidaAjusteFinalFaturamentoDM.FaturamentoFinal = consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento.FaturamentoFinal;



                consolidaAjusteFaturamentoDM.ConsolidaAjusteFinalFaturamento = ConsolidaAjusteFinalFaturamentoDM;

                //consolidaAjusteFaturamentoDM.Unidade = AutoMapper.Mapper.Map<UnidadeViewModel, Entidade.Unidade>(consolidaAjusteFaturamentoVM.Unidade);
                //consolidaAjusteFaturamentoDM.Mes =  Utils.UtilsServico.RetornarNumeroMes(consolidaAjusteFaturamentoVM.Mes);

                if (consolistaTeste != null)
                {
                    consolidaAjusteFaturamentoDM.ConsolidaFaturamento.Id = consolistaTeste.ConsolidaFaturamento.Id;
                    consolidaAjusteFaturamentoDM.ConsolidaDespesa.Id = consolistaTeste.ConsolidaDespesa.Id;
                    consolidaAjusteFaturamentoDM.ConsolidaAjusteFinalFaturamento.Id = consolistaTeste.ConsolidaDespesa.Id;
                }

                consolidaAjusteFaturamentoAplicacao.Salvar(consolidaAjusteFaturamentoDM);

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

            ViewBag.ListaUnidade = new List<UnidadeViewModel>();
            ViewBag.ListaAnos = new SelectList(Utils.UtilsServico.ListaAnosAnteriores(DateTime.Now.Year, 5));

            return View("Index");
        }


        private class UnidadeDTO : BaseEntity
        {
            public UnidadeDTO()
            {

            }

            public virtual string Nome { get; set; }
        }


    }
}