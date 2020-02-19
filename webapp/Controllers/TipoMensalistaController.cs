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
    public class TipoMensalistaController : GenericController<TipoMensalista>
    {
        public List<TipoMensalista> ListaTiposMensalista => Aplicacao?.Buscar()?.ToList() ?? new List<TipoMensalista>();

        public TipoMensalistaController(ITipoMensalistaAplicacao tipoMensalistaAplicacao)
        {
            Aplicacao = tipoMensalistaAplicacao;
        }
    }
}