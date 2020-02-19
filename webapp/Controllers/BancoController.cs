using Aplicacao;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Controllers
{
    public class BancoController : GenericController<Banco>
    {
        public List<Banco> ListaBancos => Aplicacao?.Buscar()?.ToList() ?? new List<Banco>();

        public BancoController(IBancoAplicacao marcaAplicacao)
        {
            Aplicacao = marcaAplicacao;
        }
    }
}
