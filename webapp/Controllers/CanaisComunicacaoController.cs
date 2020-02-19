using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class CanaisComunicacaoController : GenericController<CanaisComunicacao>
    {
        public List<CanaisComunicacao> ListaCanaisComunicacaos => Aplicacao?.Buscar()?.ToList() ?? new List<CanaisComunicacao>();
        public IEnumerable<ChaveValorViewModel> ListaTipoCanais => Aplicacao?.BuscarValoresDoEnum<CanalComunicacao>();
        public IEnumerable<ChaveValorViewModel> ListaTipoComunicacao => Aplicacao?.BuscarValoresDoEnum<TipoComunicacao>();


        public CanaisComunicacaoController(ICanaisComunicacaoAplicacao canaisComunicacaoAplicacao)
        {
            Aplicacao = canaisComunicacaoAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            return View("Index");
        }
    }
}
