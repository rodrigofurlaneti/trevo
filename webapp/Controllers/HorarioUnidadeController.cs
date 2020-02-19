using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Portal.Controllers
{
    public class HorarioUnidadeController : GenericController<HorarioUnidade>
    {
        public List<HorarioUnidade> ListaHorarioUnidade => Aplicacao?.Buscar()?.ToList() ?? new List<HorarioUnidade>();
        public IEnumerable<ChaveValorViewModel> ListaTipoHorario => Aplicacao.BuscarValoresDoEnum<TipoHorario>();

        public List<SelectListItem> ListaPeriodoDia { get; set; }

        public IPeriodoHorarioAplicacao _periodoHorarioAplicacao;

        public List<UnidadeViewModel> ListaUnidade => this.unidadeAplicacao.ListaUnidade().Select(x => new UnidadeViewModel(x)).ToList();

        public List<PeriodoHorarioViewModel> ListaPeriodoHorario
        {
            get => (List<PeriodoHorarioViewModel>)Session["PeriodoHorarios"];
            set => Session["PeriodoHorarios"] = value;
        }

        private readonly IUnidadeAplicacao unidadeAplicacao;
        private readonly ITipoNotificacaoAplicacao _tipoNotificacaoAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IHorarioUnidadeAplicacao _horarioUnidadeAplicacao;

        public HorarioUnidadeController(IUnidadeAplicacao unidadeAplicacao,
                                        IHorarioUnidadeAplicacao HorarioUnidadeAplicacao,
                                        IPeriodoHorarioAplicacao periodoHorarioAplicacao,
                                        ITipoNotificacaoAplicacao tipoNotificacaoAplicacao,
                                        IUsuarioAplicacao usuarioAplicacao)
        {
            this.unidadeAplicacao = unidadeAplicacao;
            Aplicacao = HorarioUnidadeAplicacao;
            _periodoHorarioAplicacao = periodoHorarioAplicacao;
            _tipoNotificacaoAplicacao = tipoNotificacaoAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
            _horarioUnidadeAplicacao = HorarioUnidadeAplicacao;

            ListaPeriodoDia = new List<SelectListItem>();

            ListaPeriodoDia.Add(new SelectListItem { Value = "1", Text = "Manhã" });
            ListaPeriodoDia.Add(new SelectListItem { Value = "2", Text = "Tarde" });
            ListaPeriodoDia.Add(new SelectListItem { Value = "3", Text = "Noite" });
            ListaPeriodoDia.Add(new SelectListItem { Value = "4", Text = "Integral" });
            ListaPeriodoDia.Add(new SelectListItem { Value = "5", Text = "Madrugada" });
            ListaPeriodoDia.Add(new SelectListItem { Value = "6", Text = "Personalizado" });
        }

        // GET: RetiradaCofre
        public override ActionResult Index()
        {
            ListaPeriodoHorario = null;

            return View();
        }

        public override ActionResult Edit(int id)
        {
            var horarioUnidade = new HorarioUnidadeViewModel(Aplicacao.BuscarPorId(id));

            ListaPeriodoHorario = Helpers.PeriodoHorario.RetornaPeriodoHorarios(horarioUnidade.PeriodosHorario.ToList());

            return View("Index", horarioUnidade);
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(HorarioUnidadeViewModel model)
        {
            try
            {
                var horarioUnidade = Aplicacao.BuscarPorId(model.Id) ?? model.ToEntity();

                if (ListaPeriodoHorario != null)
                {
                    var periodoHorariosOrdenadosVM = ListaPeriodoHorario.OrderBy(x => x.TipoHorario).ToList();

                    var listaHorarioUnidade = new List<HorarioUnidadePeriodoHorario>();

                    foreach (var periodoHorarioVM in periodoHorariosOrdenadosVM)
                    {
                        var periodoHorario = new PeriodoHorario();
                        periodoHorario.Id = periodoHorarioVM.Id < 0 ? 0 : periodoHorarioVM.Id;
                        periodoHorario.Periodo = periodoHorarioVM.Periodo;
                        periodoHorario.Inicio = periodoHorarioVM.Inicio;
                        periodoHorario.Fim = periodoHorarioVM.Fim;
                        periodoHorario.TipoHorario = periodoHorarioVM.TipoHorario;

                        var objPeriodoHorarioCriado = _periodoHorarioAplicacao.SalvarComRetorno(periodoHorario);

                        var objHorarioUnidade = new HorarioUnidadePeriodoHorario();
                        objHorarioUnidade.DiasPeriodo = periodoHorarioVM.TipoHorario;
                        objHorarioUnidade.HorarioUnidade = horarioUnidade.Id;
                        objHorarioUnidade.PeriodoHorario = new PeriodoHorario();
                        objHorarioUnidade.PeriodoHorario = objPeriodoHorarioCriado;
                        listaHorarioUnidade.Add(objHorarioUnidade);
                    }

                    horarioUnidade.PeriodosHorario = listaHorarioUnidade;
                }

                _horarioUnidadeAplicacao.Salvar(horarioUnidade, UsuarioLogado.UsuarioId);

                ModelState.Clear();

                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro("Ocorreu um erro ao salvar: " + ex.Message);
            }

            return View("Index", model);
        }

        public JsonResult BuscarDadosDosGrids(int id)
        {
            var cliente = new HorarioUnidadeViewModel(Aplicacao.BuscarPorId(id));

            ListaPeriodoHorario = cliente?.PeriodosHorario.Select(x => x.PeriodoHorario).ToList();

            return Json(
                new
                {
                    veiculos = cliente?.PeriodosHorario.Select(x => x.PeriodoHorario).ToList()
                }
            );
        }

        [HttpPost]
        public void AdicionaTipoAtividade(string index, string TipoAtividade)
        {
            if (!string.IsNullOrEmpty(TipoAtividade))
            {

                var TipoAtividades = new List<PeriodoHorarioViewModel>();
                if (ListaPeriodoHorario != null)
                {
                    TipoAtividades = ListaPeriodoHorario;
                }

                if (int.Parse(index) < 0)
                {
                    TipoAtividades.AddRange(JsonConvert.DeserializeObject<List<PeriodoHorarioViewModel>>(TipoAtividade));
                }
                else
                {
                    TipoAtividades[int.Parse(index)] = JsonConvert.DeserializeObject<List<PeriodoHorarioViewModel>>(TipoAtividade).First();
                }
                ListaPeriodoHorario = TipoAtividades;
            }

        }

        [HttpPost]
        public void AtualizaTipoAtividades(string jsonTipoAtividades)
        {
            if (!string.IsNullOrEmpty(jsonTipoAtividades) && jsonTipoAtividades != "{}")
            {
                var TipoAtividades = JsonConvert.DeserializeObject<List<PeriodoHorarioViewModel>>(jsonTipoAtividades);

                ListaPeriodoHorario = TipoAtividades;
            }

        }

        [HttpPost]
        public void RemoveTipoAtividadeSessao(string index)
        {

            var TipoAtividades = new List<PeriodoHorarioViewModel>();
            if (ListaPeriodoHorario != null)
            {
                TipoAtividades = ListaPeriodoHorario;
                TipoAtividades.RemoveAt(int.Parse(index));
                ListaPeriodoHorario = TipoAtividades;
            }
        }

        [HttpPost]
        public JsonResult ListaAtividades(string horariounidade)
        {
            var TipoAtividades = new List<PeriodoHorarioViewModel>();

            if (!string.IsNullOrEmpty(horariounidade))
            {
                var CheckListAtividades = new List<HorarioUnidadeViewModel>();
                CheckListAtividades.AddRange(JsonConvert.DeserializeObject<List<HorarioUnidadeViewModel>>(horariounidade));

                var cliente = new HorarioUnidadeViewModel(Aplicacao.BuscarPorId(CheckListAtividades.First().Id));

                if (cliente != null && cliente.PeriodosHorario != null)
                {
                    TipoAtividades = Helpers.PeriodoHorario.RetornaPeriodoHorarios(cliente.PeriodosHorario.ToList());
                }
            }


            return Json(TipoAtividades);
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

        public JsonResult BuscarPeriodoHorarios()
        {
            return Json(ListaPeriodoHorario);
        }
    }
}