using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EquipamentoRepositorio : NHibRepository<Equipamento>,IEquipamentoRepositorio
    {
        public EquipamentoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
