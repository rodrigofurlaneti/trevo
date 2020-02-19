using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Configuration;
using Aplicacao;
using Core.Extensions;
using System.Collections.Generic;
using Aplicacao.ViewModels;

namespace Portal.Controllers
{
    public class UtilsController : Controller
    {
        #region Constructors / Destructors
        public UtilsController(IUsuarioAplicacao usuarioAplicacao)
        {
            _usuarioAplicacao = usuarioAplicacao;
        }
        #endregion

        #region Private Members
        // TODO: This should be moved to the constructor of the controller in combination with a DependencyResolver setup
        // NOTE: You can use NuGet to find a strategy for the various IoC packages out there (i.e. StructureMap.MVC5)
        //private readonly UserManager _manager = UserManager.Create();

        private readonly IUsuarioAplicacao _usuarioAplicacao;

        private void AddErrors(DbEntityValidationException exc)
        {
            foreach (var error in exc.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors.Select(validationError => validationError.ErrorMessage)))
            {
                ModelState.AddModelError("", error);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            // Add all errors that were returned to the page error collection
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion

        public ActionResult RecuperarMinhaSenha(string key)
        {
            var account = new AccountLoginModel { CPF = Crypt.DeCrypt(key, ConfigurationManager.AppSettings["CryptKey"]).FromBase64() };
            return View(account);
        }

        [HttpGet]
        public JsonResult PessoaLogada()
        {
            var usuarioLogado = HttpContext.User as CustomPrincipal;
            if (usuarioLogado == null)
                throw new Exception("Usuário não logado.");
            return Json(new PessoaViewModel(_usuarioAplicacao.BuscarPorId(usuarioLogado.UsuarioId).Funcionario.Pessoa), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SuggestionPerson(string param)
        {
            return Json(_usuarioAplicacao.BuscarPor(x => x.Login.Contains(param)).Select(x => new PessoaViewModel(x.Funcionario.Pessoa)));
        }
    }
}