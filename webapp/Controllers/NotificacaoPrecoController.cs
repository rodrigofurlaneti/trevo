using Entidade;
using Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Portal.Models;
using Core.Extensions;
using Entidade.Base;
using Entidade.Uteis;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Enums;

namespace Portal.Controllers
{
    public class NotificacaoPrecoController : GenericController<PrecoNotificacao> 
    {
        private readonly INotificacaoAplicacao _notificacaoAplicacao;
        private readonly IPrecoNotificacaoAplicacao _precoNotificacaoAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private Usuario _usuario;

        public NotificacaoPrecoController(INotificacaoAplicacao notificacaoAplicacao,
                                             IPrecoNotificacaoAplicacao precoNotificacaoAplicacao, 
                                             IUsuarioAplicacao usuarioAplicacao)
                                     
        {
            _notificacaoAplicacao = notificacaoAplicacao;
            _precoNotificacaoAplicacao = precoNotificacaoAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult Index(string entidade)
        {
            ModelState.Clear();
            _usuario = (Usuario)Session["UsuarioLogado"];
            //var retorno = AutoMapper.Mapper.Map<List<PrecoNotificacao>, List<PrecoNotificacaoViewModel>>(_precoNotificacaoAplicacao.RetornaNotificacoesPendentes(_usuario));
            //return View("~/Views/Notificacao/TabelaPreco", retorno);
            return View("~/Views/Notificacao/TabelaPreco", null);
        }

        public ActionResult Aprovar(PrecoNotificacaoViewModel Model)
        {
            try
            {
                var modelDM = AutoMapper.Mapper.Map<PrecoNotificacaoViewModel, PrecoNotificacao>(Model); 
                 _precoNotificacaoAplicacao.Aprovar(modelDM);

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Aprovação realizada com sucesso",
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
            return View("Index","Home");
        }

        public ActionResult Reprovar(PrecoNotificacaoViewModel modelVM)
        {
            try
            {
                var modelDM = AutoMapper.Mapper.Map<PrecoNotificacaoViewModel, PrecoNotificacao>(modelVM);
                _precoNotificacaoAplicacao.Reprovar(modelDM);

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Reprovação realizada com sucesso!",
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
            return View("Index", "Home");
        }

        public NotificacaoMainData ObterNotificacoes()
        {
            //NotificacaoMainData obj = new NotificacaoMainData();
            //if (System.Web.HttpContext.Current.Session["UsuarioLogado"] != null) { 
            //    _usuario = (Usuario)System.Web.HttpContext.Current.Session["UsuarioLogado"];
            //    var retorno = _precoNotificacaoAplicacao.RetornaNotificacoesPendentes(_usuario);
            //    obj.totalNotificacoes = retorno.Count();
            //    List<NotificacaoData> lista = new List<NotificacaoData>();
            //    if (retorno.Count > 0)
            //    {
            //        lista.Add(new NotificacaoData() { Entidade = Entidades.TabelaPreco.ToString() , EntidadeForm = EnumExtension.GetEnumDescription(Entidades.TabelaPreco), Mensagem = " Total: (" + retorno.Count.ToString() + ") : Pendências ", UltimaAtualizacao = System.DateTime.Now.ToString("ss") + " segundos atrás..." });
            //    }
            //    obj.data = lista;
            //}
            //return obj;

            return null;
        }
    }
}