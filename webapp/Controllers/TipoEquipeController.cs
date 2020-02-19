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
    public class TipoEquipeController : GenericController<TipoEquipe>
    {
        public List<TipoEquipe> ListaTiposEquipe => Aplicacao?.Buscar()?.ToList() ?? new List<TipoEquipe>();

        public TipoEquipeController(ITipoEquipeAplicacao tipoEquipeAplicacao)
        {
            Aplicacao = tipoEquipeAplicacao;
        }
    }
}