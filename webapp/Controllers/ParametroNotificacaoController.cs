using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;

namespace Portal.Controllers
{
    public class ParametroNotificacaoController : GenericController<ParametroNotificacao>
    {
	    private readonly IParametroNotificacaoAplicacao _parametroNotificacaoAplicacao;
        private readonly ITipoNotificacaoAplicacao _tipoNotificacaoAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        public List<TipoNotificacaoViewModel> ListaTiposNotificacao => AutoMapper.Mapper.Map<List<TipoNotificacao>, List<TipoNotificacaoViewModel>>(_tipoNotificacaoAplicacao.Buscar().ToList());
        public List<UsuarioViewModel> ListaUsuarios; 
        public List<ParametroNotificacaoViewModel> ListaParametros;  

        public ParametroNotificacaoController(IParametroNotificacaoAplicacao parametroNotificacaoAplicacao,
                                              ITipoNotificacaoAplicacao tipoNotificacaoAplicacao,
                                              IUsuarioAplicacao usuarioAplicacao)
        {
            _parametroNotificacaoAplicacao = parametroNotificacaoAplicacao;
            _tipoNotificacaoAplicacao = tipoNotificacaoAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
        }

        public void AtualizaListas()
        {
            ListaUsuarios = new List<UsuarioViewModel>();
            ListaParametros = new List<ParametroNotificacaoViewModel>();
            ListaParametros = AutoMapper.Mapper.Map<List<ParametroNotificacao>, List<ParametroNotificacaoViewModel>>(_parametroNotificacaoAplicacao.Buscar().ToList());
            ListaUsuarios = AutoMapper.Mapper.Map<List<Usuario>, List<UsuarioViewModel>>(_usuarioAplicacao.Buscar().ToList());
            Session["ParametrosNot"] = ListaParametros;
            Session["UsuariosAprov"] = ListaUsuarios;
        }

	    [HttpGet]
	    [CheckSessionOut]
	    public override ActionResult Index()
	    {
            ModelState.Clear();
            AtualizaListas();
            return View("Index");
	    }

        [CheckSessionOut]
		public override ActionResult Edit(int id)
	    {
		    var parametroNotificacao = new ParametroNotificacaoViewModel(_parametroNotificacaoAplicacao.BuscarPorId(id));
            AtualizaListaSession();
            return View("Index", parametroNotificacao);
	    }

        [CheckSessionOut]
        public override ActionResult Delete(int id)
        {
            try
            {
                AtualizaListaSession();
                GerarDadosModal("Remover registro", "Deseja remover este registro?", TipoModal.Danger, "ConfirmarDelete",
                    "Sim, Desejo remover!", id);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
            }

            return View("Index");
        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            Dictionary<string, object> jsonResult = new Dictionary<string, object>();
            try
            {
                AtualizaListaSession();
                _parametroNotificacaoAplicacao.ExcluirPorId(id);

                ModelState.Clear();
                GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);
            }
            catch (NotFoundException ne)
            {
	            GerarDadosModal("Atenção", $"Registro {ne.Message} já excluído anteriormente", TipoModal.Warning);
	            jsonResult.Add("Status", "Error");
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
                GerarDadosModal("Atenção", message, tipoModal);
                jsonResult.Add("Status", "Error");
            }
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ParametroNotificacaoViewModel model)
	    {
		    try
		    {
                AtualizaListaSession();

                var itens = _parametroNotificacaoAplicacao.BuscarPor(x => x.TipoNotificacao.Id == model.TipoNotificacao.Id && x.Id != model.Id);
                if (itens != null && itens.Any())
                    throw new BusinessRuleException("Já existe um parâmetro para este tipo de notificação!");
                
                if (model.IdAprovadores != null && model.IdAprovadores.Any()) 
                    model.Usuarios.AddRange(model.IdAprovadores.Select(idUsuario => AutoMapper.Mapper.Map<Usuario, UsuarioViewModel>(_usuarioAplicacao.BuscarPorId(idUsuario))).ToList());
                else
                    throw new BusinessRuleException("Informe um ou mais usuários aprovadores!");

                ParametroNotificacao _objSalvar = model.ToEntity();
                _objSalvar.Aprovadores = RetornaUsuarios(model.Usuarios);
                _objSalvar.TipoNotificacao = _tipoNotificacaoAplicacao.BuscarPorId(model.TipoNotificacao.Id);
                _objSalvar.Aprovadores = RetornaUsuarios(_objSalvar.Aprovadores);
                _parametroNotificacaoAplicacao.Salvar(_objSalvar);

				ModelState.Clear();

				GerarDadosModal("Sucesso", "Registro salvo com sucesso", TipoModal.Success);
			    return View("Index");
		    }
		    catch (BusinessRuleException br)
		    {
			    GerarDadosModal("Atenção",
				    br.Message,
				    TipoModal.Warning);
			    return View("Index");
		    }
		    catch (Exception ex)
		    {
			    GerarDadosModal("Atenção",
				    "Ocorreu um erro ao salvar: " + ex.Message,
				    TipoModal.Danger);
			    return View("Index");
		    }
	    }

        private void AtualizaListaSession()
        {
            if (((List<ParametroNotificacaoViewModel>)(Session["ParametrosNot"])) != null)
                ListaParametros = Session["ParametrosNot"] as List<ParametroNotificacaoViewModel>;
            if (((List<UsuarioViewModel>)(Session["UsuariosAprov"])) != null)
                ListaUsuarios = Session["UsuariosAprov"] as List<UsuarioViewModel>;
        }

        private List<ParametroNotificacaoUsuario> RetornaUsuarios(IList<ParametroNotificacaoUsuario> listaAprovadores)
        {
            List<ParametroNotificacaoUsuario> lista = new List<ParametroNotificacaoUsuario>();
            foreach (var item in listaAprovadores)
            {
                item.Usuario = _usuarioAplicacao.BuscarPorId(item.Usuario.Id);
                lista.Add(item);
            }
            return lista;
        }

        private List<ParametroNotificacaoUsuario> RetornaUsuarios(IList<UsuarioViewModel> listaAprovadores)
        {
            List<ParametroNotificacaoUsuario> lista = new List<ParametroNotificacaoUsuario>();
            foreach (var item in listaAprovadores)
            {
                var user = _usuarioAplicacao.BuscarPorId(item.Id.GetValueOrDefault());
                var model = new ParametroNotificacaoUsuario();
                model.Usuario = user;
                lista.Add(model);
            }
            return lista;
        }
    }
}