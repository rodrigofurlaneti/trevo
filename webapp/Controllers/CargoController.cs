using Aplicacao;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Controllers
{
    public class CargoController : GenericController<Cargo>
    {
        public List<Cargo> ListaCargos => Aplicacao?.Buscar()?.ToList() ?? new List<Cargo>();

        public CargoController(ICargoAplicacao CargoAplicacao)
        {
            Aplicacao = CargoAplicacao;
        }
    }
}
