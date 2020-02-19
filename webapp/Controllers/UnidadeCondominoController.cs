using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Portal.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




namespace Portal.Controllers
{
    public class UnidadeCondominoController : GenericController<UnidadeCondomino>
    {
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public List<UnidadeCondomino> ListaUnidadesCondomino => Aplicacao?.Buscar()?.ToList() ?? new List<UnidadeCondomino>();
        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao.ListarOrdenadas();

        public UnidadeCondominoController(IUnidadeCondominoAplicacao UnidadeCondominoAplicacao,
                                          IUnidadeAplicacao unidadeAplicacao)
        {
            Aplicacao = UnidadeCondominoAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }
    }
}