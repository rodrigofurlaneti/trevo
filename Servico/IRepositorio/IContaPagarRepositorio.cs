using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IContaPagarRepositorio : IRepository<ContasAPagar>
    {
        IList<ContasAPagar> ListarContasPagar(int mes, int idUnidade);

        IList<ContasAPagar> ListarContasPagar(int? idContaFinanceira);

    }
}