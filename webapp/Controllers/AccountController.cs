using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Portal.Properties;
using AutoMapper;
using Entidade;
using Newtonsoft.Json;

namespace Portal.Controllers
{
    public class AccountController : BaseController
    {
        #region Constructors / Destructors
        private readonly IPessoaAplicacao _pessoaAplicacao;
        public AccountController(IUsuarioAplicacao usuarioAplicacao, IPessoaAplicacao pessoaAplicacao)
        {
            _usuarioAplicacao = usuarioAplicacao;
            _pessoaAplicacao = pessoaAplicacao;
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

        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }

        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // If we cannot verify if the url is local to our host we redirect to a default location
            return RedirectToAction("index", "home");
        }

        #endregion

        public ActionResult RecuperarMinhaSenha(string key)
        {
            var account = new AccountLoginModel { CPF = Crypt.DeCrypt(key, ConfigurationManager.AppSettings["CryptKey"]).FromBase64() };

            var usuario = _usuarioAplicacao.Buscar().Where(x => x.Login.Replace(".","").Replace("-","") == account.CPF).FirstOrDefault();


            var teste = new AccountLoginModel {
                CPF = usuario.Login,
                 Email = usuario.Funcionario.Pessoa.ContatoEmail,
                   Nome = usuario.Funcionario.Pessoa.Nome,
                     Password = usuario.Senha,    
            };

            //var teste = AccountLoginModel(usuario);

            return View(teste);
        }

        [HttpGet]
        public JsonResult PessoaLogada()
        {
            var usuarioLogado = HttpContext.User as CustomPrincipal;
            if (usuarioLogado == null)
                throw new Exception("Usuário não logado.");
            return Json(new PessoaViewModel(_usuarioAplicacao.BuscarPorId(usuarioLogado.UsuarioId).Funcionario.Pessoa), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RecuperarMinhaSenha(AccountResetPasswordModel viewModel)
        {
            try
            {
                _usuarioAplicacao.TrocarSenha(viewModel.CPF, viewModel.Password, viewModel.PasswordConfirm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErroRecuperarSenha", ex.Message);
                return View("RecuperarMinhaSenha", new AccountLoginModel { CPF = viewModel.CPF });
            }

            ViewBag.Sucesso = "Senha trocada com sucesso!";
            return View("RecuperarMinhaSenha");
        }

        // GET: /account/forgotpassword
        public ActionResult ForgotPassword()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View("ForgotPassword");
        }

        [HttpPost]
        public ActionResult ForgotPassword(AccountLoginModel viewModel)
        {
            try
            {
                _usuarioAplicacao.RecuperarSenha(viewModel.CPF, Resources.RecuperarSenha, Request.Url.GetLeftPart(UriPartial.Authority));
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("ErroRecuperarSenha", ex.Message);
                return View("ForgotPassword", new { viewModel.CPF });
            }
            catch (BusinessRuleException ex)
            {
	            ModelState.AddModelError("ErroRecuperarSenha", ex.Message);
	            return View("ForgotPassword", new { viewModel.CPF });
            }
			catch (Exception ex)
            {
                throw new Exception("ERRO: " + ex.Message);
            }

            ViewBag.ForgotPassword = true;

            return View("ForgotPassword", new { CPF = viewModel.CPF, Password = "" });
        }

        // GET: /account/login
        public ActionResult Login(string returnUrl)
        {
            HttpContext.Session.Abandon();
            Session.Abandon();

            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            // We do not want to use any existing identity information
            EnsureLoggedOut();

            // Store the originating URL so we can attach it to a form field
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };

            return View(viewModel);
        }

        // POST: /account/login
        [HttpPost]
        public ActionResult Login(AccountLoginModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Login", viewModel);

            var usuario = _usuarioAplicacao.ValidarLogin(viewModel.CPF, viewModel.Password);
            if (usuario == null)
            {
                ViewBag.MsgErro = "CPF ou Senha incorretos!";
                return View("Login", viewModel);
            }

            Session["UserAvatar"] = usuario.ImagemUpload;

            var serializeModel = new CustomPrincipalSerializeModel
            {
                UsuarioId = usuario.Id,
                PessoaId = usuario.Funcionario.Id,
                Nome = usuario.Funcionario.Pessoa.Nome,
                Login = viewModel.CPF.Replace(".", "").Replace("-", ""),
                PerfilsId = usuario.Perfils.Select(x => x.Perfil.Id).ToList(),
                RememberMe = viewModel.RememberMe,
            };

            var serializer = new JavaScriptSerializer();
            var userData = serializer.Serialize(serializeModel);
            var authTicket = new FormsAuthenticationTicket(1, serializeModel.Login, DateTime.Now, DateTime.Now.AddMinutes(30), viewModel.RememberMe, userData);
            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            Response.Cookies.Add(faCookie);

            if (usuario.PrimeiroLogin)
                return PrimeiroLogin(new AccountResetPasswordModel { Nome = usuario.Funcionario.Pessoa.Nome, CPF = viewModel.CPF, OldPassword = viewModel.Password, ImagemUpload = usuario.ImagemUpload });

            if (!string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
                return Redirect(viewModel.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult PrimeiroLogin(AccountResetPasswordModel viewModel)
        {
            ViewBag.CompletePrimeiroLogin = "Complete seu primeiro acesso, alterando a sua senha padrão!";
            return View("PrimeiroLogin", viewModel);
        }

        [HttpPost]
        public ActionResult RegistrarPrimeiroLogin(AccountResetPasswordModel viewModel)
        {
            var usuario = _usuarioAplicacao.ValidarLogin(viewModel.CPF, viewModel.OldPassword);
            if (usuario == null)
            {
                ViewBag.MsgErro = "CPF ou Senha incorretos!";
                return Login(new AccountLoginModel { CPF = viewModel.CPF, Email = viewModel.Email, Password = viewModel.OldPassword });
            }

            usuario.ImagemUpload = (byte[])(Session["ImgAvatar"] ?? Session["UserAvatar"]);
            usuario.Senha = viewModel.Password;

            _usuarioAplicacao.Salvar(usuario);
            _usuarioAplicacao.PrimeiroLoginRealizado(viewModel.CPF);
            Session["UserAvatar"] = usuario.ImagemUpload;

            return Login(new AccountLoginModel { CPF = viewModel.CPF, Password = viewModel.Password });
        }

        // GET: /account/error
        public ActionResult Error()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View("Error");
        }

        // GET: /account/register
        public ActionResult Register()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View(new AccountRegistrationModel());
        }

        // POST: /account/register
        [HttpPost]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult> Register(AccountRegistrationModel viewModel)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //// Ensure we have a valid viewModel to work with
            //if (!ModelState.IsValid)
            //    return View(viewModel);

            //// Prepare the identity with the provided information
            //var user = new IdentityUser
            //{
            //    UserName = viewModel.Username ?? viewModel.CPF,
            //    CPF = viewModel.CPF
            //};

            //// Try to create a user with the given identity
            //try
            //{
            //    var result = await _manager.CreateAsync(user, viewModel.Password);

            //    // If the user could not be created
            //    if (!result.Succeeded) {
            //        // Add all errors to the page so they can be used to display what went wrong
            //        AddErrors(result);

            //        return View(viewModel);
            //    }

            //    // If the user was able to be created we can sign it in immediately
            //    // Note: Consider using the CPF verification proces
            //    await SignInAsync(user, false);

            //    return RedirectToLocal();
            //}
            //catch (DbEntityValidationException ex)
            //{
            //    // Add all errors to the page so they can be used to display what went wrong
            //    AddErrors(ex);

            //    return View(viewModel);
            //}
            return RedirectToLocal();
        }

        // GET: /account/Logout
        [HttpGet]
        public ActionResult Logout()
        {
            // Delete the user details from cache.
            Session.Abandon();
            // First we clean the authentication ticket like always
            FormsAuthentication.SignOut();
            // Second we clear the principal to ensure the user does not retain any authentication
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            // Clear authentication cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
            // this clears the Request.IsAuthenticated flag since this triggers a new request

            return RedirectToAction("Login");
            //return RedirectToAction("Login");
        }

        // GET: /account/lock
        public ActionResult Lock()
        {
            return View("Lock");
        }

        public PartialViewResult RetornaPartialMenusLaterais(bool isMobile = false)
        {
            if (HttpContext.User == null)
            {
                this.Logout();
                TempData["danger"] = "Usuário não encontrado!";
                return PartialView("_MenusLateral", new List<MenuViewModel>());
            }
        
            var usuarioLogado = HttpContext.User as CustomPrincipal;
            if (usuarioLogado == null)
            {
                TempData["danger"] = "Usuário não encontrado!";
                return PartialView("_MenusLateral", new List<MenuViewModel>());
            }

            var usuario = _usuarioAplicacao.BuscarPorId(usuarioLogado.UsuarioId);
            Session["UsuarioLogado"] = usuario;

            if (usuario == null || usuario.Perfils == null || !usuario.Perfils.Any())
            {
                this.Logout();
                TempData["danger"] = "Usuário não encontrado!";
                return PartialView("_MenusLateral", new List<MenuViewModel>());
            }

            var menus = usuario.Perfils.SelectMany(x => x?.Perfil?.Menus).Select(x => x.Menu).ToList() ?? new List<Menu>();
            menus = menus.DistinctBy(x => x.Id).ToList();
            MenusUsuario = new MenuViewModel().MenuViewModelList(menus);

            ViewBag.IsMobile = isMobile;
            return PartialView("_MenusLateral", MenusUsuario);
        }

        public ActionResult LoginFirstStep(AccountLoginModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.CPF))
            {
                ViewBag.MsgErro = "CPF Inválido!";
                return View("Login");
            }

            var usuario = _usuarioAplicacao.RetornarPorCPF(viewModel.CPF);

            if (usuario == null)
            {
                ViewBag.MsgErro = "Usuário não encontrado!";
                return View("Login");
            }

            viewModel.Imagem = usuario?.GetImage();
            viewModel.Nome = usuario?.Funcionario?.Pessoa?.Nome;
            viewModel.Sexo = usuario?.Funcionario?.Pessoa?.Sexo ?? "Masc";
            return View("LoginSecondStep", viewModel);
        }

        [HttpPost]
        public void ArmazenarImagem(string file)
        {
            const string strBase64 = ";base64,";
            Session["ImgAvatar"] = Convert.FromBase64String(file.Substring(file.IndexOf(strBase64, StringComparison.Ordinal) + strBase64.Length));
        }
    }
}