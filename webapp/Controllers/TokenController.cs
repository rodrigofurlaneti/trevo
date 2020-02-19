using Entidade;
using System.Web.Mvc;
using Aplicacao;
using Portal.Decorators;

namespace Portal.Controllers
{
    [CustomAuthorize]
    public class TokenController : Controller
    {
        private readonly ITokenAplicacao _tokenAplicacao;

        public TokenController(ITokenAplicacao tokenAplicacao)
        {
            _tokenAplicacao = tokenAplicacao;
        }

        // GET: Token
        public ActionResult Index()
        {
            var model = _tokenAplicacao.Buscar();
            return View(model);
        }

        // GET: Token/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Token/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Token/Create
        [HttpPost]
        public ActionResult Create(Token token)
        {
            _tokenAplicacao.Salvar(token);
            return RedirectToAction("Index");
        }

        // GET: Token/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Token/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            return RedirectToAction("Index");
        }

        // GET: Token/Delete/5
        public ActionResult Delete(int id)
        {
            var obj = new Token { Id = id };
            return View(obj);
        }

        // POST: Token/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _tokenAplicacao.Excluir(new Token { Id = id });
            return RedirectToAction("Index");
        }
    }
}
