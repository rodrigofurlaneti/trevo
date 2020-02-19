using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface ITipoSeloRepositorio : IRepository<TipoSelo>
    {
        IList<TipoSelo> BuscarPorConvenioUnidade(int idConvenio, int idUnidade);
    }
}