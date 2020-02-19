using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IUnidadeRepositorio : IRepository<Unidade>
    {
        IList<Unidade> BuscarPorConvenio(int idConvenio);
        List<Unidade> ListarOrdenadoSimplificado();
    }
}