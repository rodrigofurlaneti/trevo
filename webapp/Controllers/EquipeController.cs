using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class EquipeController : GenericController<Equipe>
    {
        private readonly IUnidadeAplicacao unidadeAplicacao;
        private readonly ITipoEquipeAplicacao tipoEquipeAplicacao;
        private readonly IFuncionamentoAplicacao funcionamentoAplicacao;
        private readonly IFuncionarioAplicacao funcionarioAplicacao;
        private readonly IColaboradorAplicacao colaboradorAplicacao;
        private readonly IPeriodoHorarioAplicacao periodoHorarioAplicacao;
        private readonly IEquipeAplicacao equipeAplicacao;
        private readonly IHorarioUnidadeAplicacao horarioUnidadeAplicacao;
        private readonly IRemanejamentoAplicacao remanejamentoAplicacao;

        public EquipeController(IUnidadeAplicacao unidadeAplicacao
                                 , ITipoEquipeAplicacao tipoEquipeAplicacao
                                 , IFuncionamentoAplicacao funcionamentoAplicacao
                                 , IFuncionarioAplicacao funcionarioAplicacao
                                 , IColaboradorAplicacao colaboradorAplicacao
                                 , IPeriodoHorarioAplicacao periodoHorarioAplicacao
                                 , IEquipeAplicacao equipeAplicacao
                                 , IHorarioUnidadeAplicacao horarioUnidadeAplicacao
                                 , IRemanejamentoAplicacao remanejamentoAplicacao)
        {
            this.unidadeAplicacao = unidadeAplicacao;
            this.tipoEquipeAplicacao = tipoEquipeAplicacao;
            this.funcionamentoAplicacao = funcionamentoAplicacao;
            this.funcionarioAplicacao = funcionarioAplicacao;
            this.colaboradorAplicacao = colaboradorAplicacao;
            this.periodoHorarioAplicacao = periodoHorarioAplicacao;
            this.equipeAplicacao = equipeAplicacao;
            this.horarioUnidadeAplicacao = horarioUnidadeAplicacao;
            this.remanejamentoAplicacao = remanejamentoAplicacao;
            ListaHorarios = new List<FuncionamentoViewModel>();
            typeItemList = new List<SelectListItem>();
            listaTurnos = new List<SelectList>();
        }

        public List<Equipe> ListaEquipes => equipeAplicacao?.Buscar()?.ToList() ?? new List<Equipe>();
        public List<UnidadeViewModel> ListaUnidade => AutoMapper.Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(unidadeAplicacao.Buscar().ToList());
        public List<TipoEquipeViewModel> ListaTipoEquipe => AutoMapper.Mapper.Map<List<TipoEquipe>, List<TipoEquipeViewModel>>(tipoEquipeAplicacao.Buscar().ToList());
        public List<FuncionarioViewModel> ListaSupervisor => AutoMapper.Mapper.Map<List<Funcionario>, List<FuncionarioViewModel>>(funcionarioAplicacao.BuscarPor(x => x.Cargo.Id == (int)CargoFuncionario.Supervisor).ToList());
        public List<FuncionarioViewModel> ListaEncarregado => AutoMapper.Mapper.Map<List<Funcionario>, List<FuncionarioViewModel>>(funcionarioAplicacao.BuscarPor(x => x.Cargo.Id == (int)CargoFuncionario.Encarregado).ToList());
        public List<FuncionamentoViewModel> ListaHorarios { get; set; }
        public List<SelectListItem> typeItemList { get; set; }
        public List<SelectList> listaTurnos { get; set; }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            
            ViewBag.TurnoId = new SelectList(ListarPeriodos(), "Id", "Periodo");
            Session["Colaboradores"] = null;
            Session["Horarios"] = "";
            Session["idEquipe"] = null;
            ListaHorarios = new List<FuncionamentoViewModel>();
            SetarViewBag();
            return View();
        }

        private List<PeriodoHorario> ListarPeriodos()
        {

            var periodos = periodoHorarioAplicacao.Buscar().ToList().OrderBy(b => b.Periodo);

            var periodosFM = new List<PeriodoHorario>();

            foreach (var item in periodos)
            {
                var periodo = new PeriodoHorario();

                periodo.Id = item.Id;
                periodo.Periodo = item.Periodo + " - " + item.Inicio + " as " + item.Fim;

                periodosFM.Add(periodo);
            }

            return periodosFM;
        }


        [HttpPost]
        public JsonResult BuscarColaboradores()
        {
            SetarViewBag();
            var verificar = Session["Colaboradores"];
            var colaboradores = new List<ColaboradorViewModel>();
            if (verificar != "")
                colaboradores = (List<ColaboradorViewModel>)Session["Colaboradores"];

            return Json(colaboradores);
        }

        public ActionResult AtualizarColaboradores(List<ColaboradorViewModel> colaboradores)
        {
            SetarViewBag();

            if (colaboradores != null)
            {
                foreach (var colaborador in colaboradores)
                {
                    if (colaborador.IdTurno != null)
                    {
                        colaborador.Turno = AutoMapper.Mapper.Map<PeriodoHorario, PeriodoHorarioViewModel>(periodoHorarioAplicacao.BuscarPorId(Convert.ToInt32(colaborador.IdTurno)));
                        colaborador.Turno.Periodo = BuscarPeriodoFormatado(colaborador.Turno.Periodo, colaborador.Turno.Inicio, colaborador.Turno.Fim);
                    }
                       

                    if (colaborador.IdColaborador != null)
                        colaborador.NomeColaborador = AutoMapper.Mapper.Map<Funcionario, FuncionarioViewModel>(funcionarioAplicacao.BuscarPorId(Convert.ToInt32(colaborador.IdColaborador)));
                        

                }
            }

            Session["Colaboradores"] = colaboradores;
            ViewBag.TurnoId = new SelectList(ListarPeriodos(), "Id", "Periodo");

            return PartialView("_GridColaboradores", colaboradores);
        }

        [HttpPost]
        public JsonResult BuscarHorarioTrabalho(int idUnidade)
        {
            var teste = horarioUnidadeAplicacao.BuscarPor(x => x.Unidade.Id == idUnidade).FirstOrDefault();

            if (teste != null)
            {
                if (teste.ListaTipoHorario.Contains(1))
                    typeItemList.Add(new SelectListItem { Value = "1", Text = "Segunda à Sexta" });

                if (teste.ListaTipoHorario.Contains(2))
                    typeItemList.Add(new SelectListItem { Value = "2", Text = "Sábado" });

                if (teste.ListaTipoHorario.Contains(3))
                    typeItemList.Add(new SelectListItem { Value = "3", Text = "Domingo" });

                if (teste.ListaTipoHorario.Contains(4))
                    typeItemList.Add(new SelectListItem { Value = "4", Text = "Feriado" });

            }

            int valorHorarioTrabalho = 0;

            if (Session["idEquipe"] != null)
            {
                var Equipe = equipeAplicacao.BuscarPorId(Convert.ToInt32(Session["idEquipe"]));

                valorHorarioTrabalho = (int)Equipe.TipoHorario;
            }
            
            return new JsonResult { Data = new { typeItemList, valorHorarioTrabalho }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }
        [HttpPost]
        public JsonResult AtualizarHorarioTrabalho(int valorHorarioTrabalho, int unidadeId)
        {
            if (unidadeId == null || unidadeId == 0)
            {
                var unidade = (Unidade)Session["Unidade"];
                if (unidade != null)
                    unidadeId = unidade.Id;
            }
            var teste = horarioUnidadeAplicacao.BuscarPor(x => x.Unidade.Id == unidadeId).FirstOrDefault();

            if (teste != null)
            {
                HorarioUnidadeViewModel teste3 = AutoMapper.Mapper.Map<HorarioUnidade, HorarioUnidadeViewModel>(teste);

                var teste2 = Helpers.PeriodoHorario.RetornaPeriodoHorarios(teste3.PeriodosHorario.ToList());

                var retorno = teste2.Where(x => x.TipoHorario == valorHorarioTrabalho).ToList();

                var turnos = new SelectList(retorno.ToList(), "Id", "Periodo");



                Session["Horarios"] = valorHorarioTrabalho;

                return Json(turnos);
            }
            else
            {
                return Json(null);
            }
            //Session["Horarios"] = AutoMapper.Mapper.Map<Funcionamento, FuncionamentoViewModel>(funcionamentoAplicacao.BuscarPorId(idHOrarioTrabalho));
        }

        [HttpPost]
        public JsonResult SelecionarHorarioTrabalho()
        {
            //var horarioSelecionado = (FuncionamentoViewModel)Session["Horarios"];

            //return Json(horarioSelecionado.Id);
            return Json((int)Session["Horarios"]);
        }
        [CheckSessionOut]
        public ActionResult SalvarDados(EquipeViewModel equipe)
        {

            try
            {

                var colaboradoresVM = (List<ColaboradorViewModel>)Session["Colaboradores"];

                if (colaboradoresVM != null)
                {
                    foreach (var colaborador in colaboradoresVM)
                    {
                        if (colaborador.NomeColaborador != null)
                            colaborador.NomeColaborador = AutoMapper.Mapper.Map<Funcionario, FuncionarioViewModel>(funcionarioAplicacao.BuscarPorId(Convert.ToInt32(colaborador.IdColaborador)));

                        colaborador.Turno = AutoMapper.Mapper.Map<PeriodoHorario, PeriodoHorarioViewModel>(periodoHorarioAplicacao.BuscarPorId(Convert.ToInt32(colaborador.IdTurno)));
                    }
                }
                //equipe.Colaboradores = colaboradoresVM;
                var equipesColaborador = new List<EquipeColaborador>();
                var colaboradorDM = AutoMapper.Mapper.Map<List<ColaboradorViewModel>, List<Colaborador>>(colaboradoresVM);

                foreach (var colaborador in colaboradorDM)
                {
                    if (colaborador.Turno == null)
                    {
                        var objTurno = colaboradorAplicacao.BuscarPorId(colaborador.Id);

                        if (objTurno != null && objTurno.Turno != null)
                            colaborador.Turno = objTurno.Turno;

                        var objNomeColaborador = colaboradorAplicacao.BuscarPorId(colaborador.Id);

                        if (objNomeColaborador != null && objNomeColaborador.NomeColaborador != null)
                            colaborador.NomeColaborador = objNomeColaborador.NomeColaborador;
                    }
                    var equipeColahorador = new EquipeColaborador();
                    equipeColahorador.Colaborador = colaborador;
                    equipesColaborador.Add(equipeColahorador);
                }

                var equipeEntity = AutoMapper.Mapper.Map<EquipeViewModel, Equipe>(equipe);
                //var horarioVM = (FuncionamentoViewModel)Session["Horarios"];
                //var horarioDM = AutoMapper.Mapper.Map<FuncionamentoViewModel, Funcionamento>(horarioVM);
                //equipeEntity.HorarioTrabalho = horarioDM;

                Equipe equipeAntes;
                if (equipe.Id != 0)
                {
                    equipeAntes = equipeAplicacao.BuscarPorId(equipe.Id);
                    equipeEntity.ParametrosEquipe = equipeAntes.ParametrosEquipe;
                }

                int valorTipoorario = Convert.ToInt32(Session["Horarios"]);



                if (valorTipoorario == 1)
                    equipeEntity.TipoHorario = TipoHorario.SegundaASexta;
                else if (valorTipoorario == 2)
                    equipeEntity.TipoHorario = TipoHorario.Sabado;
                else if (valorTipoorario == 3)
                    equipeEntity.TipoHorario = TipoHorario.Domingo;
                else
                    equipeEntity.TipoHorario = TipoHorario.Feriado;


                equipeEntity.Supervisor = funcionarioAplicacao.BuscarPorId(Convert.ToInt32(equipe.IdSupervisor));
                equipeEntity.Encarregado = funcionarioAplicacao.BuscarPorId(Convert.ToInt32(equipe.IdEncarregado));
                equipeEntity.Colaboradores = equipesColaborador;
                equipeAplicacao.Salvar(equipeEntity);

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

            SetarViewBag();
            ViewBag.TurnoId = new SelectList(ListarPeriodos(), "Id", "Periodo");

            return View("Index");
        }

        public ActionResult BuscarEquipes()
        {
            var equipes = AutoMapper.Mapper.Map<List<Equipe>, List<EquipeViewModel>>(ListaEquipes);
            return PartialView("_GridEquipes", equipes);
        }

        public override ActionResult Edit(int id)
        {
            var equipeDM = equipeAplicacao.BuscarPorId(id);
            var equipe = AutoMapper.Mapper.Map<Equipe, EquipeViewModel>(equipeDM);
            equipe.Datafim = equipeDM.Datafim;
            equipe.IdEncarregado = equipe.Encarregado.Id.ToString();
            equipe.IdSupervisor = equipe.Supervisor.Id.ToString();

            var colaboradoresViewModel = new List<ColaboradorViewModel>();

            foreach (var colaborador in equipeDM.Colaboradores)
            {
                var colaboradorViewModel = new ColaboradorViewModel();

                colaboradorViewModel = AutoMapper.Mapper.Map<Colaborador, ColaboradorViewModel>(colaborador.Colaborador);
                colaboradorViewModel.Turno.Periodo = this.BuscarPeriodoFormatado(colaboradorViewModel.Turno.Periodo,colaboradorViewModel.Turno.Inicio,colaboradorViewModel.Turno.Fim);
                colaboradoresViewModel.Add(colaboradorViewModel);
            }
            equipe.Colaboradores = colaboradoresViewModel;

            Session["Colaboradores"] = equipe.Colaboradores;

            Session["Unidade"] = equipeDM.Unidade;

            if (equipeDM.TipoHorario == TipoHorario.SegundaASexta)
                Session["Horarios"] = 1;
            else if (equipeDM.TipoHorario == TipoHorario.Sabado)
                Session["Horarios"] = 2;
            else if (equipeDM.TipoHorario == TipoHorario.Domingo)
                Session["Horarios"] = 3;
            else
                Session["Horarios"] = 4;

            Session["idEquipe"] = id;

            SetarViewBag();
            ViewBag.TurnoId = new SelectList(ListarPeriodos(), "Id", "Periodo");


            return View("Index", equipe);
        }

        private string BuscarPeriodoFormatado(string periodo, string inicio, string fim)
        {

            return periodo + " - " + inicio + " as " + fim;

        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            Dictionary<string, object> jsonResult = new Dictionary<string, object>();
            try
            {
                SetarViewBag();
                ViewBag.TurnoId = new SelectList(ListarPeriodos(), "Id", "Periodo");

                equipeAplicacao.ExcluirPorId(id);

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
                    message = "A equipe esta sendo usada em outras associações de tabelas do sistema";
                    tipoModal = TipoModal.Warning;
                }

                GerarDadosModal("Atenção", message, tipoModal);

                jsonResult.Add("Status", "Error");
            }

            return View("Index");

        }

        [CheckSessionOut]
        public override ActionResult Delete(int id)
        {
            try
            {
                SetarViewBag();
                GerarDadosModal("Remover registro", "Deseja remover este registro?", TipoModal.Danger, "ConfirmarDelete",
                    "Sim, Desejo remover!", id);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                return View("Index");
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", new BusinessRuleException(ex.Message).Message, TipoModal.Danger);
                return View("Index");
            }
            return View("Index");
        }

        public void SetarViewBag()
        {
            ViewBag.TurnoId = new SelectList(periodoHorarioAplicacao.Buscar().ToList(), "Id", "Periodo");
            ViewBag.ColaboradorID = new SelectList(funcionarioAplicacao.Buscar().ToList(), "Id", "Pessoa.Nome");
        }

    }
}