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
    public class EstruturaGaragemController : GenericController<EstruturaGaragem>
    {
        public List<EstruturaGaragem> ListaTiposEstrutura => Aplicacao?.Buscar()?.ToList() ?? new List<EstruturaGaragem>();

        public EstruturaGaragemController(IEstruturaGaragemAplicacao EstruturaGaragemAplicacao)
        {
            Aplicacao = EstruturaGaragemAplicacao;
        }
    }
}