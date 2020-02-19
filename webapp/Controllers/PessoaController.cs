using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;

namespace Portal.Controllers
{
    public class PessoaController : GenericController<Pessoa>
    {
        private readonly IPessoaAplicacao _pessoaAplicacao;
        private readonly IEmpresaAplicacao _empresaAplicacao;
        public PessoaController(IPessoaAplicacao pessoaAplicacao, IEmpresaAplicacao empresaAplicacao)
        {
            Aplicacao = pessoaAplicacao;
            _pessoaAplicacao = pessoaAplicacao;
            _empresaAplicacao = empresaAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            return View("Index", new PessoaViewModel());
        }

        [CheckSessionOut]
        public ActionResult Pesquisar(string nome, string CPF)
        {
            return PartialView("_lstPessoas", ModoPesquisa(nome, CPF));
        }

        private List<PessoaViewModel> ModoPesquisa(string nome, string CPF)
        {
            var model = new List<PessoaViewModel>();
            var result = _pessoaAplicacao.BuscarPor(p => p.Nome.Contains(nome) && p.Documentos.Any(pd => pd.Documento.Numero == CPF));

            if (result.Count > 0)
                foreach (var item in result)
                    model.Add(new PessoaViewModel(item));

            return model.ToList();
        }

        public JsonResult ValidarSeCpfExiste(string cpf)
        {
            return Json(_pessoaAplicacao.ValidarSeCpfExiste(cpf), JsonRequestBehavior.AllowGet);
        }
    }
}