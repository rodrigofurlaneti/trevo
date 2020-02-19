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

namespace Portal.Controllers
{
    [PermitirQualquerPerfil]
    public class NotificacaoController : GenericController<Notificacao>
    {
        private readonly IPrecoNotificacaoAplicacao _precoNotificacaoAplicacao;
        private readonly INotificacaoAplicacao _notificacaoAplicacao;

        public NotificacaoController(IPrecoNotificacaoAplicacao precoNotificacaoAplicacao, INotificacaoAplicacao notificacaoAplicacao)
        {
            _notificacaoAplicacao = notificacaoAplicacao;
            _precoNotificacaoAplicacao = precoNotificacaoAplicacao;
        }

        [HttpPost]
        [CheckSessionOut]
        public ActionResult Index(string entidade)
        {
            return View();
        }

        [HttpPost]
        [CheckSessionOut]
        public ActionResult Principal(int idTipoNotificacao)
        {
            var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
            var notificacoes = _notificacaoAplicacao.ObterNotificacoes(usuarioLogadoCurrent.UsuarioId, idTipoNotificacao).ToList();

            return PartialView("_GridNotificacao", notificacoes);
        }

        [CheckSessionOut]
        public ActionResult RespostaNotificacao(int id, int tipoNotificacao, int acao)
        {
            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _notificacaoAplicacao.AtualizarStatus(id, (Entidades)tipoNotificacao, usuarioLogadoCurrent.UsuarioId, (AcaoNotificacao)acao, ControllerContext);
                ModelState.Clear();
            }
            catch (BusinessRuleException br)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new Resultado<object>()
            {
                Sucesso = true,
                TipoModal = TipoModal.Warning.ToDescription(),
                Titulo = "Atenção",
                Mensagem = "Aprovação realizada com sucesso",
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Informacoes(int id, int tipoNotificacao, int acao)
        {
            try
            {
                var notificacao = _notificacaoAplicacao.BuscarPorId(id);
                var url = string.Empty;
                var retorno = _notificacaoAplicacao.Informacoes(id, (Entidades)tipoNotificacao);

                if (!string.IsNullOrEmpty(notificacao.UrlPersonalizada))
                    url = notificacao.UrlPersonalizada.Contains("?") ? $"/{notificacao.UrlPersonalizada}" : $"/{notificacao.UrlPersonalizada}/{retorno}";
                else
                    url = $"/{((Entidades)tipoNotificacao).ToString()}/Edit/{retorno}";

                return Json(
                    new
                    {
                        url
                    }, JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                return Json(new Resultado<ParametroEquipeViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = ex.Message
                });
            }
        }

        public JsonResult ObterNotificacoes()
        {
            var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
            if (usuarioLogadoCurrent != null)
            {
                var notificacoesDM = _notificacaoAplicacao.ObterNotificacoes(usuarioLogadoCurrent.UsuarioId, null).ToList();
                var notificacoesVM = ListarNotificacoesDestalhe(notificacoesDM);
                return Json(
                    new
                    {
                        ListaNotificacoes = notificacoesVM,
                        TotalNot = notificacoesDM.Count
                    }, JsonRequestBehavior.AllowGet
                );
            }

            return null;
        }

        private List<NotificacaoData> ListarNotificacoesDestalhe(List<NotificacaoViewModel> notificacoesDM)
        {
            List<NotificacaoData> notificacoesVM = new List<NotificacaoData>();

            var tipos = notificacoesDM.Select(x => x.TipoNotificacao.Entidade).Distinct();

            foreach (var item in tipos)
            {
                notificacoesVM.Add(new NotificacaoData() { IdTipoNotificacao = (int)item, Entidade = item.ToDescription(), EntidadeForm = item.ToDescription(), Mensagem = notificacoesDM.Where(x => x.TipoNotificacao.Entidade == item).Count().ToString(), UltimaAtualizacao = DateTime.Now.ToString("ss") + " segundos atrás..." });
            }
            
            return notificacoesVM;
        }
    }
}