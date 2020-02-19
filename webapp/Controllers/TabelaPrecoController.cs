using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class TabelaPrecoController : GenericController<Preco>
    {
        public List<Preco> ListaTabelaPrecos => _precoAplicacao?.Buscar()?.ToList() ?? new List<Preco>();
        public List<PeriodoPreco> ListaPeriodoPreco { get; set; }
        private readonly IPrecoAplicacao _precoAplicacao;
        private readonly IPrecoStatusAplicacao _precoStatusAplicacao;
        private readonly ITipoNotificacaoAplicacao _tipoNotificacaoAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;

        public TabelaPrecoController(IPrecoAplicacao precoAplicacao,
                                    IPrecoStatusAplicacao precoStatusAplicacao,
                                    ITipoNotificacaoAplicacao tipoNotificacaoAplicacao,
                                    IUsuarioAplicacao usuarioAplicacao)
        {

            _precoAplicacao = precoAplicacao;
            _precoStatusAplicacao = precoStatusAplicacao;
            _tipoNotificacaoAplicacao = tipoNotificacaoAplicacao;
            _usuarioAplicacao = usuarioAplicacao;

            ViewBag.Funcionamentos = new SelectList(
                Enum.GetValues(typeof(PeriodoPreco)).Cast<PeriodoPreco>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
                "Id",
                "Descricao");

        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            Session["Mensalistas"] = "";
            Session["Alugueis"] = "";
            Session["Convenios"] = "";
            Session["Funcionamentos"] = "";
            return View();
        }

        [CheckSessionOut]
        public ActionResult AtualizarMensalistas(List<MensalistaViewModel> mensalistas)
        {
            Session["Mensalistas"] = mensalistas;

            return PartialView("_GridMensalistas", mensalistas);
        }

        [CheckSessionOut]
        public ActionResult AtualizarAlugueis(List<AluguelViewModel> alugueis)
        {
            //var teste = Convert.ToDecimal(alugueis.FirstOrDefault().Valor);
            Session["Alugueis"] = alugueis;
            return PartialView("~/Views/TabelaPreco/_GridAlugueis.cshtml", alugueis);
        }

        [CheckSessionOut]
        public ActionResult AtualizarConvenios(List<ConvenioViewModel> convenios)
        {
            Session["Convenios"] = convenios;
            return PartialView("~/Views/TabelaPreco/_GridConvenios.cshtml", convenios);
        }

        [CheckSessionOut]
        public void AtualizarFuncionamentos(FuncionamentoViewModel funcionamento)
        {
            if (Session["Funcionamentos"] == "")
            {
                var listaFuncionamentos = new List<FuncionamentoViewModel>();
                listaFuncionamentos.Add(funcionamento);
                Session["Funcionamentos"] = listaFuncionamentos;
            }
            else
            {
                var listaFuncionamento = (List<FuncionamentoViewModel>)Session["Funcionamentos"];
                var funcionamentoUnico = listaFuncionamento.Where(x => x.Nome == funcionamento.Nome);
                if (funcionamentoUnico.Count() > 0)
                {
                    if (funcionamento.Id != null)
                    {
                        FuncionamentoViewModel funcionamentoAdd = funcionamentoUnico.FirstOrDefault();
                        funcionamentoAdd.HorariosPrecos.Add(funcionamento.HorariosPrecos.Where(x => x.Id == null).LastOrDefault());
                        listaFuncionamento.Add(funcionamentoAdd);
                    }
                    else
                    {
                        listaFuncionamento.Remove(funcionamentoUnico.Single());
                        listaFuncionamento.Add(funcionamento);
                    }
                    
                    Session["Funcionamentos"] = listaFuncionamento;
                }
                else
                {
                    listaFuncionamento.Add(funcionamento);
                    Session["Funcionamentos"] = listaFuncionamento;
                }
            }
        }

        [CheckSessionOut]
        public ActionResult SalvarDados(PrecoViewModel preco)
        {
            try
            {
                var usuarioLogado = HttpContext.User as CustomPrincipal;
                var mensalistas = (Session["Mensalistas"] != "") ? (List<MensalistaViewModel>)Session["Mensalistas"] : new List<MensalistaViewModel>();
                var alugueis = (Session["Alugueis"] != "") ? (List<AluguelViewModel>)Session["Alugueis"] : new List<AluguelViewModel>();
                var convenios = (Session["Convenios"] != "") ? (List<ConvenioViewModel>)Session["Convenios"] : new List<ConvenioViewModel>();
                var funcionamentos = (Session["Funcionamentos"] != "") ? (List<FuncionamentoViewModel>)Session["Funcionamentos"] : new List<FuncionamentoViewModel>();
                var status = StatusFuncionario.Ativo;
                var precoStatus = _precoStatusAplicacao.BuscarPor(x => x.Id == 3).SingleOrDefault();

                //preco.Mensalistas = mensalistas;
                //preco.Alugueis = alugueis;
                preco.Funcionamentos = funcionamentos;
                preco.NomeUsuario = usuarioLogado.Nome;
                preco.DataInsercao = DateTime.Now;
                var precoEntity = AutoMapper.Mapper.Map<PrecoViewModel, Preco>(preco);

                _precoAplicacao.Salvar(precoEntity, usuarioLogado.UsuarioId);

                ModelState.Clear();

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

            return View("Index");
        }

        public ActionResult BuscarTabelaPreco()
        {
            var tabelasPrecos = AutoMapper.Mapper.Map<List<Preco>, List<PrecoViewModel>>(ListaTabelaPrecos);
            for (int i = 0; i < ListaTabelaPrecos.Count; i++)
                tabelasPrecos[i].PrecoStatus = ListaTabelaPrecos[i].PrecoStatus;

            return PartialView("_GridTabelaPrecos", tabelasPrecos);
        }

        public override ActionResult Edit(int id)
        {
            var tabelaPreco = AutoMapper.Mapper.Map<Preco, PrecoViewModel>(_precoAplicacao.BuscarPorId(id));
            //Session["Mensalistas"] = tabelaPreco.Mensalistas;
            //Session["Alugueis"] = tabelaPreco.Alugueis;
            Session["Funcionamentos"] = tabelaPreco.Funcionamentos;
            return View("Index", tabelaPreco);
        }

        [HttpPost]
        public JsonResult BuscarMensalistas()
        {
            var verificar = Session["Mensalistas"];
            var mensalistas = new List<MensalistaViewModel>();
            if (verificar != "")
                mensalistas = (List<MensalistaViewModel>)Session["Mensalistas"];

            return Json(mensalistas);
        }

        [HttpPost]
        public JsonResult BuscarAlugueis()
        {
            var verificar = Session["Alugueis"];
            var alugueis = new List<AluguelViewModel>();
            if (verificar != "")
                alugueis = (List<AluguelViewModel>)Session["Alugueis"];

            return Json(alugueis);
        }

        [HttpPost]
        public JsonResult BuscarConvenios()
        {
            var verificar = Session["Convenios"];
            var convenios = new List<ConvenioViewModel>();
            if (verificar != "")
                convenios = (List<ConvenioViewModel>)Session["Convenios"];

            return Json(convenios);
        }

        [HttpPost]
        public JsonResult BuscarFuncionamentos()
        {
            var verificar = Session["Funcionamentos"];
            var funcionamentos = new List<FuncionamentoViewModel>();
            if (verificar != "")
            {
                funcionamentos = (List<FuncionamentoViewModel>)Session["Funcionamentos"];
            }

            return Json(funcionamentos);
        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            Dictionary<string, object> jsonResult = new Dictionary<string, object>();
            try
            {
                _precoAplicacao.ExcluirPorId(id);

                ModelState.Clear();
                GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                jsonResult.Add("Status", "Error");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                var tipoModal = TipoModal.Danger;

                if (ex.Message.IndexOf("DELETE") > 0)
                {
                    message = "A unidade esta sendo usada em outras associações de tabelas do sistema";
                    tipoModal = TipoModal.Warning;
                }

                GerarDadosModal("Atenção", message, tipoModal);

                jsonResult.Add("Status", "Error");
            }

            return View("Index");
        }

    }
}