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
    public class ContaContabilController : GenericController<ContaContabil>
    {
        public List<ContaContabil> ListaContasContabil => Aplicacao?.Buscar()?.ToList() ?? new List<ContaContabil>();

        public ContaContabilController(IContaContabilAplicacao ContaContabilAplicacao)
        {
            Aplicacao = ContaContabilAplicacao;
        }
    }
}