using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class SelecaoDespesaRepositorio : NHibRepository<SelecaoDespesa>, ISelecaoDespesaRepositorio
    {
        public SelecaoDespesaRepositorio(NHibContext context) : base(context)
        {

        }

        public void RemoverPorContaAPagarId(int id)
        {
            var sql = $@"DELETE FROM selecaodespesa WHERE ContaAPagar = " + id + ";";

            Session.CreateSQLQuery(sql);
        }
    }
}
