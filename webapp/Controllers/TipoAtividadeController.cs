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
    public class TipoAtividadeController : GenericController<TipoAtividade>
    {
        public List<TipoAtividade> ListaTiposAtividade => Aplicacao?.Buscar()?.ToList() ?? new List<TipoAtividade>();

        public TipoAtividadeController(ITipoAtividadeAplicacao tipoAtividadeAplicacao)
        {
            Aplicacao = tipoAtividadeAplicacao;
        }
    }
}