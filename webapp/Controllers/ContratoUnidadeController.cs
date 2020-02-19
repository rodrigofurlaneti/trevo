using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Portal.Controllers
{
    public class ContratoUnidadeController : GenericController<ContratoUnidade>
    {
        public List<UnidadeViewModel> ListaUnidade
        {
            get { return (List<UnidadeViewModel>)Session["ListaUnidade"] ?? new List<UnidadeViewModel>(); }
            set { Session["ListaUnidade"] = value; }
        }

        private readonly IContaPagarAplicacao _contapagarAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IContratoUnidadeContasAPagarAplicacao _contratoUnidadeContasAPagarAplicacao;

        public List<ContratoUnidade> ListaTiposEquipe => Aplicacao?.Buscar()?.ToList() ?? new List<ContratoUnidade>();

        public ContratoUnidadeAplicacao _contratounidadeAplicacao;

        public ContratoUnidadeController(IContratoUnidadeAplicacao ContratoUnidadeAplicacao,
                                         IUnidadeAplicacao unidadeAplicacao,
                                         IContaPagarAplicacao contapagarAplicacao,
                                         IContratoUnidadeContasAPagarAplicacao contratoUnidadeContasAPagarAplicacao)
        {
            Aplicacao = ContratoUnidadeAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _contapagarAplicacao = contapagarAplicacao;
            _contratoUnidadeContasAPagarAplicacao = contratoUnidadeContasAPagarAplicacao;
        }

        [HttpGet]
        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaUnidade = _unidadeAplicacao.ListaUnidade().Select(x => new UnidadeViewModel(x)).ToList();

            return View("Index");
        }

        [HttpPost]
        [CheckSessionOut]
        public ActionResult SalvarDados(ContratoUnidadeViewModel viewModel)
        {
            try
            {
                ContratoUnidade contratounidateEntity;
                contratounidateEntity = viewModel.ToEntity();

                List<int> excluirContas = new List<int>();
                if (viewModel.Id != 0)
                {
                    var retorno = Aplicacao.BuscarPorId(viewModel.Id);

                    foreach (var item in retorno.ContratoUnidadeContasAPagar)
                    {
                        excluirContas.Add(item.ContaAPagar.Id);
                    }
                }

                DateTime dtInicioContrato = viewModel.InicioContrato;
                DateTime dtFinalContrato = viewModel.FinalContrato;

               // string ajusteFinalFaturamento = consolidaAjusteFaturamentoVM.ConsolidaAjusteFinalFaturamento.AjusteFinalFaturamento.Replace(".", "");

                var valor = viewModel.Valor.Replace(".","");

                var entre = dtFinalContrato - dtInicioContrato;
                var qntParcelas = entre.Days / 30;
                var DiaVencimento = viewModel.DiaVencimento;

                contratounidateEntity.ContratoUnidadeContasAPagar = new List<ContratoUnidadeContasAPagar>();

                var model = new ContasAPagarViewModel();

                var tipoContrato = viewModel.TipoContrato;

                var diaVenc = DiaVencimento;
                var mesVenc = dtInicioContrato.Month;
                var anoVenc = dtInicioContrato.Year;

                var DataVencimento = new DateTime(anoVenc, mesVenc, diaVenc);

                if (tipoContrato == TipoContrato.Compra)
                {
                    var DataVencimentoCompra = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DiaVencimento);

                    var codigoAgrupador = DateTime.Now.ToString("yyyyMMddHHmmss");

                    var entity = new ContasAPagar();

                    entity.TipoPagamento = TipoContaPagamento.Etc;
                    entity.DataVencimento = DataVencimentoCompra.AddMonths(1);
                    entity.ValorTotal = Convert.ToDecimal(valor);
                    entity.NumeroParcela = 1;
                    entity.CodigoAgrupadorParcela = codigoAgrupador;
                    entity.StatusConta = StatusContasAPagar.EmAberto;
                    entity.Ativo = true;

                    var retorno = _contapagarAplicacao.SalvarComRetorno(entity);

                    var modelContratoContaPagar = new ContratoUnidadeContasAPagar();

                    modelContratoContaPagar.ContaAPagar = retorno;

                    contratounidateEntity.ContratoUnidadeContasAPagar.Add(modelContratoContaPagar);


                    Aplicacao.Salvar(contratounidateEntity);

                }
                else if (tipoContrato == TipoContrato.Aluguel)
                {
                    var codigoAgrupador = DateTime.Now.ToString("yyyyMMddHHmmss");
                    for (var i = 1; i <= qntParcelas; i++)
                    {
                        var entity = new ContasAPagar();

                        entity.TipoPagamento = TipoContaPagamento.Etc;
                        entity.DataVencimento = DataVencimento.AddMonths(i);
                        entity.ValorTotal = Convert.ToDecimal(valor);
                        entity.NumeroParcela = i;
                        entity.CodigoAgrupadorParcela = codigoAgrupador;
                        entity.StatusConta = StatusContasAPagar.EmAberto;
                        entity.Ativo = true;

                        var retorno = _contapagarAplicacao.SalvarComRetorno(entity);

                        var modelContratoContaPagar = new ContratoUnidadeContasAPagar();

                        modelContratoContaPagar.ContaAPagar = retorno;

                        contratounidateEntity.ContratoUnidadeContasAPagar.Add(modelContratoContaPagar);

                    }

                    Aplicacao.Salvar(contratounidateEntity);
                }
                else if (tipoContrato == TipoContrato.Parceria)
                {
                    var codigoAgrupador = DateTime.Now.ToString("yyyyMMddHHmmss");
                    for (var i = 1; i <= qntParcelas; i++)
                    {
                        var entity = new ContasAPagar();

                        entity.TipoPagamento = TipoContaPagamento.Etc;
                        entity.DataVencimento = DataVencimento.AddMonths(i);
                        entity.ValorTotal = Convert.ToDecimal(valor);
                        entity.NumeroParcela = i + 1;
                        entity.CodigoAgrupadorParcela = codigoAgrupador;
                        entity.StatusConta = StatusContasAPagar.EmAberto;
                        entity.Ativo = true;

                        var retorno = _contapagarAplicacao.SalvarComRetorno(entity);

                        var modelContratoContaPagar = new ContratoUnidadeContasAPagar();

                        modelContratoContaPagar.ContaAPagar = retorno;

                        contratounidateEntity.ContratoUnidadeContasAPagar.Add(modelContratoContaPagar);
                    }

                    Aplicacao.Salvar(contratounidateEntity);
                }

                if(excluirContas.Count > 0)
                {
                    foreach (var item in excluirContas)
                    {
                        _contapagarAplicacao.ExcluirPorId(item);
                    }
                }

                ModelState.Clear();

                GerarDadosModal("Sucesso", "Registro salvo com sucesso", TipoModal.Success);

                return View("Index");
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção",
                                br.Message,
                                TipoModal.Warning);
                return View("Index");
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção",
                                "Ocorreu um erro ao salvar: " + ex.Message,
                                TipoModal.Danger);
                return View("Index");
            }
        }

        [HttpGet]
        [CheckSessionOut]
        public ActionResult Editar(int id)
        {
            //var devedor = _devedorAplicacao.BuscarPor(x => x.Contratos.Any(c => c.Contrato.Id == id)).FirstOrDefault() ?? new Devedor();
            var model = Aplicacao.BuscarPorId(id); // devedor?.Contratos?.FirstOrDefault(c => c.Contrato.Id == id)?.Contrato ?? new Contrato();  // 
            //model.Devedor = devedor;

            model.Valor = Math.Round(model.Valor,2);

            var modelView = new ContratoUnidadeViewModel(model);
            modelView.displayValor = model.Valor.ToString("C");
            return View("Index", modelView);
        }

        [HttpGet]
        [CheckSessionOut]
        public ActionResult Deletar(int id)
        {
            try
            {
                GerarDadosModal("Remover registro",
                                "Deseja remover este registro?",
                                TipoModal.Danger,
                                "ConfirmarDelete",
                                "Sim, Desejo remover!",
                                id);


                return View("Index");

            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                return View("Index");
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
                return View("Index");
            }
        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            try
            {
                var contrato = Aplicacao.BuscarPorId(id);

                Aplicacao.Excluir(contrato);

                ModelState.Clear();

                GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);

                return View("Index");
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                return View("Index");
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
                return View("Index");
            }
        }
    }
}