using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IConvenioRepositorio : IRepository<Convenio>
    {
        IList<Convenio> BuscarPorAtivoComUnidade();
        IList<Convenio> BuscarAtivosPorUnidade(int idUnidade);
    }
}