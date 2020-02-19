using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class TipoLocacaoController : GenericController<TipoLocacao>
    {
        public List<TipoLocacao> ListaTiposLocacao => Aplicacao?.Buscar()?.ToList() ?? new List<TipoLocacao>();

        public TipoLocacaoController(ITipoLocacaoAplicacao TipoLocacaoAplicacao)
        {
            Aplicacao = TipoLocacaoAplicacao;
        }
    }
}