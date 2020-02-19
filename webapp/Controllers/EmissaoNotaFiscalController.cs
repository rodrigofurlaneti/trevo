using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Extensions;
using Entidade;
using Entidade.Base;
using Entidade.Uteis;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Portal.Controllers
{
    public class EmissaoNotaFiscalController : GenericController<Pagamento>
    {
        private readonly IUnidadeAplicacao unidadeAplicacao;
        private readonly IEmpresaAplicacao empresaUnidadeAplicacao;

        private readonly IPagamentoAplicacao pagamentoAplicacao;
        private readonly IParametroNumeroNotaFiscalAplicacao parametroNumeroNotaFiscalAplicacao;

        public List<EmpresaViewModel> ListaEmpresaUnidade => AutoMapper.Mapper.Map<List<Empresa>, List<EmpresaViewModel>>(empresaUnidadeAplicacao.Buscar().ToList());


        public EmissaoNotaFiscalController(IUnidadeAplicacao unidadeAplicacao,
                                           IEmpresaAplicacao empresaUnidadeAplicacao,
                                           IPagamentoAplicacao pagamentoAplicacao,
                                           IParametroNumeroNotaFiscalAplicacao parametroNumeroNotaFiscalAplicacao)
        {
            this.unidadeAplicacao = unidadeAplicacao;
            this.pagamentoAplicacao = pagamentoAplicacao;
            this.empresaUnidadeAplicacao = empresaUnidadeAplicacao;
            this.parametroNumeroNotaFiscalAplicacao = parametroNumeroNotaFiscalAplicacao;

            ViewBag.ListaMes = new SelectList(
              Enum.GetValues(typeof(Mes)).Cast<Mes>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
              "Id",
              "Descricao");
        }

        // GET: EmissaoNotaFiscal
        [CheckSessionOut]
        public override ActionResult Index()
        {

            ViewBag.ListaUnidade = new List<UnidadeViewModel>();
            ViewBag.ListaAnos = new SelectList(Utils.UtilsServico.ListaAnosAnteriores(DateTime.Now.Year, 5));

            return View("Index");
        }

        [HttpPost]
        public JsonResult BuscarUnidades(int IdEmpresa)
        {
            var unidades = AutoMapper.Mapper.Map<List<Entidade.Unidade>, List<UnidadeDTO>>(unidadeAplicacao.BuscarPor(x => x.Empresa.Id == IdEmpresa && x.Nome != null).ToList());

            return Json(unidades, JsonRequestBehavior.AllowGet);
        }

        public ActionResult buscardadospagamentos(EmissaoNotaFiscalViewModel consolidaAjusteFaturamentoVM)
        {

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
                        var pagamento = pagamentoAplicacao.BuscarPor(x => x.Unidade.Id == unidade.Id)
                                                                .Where(x => x.DataInsercao >= datainicio && x.DataInsercao <= datafim).ToList();
                        pagamentos.AddRange(pagamento);
                    }
                }

                consolidaAjusteFaturamentoVM.Pagamentos = pagamentos;

            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
                return PartialView("_GridEmissaoNotaFiscal", consolidaAjusteFaturamentoVM);
            }

            return PartialView("_GridEmissaoNotaFiscal", consolidaAjusteFaturamentoVM);

        }

        public ActionResult LiberarNotas(List<PagamentoViewModel> emissaoNotafiscal)
        {
            bool todosLiberados = true;

            try
            {


                if (emissaoNotafiscal == null)
                {
                    return Json(new Resultado<object>()
                    {
                        Sucesso = false,
                        TipoModal = TipoModal.Danger.ToDescription(),
                        Titulo = "Atenção",
                        Mensagem = "Nenhuma nota foi Selecionada"
                    }, JsonRequestBehavior.AllowGet);
                }
                  

                foreach (var item in emissaoNotafiscal)
                {
                    var emissaoNota = pagamentoAplicacao.BuscarPorId(item.Id);

                    var parametros = parametroNumeroNotaFiscalAplicacao.Buscar().ToList();

                    var parametrox = parametros.Where(x => x.Unidade.Id == emissaoNota.Unidade.Id).FirstOrDefault();

                    if (parametrox != null)
                    {
                        var valorMaximo = string.IsNullOrEmpty(parametrox.ValorMaximoNota) ? 0 : Convert.ToDecimal(parametrox.ValorMaximoNota);
                        var valorDiaMaximo = string.IsNullOrEmpty(parametrox.ValorMaximoNotaDia) ? 0 : Convert.ToDecimal(parametrox.ValorMaximoNotaDia);

                        if (emissaoNota.ValorPago > valorMaximo)
                        {
                            ModelState.Clear();

                            emissaoNota.StatusEmissao = StatusEmissao.MovimentoPendente;

                            todosLiberados = false;

                            pagamentoAplicacao.Salvar(Mapper.Map<Pagamento>(emissaoNota));

                            continue;

                        }

                        var listPagamento = pagamentoAplicacao.Buscar().ToList();

                        if (listPagamento != null & listPagamento.Count > 0)
                        {
                            var listaDia = listPagamento.Where(x => x.DataEnvio.Date == DateTime.Now.Date);
                            

                            if (listaDia != null && listaDia.Count() > 0)
                            {
                                var somaDoDia = listaDia.Sum(x => x.ValorPago);

                                if(somaDoDia > valorDiaMaximo)
                                {
                                    ModelState.Clear();

                                    emissaoNota.StatusEmissao = StatusEmissao.MovimentoPendente;

                                    todosLiberados = false;

                                    pagamentoAplicacao.Salvar(Mapper.Map<Pagamento>(emissaoNota));

                                    continue;
                                }
                            } 
                        }
                    }


                    emissaoNota.StatusEmissao = StatusEmissao.Liberado;

                    pagamentoAplicacao.Salvar(Mapper.Map<Pagamento>(emissaoNota));
                }
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Erro",
                    Mensagem = "Erro na tentativa de liberação !"
                }, JsonRequestBehavior.AllowGet);
            }

            ModelState.Clear();

            if (todosLiberados)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Success.ToDescription(),
                    Titulo = "Sucesso",
                    Mensagem = "Liberação realizado com sucesso."
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Uma ou mais Notas Possuem Valor não compatível com os parametros pré estabelecidos !"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [CheckSessionOut]
        public ActionResult ExecutarPagamentoModal()
        {

            try
            {

                return PartialView("_ModalEmissaoNotaFiscal");
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro na execução da ação: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
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