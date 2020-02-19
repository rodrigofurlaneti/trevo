using System;
using System.Web.Mvc;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using Entidade.Base;
using Core.Exceptions;
using System.Web.UI.WebControls;
using Portal.Helpers;
using Newtonsoft.Json;
using System.Linq;
using System.Data.SqlClient;
using Core.Extensions;

namespace Portal.Controllers
{
    public abstract class GenericController<T> : BaseController where T : IEntity, new()
    {
        public IBaseAplicacao<T> Aplicacao { get; set; }
        public CustomPrincipal UsuarioLogado => HttpContext.User as CustomPrincipal;

        protected GenericController(IBaseAplicacao<T> aplicacao)
        {
            Aplicacao = aplicacao;
        }

        protected GenericController()
        {
            //É necessário este construtor vazio
        }

        [CheckSessionOut]
        public virtual ActionResult Index()
        {
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public virtual ActionResult Index(T model)
        {
            return View("Index", new T());
        }

        [CheckSessionOut]
        public virtual ActionResult Edit(int id)
        {
            var model = Aplicacao.BuscarPorId(id);

            return View("Index", model);
        }
        
        [CheckSessionOut]
        public virtual ActionResult Delete(int id)
        {
            try
            {
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
        
        [CheckSessionOut]
        public virtual ActionResult ConfirmarDelete(int id)
        {
            try
            {
                Aplicacao.ExcluirPorId(id);

                ModelState.Clear();
                GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                return View("Index");
            }
            catch(NotFoundException ne)
            {
                CriarModalAvisoComRetornoParaIndex(ne.Message);
            }
            catch (SoftparkIntegrationException sx)
            {
                CriarModalAvisoComRetornoParaIndex(sx.Message);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                GerarDadosModal("Atenção", new BusinessRuleException(message).Message, TipoModal.Danger);
                return View("Index");
            }


            return View("Index");
        }
        
        [CheckSessionOut]
        [HttpPost]
        public virtual ActionResult Salvar(T model)
        {
            try
            {
                Aplicacao.Salvar(model);
                ModelState.Clear();
                GerarDadosModal("Sucesso", "Registro salvo com sucesso", TipoModal.Success);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
            }
            catch (SoftparkIntegrationException sx)
            {
                CriarModalAvisoComRetornoParaIndex(sx.Message);
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", "Ocorreu um erro ao salvar: " + new BusinessRuleException(ex.Message).Message, TipoModal.Danger);
            }
            return View("Index");
        }
        
        [CheckSessionOut]
        [HttpGet]
        public ActionResult SalvarDados()
        {
            return RedirectToAction("Index");
        }

        public T UpdateModel(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var propertiesFromNewEntity = entity.GetType().GetProperties();

            var currentEntity = Aplicacao.BuscarPorId(entity.Id);

            if (currentEntity == null)
                return entity;

            var propertiesFromCurrentEntity = currentEntity.GetType().GetProperties();

            for (var i = 0; i < propertiesFromCurrentEntity.Length; i++)
                propertiesFromCurrentEntity[i].SetValue(currentEntity, propertiesFromNewEntity[i].GetValue(entity, null), null);

            return currentEntity;
        }

        [HttpGet]
        [CheckSessionOut]
        public string VisualizarLogEntidade(string nomeEntidade, int codigoEntidade)
        {
            var logs = Aplicacao.BuscarLogPor(x => x.Entidade.ToLower() == nomeEntidade.ToLower() && x.CodigoEntidade == codigoEntidade).Select(x=> new AuditViewModel(x)).ToList();
            return JsonConvert.SerializeObject(logs);
        }
        
        protected DadosModal CriarDadosModalSucesso(string mensagem, string titulo = "Sucesso")
        {
            return GerarDadosModal(titulo, mensagem, TipoModal.Success);
        }

        protected DadosModal CriarDadosModalAviso(string mensagem, string titulo = "Atenção")
        {
            return GerarDadosModal(titulo, mensagem, TipoModal.Warning);
        }

        protected DadosModal CriarDadosModalErro(string mensagem, string titulo = "Erro")
        {
            return GerarDadosModal(titulo, new BusinessRuleException(mensagem).Message, TipoModal.Danger);
        }

        protected DadosModal CriarModalAvisoComRetornoParaIndex(string mensagem, string titulo = "Atenção")
        {
            DadosModal = CriarDadosModalAviso(mensagem);
			DadosModal.RedirectUrl = $"/{ControllerName}/Index";
            return DadosModal;
        }

        protected DadosModal CriarModalConfirmacao(string mensagem, string titulo, string acaoConfirmar = null, string tituloConfirmar = "Sim, OK!", int idRegistro = 0, string redirectUrl = null, string acaoFunction = null)
        {
            return GerarDadosModal(titulo, mensagem, TipoModal.Info, acaoConfirmar, tituloConfirmar, idRegistro, redirectUrl, acaoFunction);
        }

        public ActionResult GerarDadosModalPartial(DadosModal dto)
        {
            GerarDadosModal(dto.Titulo, dto.Mensagem, dto.TipoModal, dto.AcaoConfirma, dto.TituloConfirma, dto.Id, dto.RedirectUrl);
            return PartialView("_ModalNovo", DadosModal);
        }

        public ActionResult AtualizarAbrirModal(DadosModal dadosModal)
        {
            return PartialView("_Modal", dadosModal);
        }

        public TJson RemoverLoopDoJson<TJson>(TJson obj)
        {
            var objJson = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            var objConvertido = JsonConvert.DeserializeObject<TJson>(objJson);
            return objConvertido;
        }
    }
}
