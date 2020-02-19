using Aplicacao;
using Aplicacao.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ComboController : BaseController
    {
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ITipoSeloAplicacao _tipoSeloAplicacao;

        public ComboController(
            IUnidadeAplicacao unidadeAplicacao,
            ITipoSeloAplicacao tipoSeloAplicacao)
        {
            _unidadeAplicacao = unidadeAplicacao;
            _tipoSeloAplicacao = tipoSeloAplicacao;
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarUnidade(int? idConvenio = null)
        {
            var lista = _unidadeAplicacao.ListaUnidade(idConvenio)
                .Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = x.Nome
                })
                .ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarTipoSelo(int? idUnidade = null, int? idConvenio = null)
        {
            var lista = _tipoSeloAplicacao.ListaTipoSelo(idConvenio, idUnidade)
                .Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = x.Nome
                })
                .ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}