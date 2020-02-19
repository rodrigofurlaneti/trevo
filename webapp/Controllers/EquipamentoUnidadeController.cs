using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Newtonsoft.Json;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InitializerHelper.Startup;

namespace Portal.Controllers
{
    public class EquipamentoUnidadeController : GenericController<EquipamentoUnidade>
    {
        public List<UnidadeViewModel> ListaUnidade
        {
            get { return (List<UnidadeViewModel>)Session["ListaUnidade"] ?? new List<UnidadeViewModel>(); }
            set { Session["ListaUnidade"] = value; }
        }

        public List<EquipamentoUnidade> ListaEquipamentoUnidade => Aplicacao?.Buscar()?.ToList() ?? new List<EquipamentoUnidade>();
        public List<SelectListItem> ListaPeriodoDia { get; set; }

        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IPerfilAplicacao _perfilAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly ICheckListEstruturaUnidadeAplicacao _checkListEstruturaUnidadeAplicacao;
        public string NomePerfil { get; set; }

        public EquipamentoUnidadeController(IEquipamentoUnidadeAplicacao EquipamentoUnidadeAplicacao,
                                            IUnidadeAplicacao unidadeAplicacao,
                                            IPerfilAplicacao perfilAplicacao,
                                            IUsuarioAplicacao usuarioAplicacao,
                                            ICheckListEstruturaUnidadeAplicacao checkListEstruturaUnidadeAplicacao)
        {
            Aplicacao = EquipamentoUnidadeAplicacao;

            ListaPeriodoDia = new List<SelectListItem>();

            ListaPeriodoDia.Add(new SelectListItem { Value = "1", Text = "a cada 7 dias" });
            ListaPeriodoDia.Add(new SelectListItem { Value = "2", Text = "a cada 10 dias" });
            ListaPeriodoDia.Add(new SelectListItem { Value = "3", Text = "a cada 20 dias" });

            _unidadeAplicacao = unidadeAplicacao;
            _perfilAplicacao = perfilAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
            _checkListEstruturaUnidadeAplicacao = checkListEstruturaUnidadeAplicacao;

        }

        // GET: RetiradaCofre
        public override ActionResult Index()
        {

            ListaUnidade = AutoMapper.Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(_unidadeAplicacao.Buscar().ToList());

            NomePerfil = BuscaPerfil();

            return View();
        }

        /// <summary>
        /// busca o nome do Perfil do usuário atual 
        /// </summary>
        /// <returns>se admin retorna o perfil, se não branco</returns>
        private string BuscaPerfil()
        {
            var usuarioLogado = HttpContext.User as CustomPrincipal;

            var usuarioAtivo = _usuarioAplicacao.BuscarPor(x => x.Id == usuarioLogado.UsuarioId).FirstOrDefault();

            var perfil = usuarioAtivo.Perfils.Where(x => x.Perfil.Nome.ToLower().Trim().Contains("root")).ToList();

            if(perfil.Any())
            {
                return perfil.FirstOrDefault().Perfil.Nome;
            }

            return "";
        }

        [CheckSessionOut]
        [HttpPost]
        public new JsonResult SalvarDados(EquipamentoUnidadeViewModel model, IList<CheckListEstruturaUnidadeViewModel> listaequipamentos)
        {
            try
            {

                var entity = Aplicacao.BuscarPorId(model.Id) ?? model.ToEntity();

                //ajustando propriedades
                entity.ConferenciaConcluida = model.ConferenciaConcluida;
                //entity.DataInsercao = model.DataInsercao;

                entity.GerarNotificacao = model.GerarNotificacao;
                entity.Unidade = _unidadeAplicacao.BuscarPorId(model.Unidade.Id);

                entity.Unidade.CheckListEstruturaUnidade = listaequipamentos == null ? new List<CheckListEstruturaUnidade>() : listaequipamentos.Select(x => x.ToEntity()).ToList(); 

                entity.Observacao = model.Observacao;
                entity.PeriodoEquipamentoUnidade = model.PeriodoEquipamentoUnidade;
                entity.Usuario = model.Usuario;

                Aplicacao.Salvar(entity);

                ModelState.Clear();

                return Json(new Resultado<EquipamentoUnidadeViewModel>()
                {
                    Sucesso = true,
                    TipoModal = TipoModal.Success.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = "Registro Salvo com Sucesso"
                });
            }
            catch (BusinessRuleException br)
            {
                return Json(new Resultado<EquipamentoUnidadeViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = br.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new Resultado<EquipamentoUnidadeViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = ex.Message
                });
            }
        }


        [CheckSessionOut]
        [HttpPost]
        public new JsonResult Conferencia(EquipamentoUnidadeViewModel model, IList<CheckListEstruturaUnidadeViewModel> listaequipamentos)
        {
            bool ultrapassouData = false;

            try
            {
                var entity = Aplicacao.BuscarPorId(model.Id) ?? new EquipamentoUnidade();

                var dataModificacao = entity.UltimaConferencia;

                if (dataModificacao != null && entity.GerarNotificacao)
                {
                    if (model.PeriodoEquipamentoUnidade == Entidade.Uteis.PeriodoDiasEquipamentoUnidade.aCada7Dias)
                    {
                        var diasdiff = dataModificacao.Value.AddDays(7);

                        if(DateTime.Now > diasdiff)
                        {
                            ultrapassouData = true;
                        }
                    }
                    else if (model.PeriodoEquipamentoUnidade == Entidade.Uteis.PeriodoDiasEquipamentoUnidade.aCada10Dias)
                    {
                        var diasdiff = dataModificacao.Value.AddDays(10);

                        if (DateTime.Now > diasdiff)
                        {
                            ultrapassouData = true;
                        }
                    }
                    else if (model.PeriodoEquipamentoUnidade == Entidade.Uteis.PeriodoDiasEquipamentoUnidade.aCada20Dias)
                    {
                        var diasdiff = dataModificacao.Value.AddDays(20);

                        if (DateTime.Now > diasdiff)
                        {
                            ultrapassouData = true;
                        }
                    } 
                }

                //ajustando propriedades
                entity.UltimaConferencia = DateTime.Now;

                //entity.Usuario = ??

                NomePerfil = BuscaPerfil();

                Aplicacao.Salvar(entity);

                ModelState.Clear();

                return Json(new Resultado<EquipamentoUnidadeViewModel>()
                {
                    Sucesso = true,
                    TipoModal = TipoModal.Success.ToDescription(),
                    Titulo = "Conferência",
                    Mensagem = ultrapassouData ? "Conferência Realilzada. OBS: O Prazo de conferência foi ultrapassado!!!" : "Conferência realizada com Sucesso"
                    //ATENÇÃO: ao inves da mensagem na modal, verificar a variável 'ultrapassouData' para realizar a notificação <<<<<
                });
            }
            catch (BusinessRuleException br)
            {
                return Json(new Resultado<EquipamentoUnidadeViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Conferência",
                    Mensagem = br.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new Resultado<EquipamentoUnidadeViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Conferência",
                    Mensagem = ex.Message
                });
            }
        }


        [CheckSessionOut]
        [HttpPost]
        public ActionResult DeletaEquipamento(int Index, List<CheckListEstruturaUnidade> listahorarioparametro)//int IdEquipamento, int IdUnidade)
        {

            try
            {
                if (listahorarioparametro[Index].Id != 0)
                    _checkListEstruturaUnidadeAplicacao.ExcluirPorId(listahorarioparametro[Index].Id);

                listahorarioparametro.RemoveAt(Index);

                //if (IdEquipamento != 0)
                //{
                //    var Equipamento = _checkListEstruturaUnidadeAplicacao.BuscarPorId(IdEquipamento);
                //    _checkListEstruturaUnidadeAplicacao.Excluir(Equipamento);
                //}

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro excluído com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Index");
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index");
            }

            NomePerfil = BuscaPerfil();

            return PartialView("_GridCheckListEstruturaUnidade", listahorarioparametro);
        }

        public override ActionResult Edit(int id)
        {
            var cliente = new EquipamentoUnidadeViewModel(Aplicacao.BuscarPorId(id));

            NomePerfil = BuscaPerfil();

            return View("Index", cliente);
        }

        [HttpPost]
        public ActionResult ListaCheckListEstruturaUnidade(int IdUnidade)
        {
            var TipoAtividades = new List<TipoAtividadeViewModel>();

            var cliente = new UnidadeViewModel(_unidadeAplicacao.BuscarPorId(IdUnidade));

            //return Json(cliente.CheckListEstruturaUnidade);
            return PartialView("_GridCeckListEstruturaUnidade", cliente.CheckListEstruturaUnidade);
        }


        [HttpPost]
        public ActionResult AtualizaEquipamentos(int Id)
        {

            var unidade = AutoMapper.Mapper.Map<Unidade, UnidadeViewModel>(_unidadeAplicacao.BuscarPorId(Id));

            //return Json(cliente.CheckListEstruturaUnidade);
            return PartialView("_GridCheckListEstruturaUnidade", unidade.CheckListEstruturaUnidade);
        }
    }
}