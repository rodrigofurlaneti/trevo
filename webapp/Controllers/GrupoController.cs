using Aplicacao;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Controllers
{
    public class GrupoController : GenericController<Grupo>
    {
        public List<Grupo> ListaGrupos => Aplicacao?.Buscar()?.ToList() ?? new List<Grupo>();

        public GrupoController(IGrupoAplicacao grupoAplicacao)
        {
            Aplicacao = grupoAplicacao;
        }
    }
}
