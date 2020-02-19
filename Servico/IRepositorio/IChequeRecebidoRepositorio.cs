using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IChequeRecebidoRepositorio : IRepository<ChequeRecebido>
    {
        IList<ChequeRecebido> BuscarDadosSimples();
    }
}
