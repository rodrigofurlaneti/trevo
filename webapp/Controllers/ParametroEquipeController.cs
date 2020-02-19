using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Portal.Controllers
{
    public class ParametroEquipeController : GenericController<ParametroEquipe>
    {
        private readonly IEquipeAplicacao _equipeAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ITipoEquipeAplicacao _tipoEquipeAplicacao;
        private readonly IHorarioParametroEquipeAplicacao _horarioParametroEquipeAplicacao;
        private readonly ITipoNotificacaoAplicacao _tipoNotificacaoAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IParametroEquipeAplicacao _parametroEquipeAplicacao;

        public List<EquipeViewModel> ListaEquipe;
        public List<TipoEquipeViewModel> ListaTipoEquipe;

        public ParametroEquipeController(IParametroEquipeAplicacao parametroEquipeAplicacao,
                                         IUnidadeAplicacao unidadeAplicacao,
                                         ITipoEquipeAplicacao tipoEquipeAplicacao,
                                         IEquipeAplicacao equipeAplicacao,
                                         IHorarioParametroEquipeAplicacao horarioParametroEquipeAplicacao,
                                         ITipoNotificacaoAplicacao tipoNotificacaoAplicacao,
                                         IUsuarioAplicacao usuarioAplicacao)
        {
            _unidadeAplicacao = unidadeAplicacao;
            _tipoEquipeAplicacao = tipoEquipeAplicacao;
            Aplicacao = parametroEquipeAplicacao;
            _equipeAplicacao = equipeAplicacao;
            _horarioParametroEquipeAplicacao = horarioParametroEquipeAplicacao;
            _tipoNotificacaoAplicacao = tipoNotificacaoAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
            ListaEquipe = new List<EquipeViewModel>();
            ListaTipoEquipe = new List<TipoEquipeViewModel>();
            _parametroEquipeAplicacao = parametroEquipeAplicacao;
        }

        public List<UnidadeViewModel> ListaUnidade => AutoMapper.Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(_unidadeAplicacao.Buscar().ToList());
        public List<ParametroEquipe> ListaParametroEquipe => Aplicacao?.Buscar()?.Where(x => x.Equipe != null).ToList() ?? new List<ParametroEquipe>();

        // GET: ParametroEquipe
        public override ActionResult Index()
        {
            return View();
        }

        [CheckSessionOut]
        [HttpPost]
        public new JsonResult SalvarDados(ParametroEquipeViewModel model, List<HorarioParametroEquipe> listahorarioparametro)
        {
            try
            {
                var entity = Aplicacao.BuscarPorId(model.Id) ?? model.ToEntity();

                var usuarioLogado = HttpContext.User as CustomPrincipal;

                entity.Usuario = usuarioLogado.Nome;

                entity.Ativo = model.Ativo;

                entity.HorarioParametroEquipe = listahorarioparametro == null ? new List<HorarioParametroEquipe>() : listahorarioparametro;
                entity.Equipe = _equipeAplicacao.BuscarPorId(model.Equipe.Id);

                _parametroEquipeAplicacao.Salvar(entity, usuarioLogado.UsuarioId);

                ModelState.Clear();

                return Json(new Resultado<ParametroEquipeViewModel>()
                {
                    Sucesso = true,
                    TipoModal = TipoModal.Success.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = "Registro Salvo com Sucesso"
                });
            }
            catch (BusinessRuleException br)
            {
                return Json(new Resultado<ParametroEquipeViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = br.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new Resultado<ParametroEquipeViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = ex.Message
                });
            }
        }

        public override ActionResult Edit(int id)
        {
            var cliente = new ParametroEquipeViewModel(Aplicacao.BuscarPorId(id));


            return View("Index", cliente);
        }

        [HttpPost]
        public ActionResult AtualizaEquipamentos(int Id)
        {

            //var unidade = AutoMapper.Mapper.Map<Unidade, UnidadeViewModel>(_unidadeAplicacao.BuscarPorId(Id));

            var cliente = new ParametroEquipeViewModel(Aplicacao.BuscarPorId(Id));

            //return Json(cliente.CheckListEstruturaUnidade);
            return PartialView("_GridHorarioParametroEquipe", cliente.HorarioParametroEquipe);
        }


        [CheckSessionOut]
        [HttpPost]
        public ActionResult DeletaEquipamento(int Index, List<HorarioParametroEquipeViewModel> listahorarioparametro)
        {
            try
            {
                if(listahorarioparametro[Index].Id != 0)
                    _horarioParametroEquipeAplicacao.ExcluirPorId(listahorarioparametro[Index].Id);

                listahorarioparametro.RemoveAt(Index);

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

            return PartialView("_GridHorarioParametroEquipe", listahorarioparametro);
        }


        [HttpPost]
        public ActionResult AtualizaTipoEquipes(int? unidadeid)
        {


            var tipoequipes = _equipeAplicacao.BuscarPor(x => x.Unidade.Id == unidadeid).Select(x => x.TipoEquipe).ToList();

            ListaTipoEquipe = tipoequipes.Select(x => new TipoEquipeViewModel(x)).ToList();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(ListaEquipe);

            return Json(ListaTipoEquipe, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AtualizaEquipes(int? unidadeid, int? tipoequipeid)
        {

            var retornoEquipes = _equipeAplicacao.BuscarPor(x => x.Unidade.Id == unidadeid && x.TipoEquipe.Id == tipoequipeid);

            //var retornoEquipes = _equipeAplicacao.BuscarPor(x => (unidadeid == null ? x.Unidade.Id == x.Unidade.Id : x.Unidade.Id == unidadeid)
            //                                           && (tipoequipeid == null ? x.TipoEquipe.Id == x.TipoEquipe.Id : x.TipoEquipe.Id == tipoequipeid)).ToList();

            ListaEquipe = retornoEquipes.Select(x => new EquipeViewModel(x)).ToList();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(ListaEquipe);

            return Json(ListaEquipe, JsonRequestBehavior.AllowGet);
        }
    }
}