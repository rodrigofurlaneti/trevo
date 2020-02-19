using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IDespesaContasAPagarRepositorio : IRepository<DespesaContasAPagar>
    {
        void RemoverPorSelecaoDespesaId(int id);
        void RemoverPorContasAPagar(List<ContasAPagar> contasAPagar);
    }
}
