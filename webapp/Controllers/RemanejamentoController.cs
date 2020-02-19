using Aplicacao;
using Aplicacao.ViewModels;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Portal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class RemanejamentoController : GenericController<Remanejamento>
    {
        private readonly IUnidadeAplicacao unidadeAplicacao;
        private readonly ITipoEquipeAplicacao tipoEquipeAplicacao;
        private readonly IFuncionamentoAplicacao funcionamentoAplicacao;
        private readonly IFuncionarioAplicacao funcionarioAplicacao;
        private readonly IEquipeAplicacao equipeAplicacao;
        private readonly IRemanejamentoAplicacao remanejamentoAplicacao;
        private readonly IColaboradorAplicacao colaboradorAplicacao;
        private readonly IHorarioPrecoAplicacao horarioPrecoAplicacao;

        public RemanejamentoController(
            IUnidadeAplicacao unidadeAplicacao,
            ITipoEquipeAplicacao tipoEquipeAplicacao,
            IFuncionamentoAplicacao funcionamentoAplicacao,
            IFuncionarioAplicacao funcionarioAplicacao,
            IEquipeAplicacao equipeAplicacao,
            IRemanejamentoAplicacao remanejamentoAplicacao,
            IColaboradorAplicacao colaboradorAplicacao,
            IHorarioPrecoAplicacao horarioPrecoAplicacao)
        {
            this.unidadeAplicacao = unidadeAplicacao;
            this.tipoEquipeAplicacao = tipoEquipeAplicacao;
            this.funcionamentoAplicacao = funcionamentoAplicacao;
            this.funcionarioAplicacao = funcionarioAplicacao;
            this.equipeAplicacao = equipeAplicacao;
            this.remanejamentoAplicacao = remanejamentoAplicacao;
            this.colaboradorAplicacao = colaboradorAplicacao;
            this.horarioPrecoAplicacao = horarioPrecoAplicacao;

            ViewBag.ListaOperacao = new SelectList(
               Enum.GetValues(typeof(TipoOpreracao)).Cast<TipoOpreracao>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
               "Id",
               "Descricao");
            ListaEquipe = new List<EquipeViewModel>();

        }

        public List<UnidadeViewModel> ListaUnidade => unidadeAplicacao.ListarOrdenadas();

        public List<EquipeViewModel> ListaEquipe { get; set; }
        // GET: Remanejamento
        [CheckSessionOut]
        public override ActionResult Index()
        {
            Session["EquipeOrigem"] = "";
            Session["EquipeDestino"] = "";

            Session["TipoEquipeOrigem"] = "";
            Session["TipoEquipeDestino"] = "";

            Session["HorarioOrigem"] = "";
            Session["HorarioDestino"] = "";

            Session["EquipeDestinoBusca"] = "";
            Session["EquipeOrigemBusca"] = "";

            return View();
        }

        [HttpPost]
        public JsonResult BuscarEquipe(int idUnidade, int IdTipoEquipe)
        {
            var equipes = equipeAplicacao.BuscarPor(x => x.Unidade.Id == idUnidade && x.TipoEquipe.Id == IdTipoEquipe).ToList();

            return Json(equipes.Select(x => new { x.Id, x.Nome }));
        }

        [HttpPost]
        public JsonResult BuscarTipoEquipe(int idUnidade)
        {
            var tipoEquipes = equipeAplicacao.BuscarPor(x => x.Unidade.Id == idUnidade).Select(x => x.TipoEquipe).ToList();
            tipoEquipes = tipoEquipes.DistinctBy(p => p.Id).ToList();
            return Json(tipoEquipes.Select(x => new { x.Id, x.Descricao }));
        }

        [HttpPost]
        public JsonResult BuscarHorario(int IdEquipe)
        {

            var equipeDM = equipeAplicacao.BuscarPorId(IdEquipe);



             var typeItemList = new List<HorarioPrecoViewModel>();

            if (equipeDM.TipoHorario == TipoHorario.SegundaASexta)
                typeItemList.Add(new HorarioPrecoViewModel { Id = 1, Horario = "Segunda à Sexta" });

            if (equipeDM.TipoHorario == TipoHorario.Sabado)
                typeItemList.Add(new HorarioPrecoViewModel { Id = 2, Horario = "Sábado" });

            if (equipeDM.TipoHorario == TipoHorario.Domingo)
                typeItemList.Add(new HorarioPrecoViewModel { Id = 3, Horario = "Domingo" });

            if (equipeDM.TipoHorario == TipoHorario.Feriado)
                typeItemList.Add(new HorarioPrecoViewModel { Id = 4, Horario = "Feriado" });



            //var HorarioPrecoVM = AutoMapper.Mapper.Map<List<HorarioPreco>, List<HorarioPrecoViewModel>>(equipeAplicacao.BuscarPor(x => x.Id == IdEquipe)
            //                                                                                    .FirstOrDefault().HorarioTrabalho.HorariosPrecos.ToList());
            return Json(typeItemList);
        }

        public ActionResult SalvarDados(RemanejamentoViewModel remanejamento, 
                                        RemanejamentoTransferenciaViewModel RemanejamentoOrigem, 
                                        RemanejamentoTransferenciaViewModel RemanejamentoDestino)
        {
            try
            {

                var horarioOrigemDM = new HorarioPreco();
                var horarioDestinoDM = new HorarioPreco();
                var equipeOrigemVM = (EquipeViewModel)Session["EquipeOrigem"];
                var equipeDestinoVM = (EquipeViewModel)Session["EquipeDestino"];
                var equipeOrigemDM = equipeAplicacao.BuscarPorId(equipeOrigemVM.Id);
                var equipeDestinoDM = equipeAplicacao.BuscarPorId(equipeDestinoVM.Id);
                var colaboradoresOrigemDM = equipeOrigemDM.Colaboradores;
                var colaboradoresDestinoDM = equipeDestinoDM.Colaboradores;
                var equipesColaboradorOrigemDM = new List<EquipeColaborador>();
                var equipesColaboradorDestinoDM = new List<EquipeColaborador>();

                foreach (var colaborador in equipeOrigemVM.Colaboradores)
                {
                    var equipeColaboradorDM = new EquipeColaborador();

                    var colaboradorDM = new Colaborador();

                    colaboradorDM = colaboradorAplicacao.BuscarPorId(colaborador.Id);

                    if (colaborador.NomeColaborador != null)
                    {
                        colaboradorDM = colaboradorAplicacao.BuscarPor(x => x.NomeColaborador.Pessoa.Id == colaborador.NomeColaborador.Pessoa.Id).FirstOrDefault();
                    }


 
                    equipeColaboradorDM.Colaborador = colaboradorDM;
                    equipesColaboradorOrigemDM.Add(equipeColaboradorDM);
                }

                foreach (var colaborador in equipeDestinoVM.Colaboradores)
                {
                    var equipeColaboradorDM = new EquipeColaborador();

                    var colaboradorDM = new Colaborador();

                    colaboradorDM = colaboradorAplicacao.BuscarPorId(colaborador.Id);

                    if (colaborador.NomeColaborador != null)
                    {
                        colaboradorDM = colaboradorAplicacao.BuscarPor(x => x.NomeColaborador.Pessoa.Id == colaborador.NomeColaborador.Pessoa.Id).FirstOrDefault();
                    }

                    equipeColaboradorDM.Colaborador = colaboradorDM;
                    equipesColaboradorDestinoDM.Add(equipeColaboradorDM);
                }

                equipeOrigemDM.Colaboradores = equipesColaboradorOrigemDM;
                equipeDestinoDM.Colaboradores = equipesColaboradorDestinoDM;

                var tipoEquipeOrigemDM = (TipoEquipe)Session["TipoEquipeOrigem"];
                var tipoEquipeDestinoDM = (TipoEquipe)Session["TipoEquipeDestino"];

                horarioOrigemDM = null;
                horarioDestinoDM = null;
                var RemanejamentoOrigemDM = new RemanejamentoTransferencia();
                var RemanejamentoDestinoDM = new RemanejamentoTransferencia();


                RemanejamentoOrigemDM.Horario = horarioOrigemDM;
                RemanejamentoOrigemDM.TipoEquipe = tipoEquipeOrigemDM;
                RemanejamentoOrigemDM.Equipe = equipeOrigemDM;
                RemanejamentoOrigemDM.Unidade = AutoMapper.Mapper.Map<UnidadeViewModel, Entidade.Unidade>(RemanejamentoOrigem.Unidade);

                RemanejamentoDestinoDM.Horario = horarioDestinoDM;
                RemanejamentoDestinoDM.TipoEquipe = tipoEquipeDestinoDM;
                RemanejamentoDestinoDM.Equipe = equipeDestinoDM;
                RemanejamentoOrigemDM.Unidade = AutoMapper.Mapper.Map<UnidadeViewModel, Entidade.Unidade>(RemanejamentoDestino.Unidade);

                var remanejamentoDM = AutoMapper.Mapper.Map<RemanejamentoViewModel, Remanejamento>(remanejamento);

                remanejamentoDM.RemanejamentoOrigem = RemanejamentoOrigemDM;
                remanejamentoDM.RemanejamentoDestino = RemanejamentoDestinoDM;

                remanejamentoAplicacao.Salvar(remanejamentoDM);

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

            //return RedirectToAction("Index");
            return View("Index");
        }

        [CheckSessionOut]
        public ActionResult BuscarColaboradores(int IdEquipe)
        {
            var equipeDM = equipeAplicacao.BuscarPorId(IdEquipe);
            var equipeViewModel = AutoMapper.Mapper.Map<Equipe, EquipeViewModel>(equipeDM);


            var colaboradoresViewModel = new List<ColaboradorViewModel>();

            foreach (var colaborador in equipeDM.Colaboradores)
            {
                var colaboradorViewModel = new ColaboradorViewModel();

                colaboradorViewModel = AutoMapper.Mapper.Map<Colaborador, ColaboradorViewModel>(colaborador.Colaborador);

                if (colaboradorViewModel.Turno != null)
                {
                    colaboradoresViewModel.Add(colaboradorViewModel);
                }
            }
            equipeViewModel.Colaboradores = colaboradoresViewModel;

            Session["EquipeDestinoBusca"] = equipeViewModel;

            return PartialView("_GridColaboradoresDestino", equipeViewModel);
        }


        [CheckSessionOut]
        public ActionResult BuscarColaboradoresOrigem(int IdEquipe)
        {
            var equipeDM = equipeAplicacao.BuscarPorId(IdEquipe);
            var equipeViewModel = AutoMapper.Mapper.Map<Equipe, EquipeViewModel>(equipeDM);


            var colaboradoresViewModel = new List<ColaboradorViewModel>();

            foreach (var colaborador in equipeDM.Colaboradores)
            {
                var colaboradorViewModel = new ColaboradorViewModel();

                colaboradorViewModel = AutoMapper.Mapper.Map<Colaborador, ColaboradorViewModel>(colaborador.Colaborador);

                if (colaboradorViewModel.Turno != null)
                {
                    colaboradoresViewModel.Add(colaboradorViewModel);
                }
            }
            equipeViewModel.Colaboradores = colaboradoresViewModel;


            Session["EquipeOrigemBusca"] = equipeViewModel;

            return PartialView("_GridColaboradoresOrigem", equipeViewModel);
        }

        [CheckSessionOut]
        public JsonResult AtualizarEquipes(EquipeViewModel equipeOrigem, EquipeViewModel equipeDestino)
        {
            equipeOrigem.Colaboradores = equipeOrigem.Colaboradores.DistinctBy(x => x.Id).ToList();
            equipeDestino.Colaboradores = equipeDestino.Colaboradores.DistinctBy(x => x.Id).ToList();
            string message;
            var tipo = TipoModal.Success;
            var divGridColaboradorOrigem = string.Empty;
            var divGridColaboradorDestino = string.Empty;
            try
            {
                message = "Transferência feita com sucesso!";
                divGridColaboradorDestino = SalvarEquipeDestino(equipeOrigem, equipeDestino);
                divGridColaboradorOrigem = SalvarEquipeOrigem(equipeOrigem, equipeDestino);
            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }


            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), divGridColaboradorOrigem, divGridColaboradorDestino }, ContentType = "application/json", MaxJsonLength = int.MaxValue };

        }

        private string SalvarEquipeOrigem(EquipeViewModel equipeOrigem, EquipeViewModel equipeDestino)
        {
            var equipeOrigemViewModel = Session["EquipeOrigemBusca"] as EquipeViewModel;

            var colaboradoresOrigem = equipeOrigem.Colaboradores;
            var colaboradoresDestino = equipeDestino.Colaboradores;

            for (int i = 0; i < equipeOrigem.Colaboradores.Count; i++)
            {
                var colaboradorAtualizar = equipeOrigemViewModel.Colaboradores.Where(x => x.Id == equipeOrigem.Colaboradores[i].Id).FirstOrDefault();
                if (colaboradorAtualizar != null)
                        colaboradorAtualizar.NomeColaborador = colaboradoresDestino[i].NomeColaborador;

            }

            Session["EquipeOrigem"] = equipeOrigemViewModel;

            return RazorHelper.RenderRazorViewToString(ControllerContext, "_GridColaboradoresOrigem", equipeOrigemViewModel);
        }

        private string SalvarEquipeDestino(EquipeViewModel equipeOrigem, EquipeViewModel equipeDestino)
        {
            var equipeDestinoViewModel = Session["EquipeDestinoBusca"] as EquipeViewModel;

            var colaboradoresOrigem =  equipeOrigem.Colaboradores;
            var colaboradoresDestino = equipeOrigem.Colaboradores;

             for (int i = 0; i < equipeDestino.Colaboradores.Count; i++)
            {
                var colaboradorAtualizar = equipeDestinoViewModel.Colaboradores.Where(x => x.Id == equipeDestino.Colaboradores[i].Id).FirstOrDefault();
                if (colaboradorAtualizar != null)
                    colaboradorAtualizar.NomeColaborador = colaboradoresOrigem[i].NomeColaborador;
            }

            Session["EquipeDestino"] = equipeDestinoViewModel;
            return RazorHelper.RenderRazorViewToString(ControllerContext, "_GridColaboradoresDestino", equipeDestinoViewModel);
        }

        public void SalvarTipoEquipeOrigem(int IdTipoEquipe)
        {
            var tipoEquipeOrigem = tipoEquipeAplicacao.BuscarPorId(IdTipoEquipe);
            Session["TipoEquipeOrigem"] = tipoEquipeOrigem;
        }

        public void SalvarTipoEquipeDestino(int IdTipoEquipe)
        {
            var tipoEquipeDestino = tipoEquipeAplicacao.BuscarPorId(IdTipoEquipe);
            Session["TipoEquipeDestino"] = tipoEquipeDestino;
        }


        //public void SalvarHorarioOrigem(int IdHorario)
        //{
        //    var horarioOrigem = horarioPrecoAplicacao.BuscarPorId(IdHorario);
        //    Session["HorarioOrigem"] = horarioOrigem;
        //}

        //public void SalvarHorarioDestino(int IdHorario)
        //{
        //    var horarioDestino = horarioPrecoAplicacao.BuscarPorId(IdHorario);
        //    Session["HorarioDestino"] = horarioDestino;
        //}

    }
}