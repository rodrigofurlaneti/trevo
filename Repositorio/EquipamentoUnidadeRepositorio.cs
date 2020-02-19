using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EquipamentoUnidadeRepositorio:NHibRepository<EquipamentoUnidade>, IEquipamentoUnidadeRepositorio
    {
        public EquipamentoUnidadeRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
