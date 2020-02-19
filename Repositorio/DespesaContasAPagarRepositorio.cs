using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio
{
    public class DespesaContasAPagarRepositorio : NHibRepository<DespesaContasAPagar>, IDespesaContasAPagarRepositorio
    {
        public DespesaContasAPagarRepositorio(NHibContext context) : base(context)
        {

        }

        public void RemoverPorSelecaoDespesaId(int id)
        {
            var sql = $@"DELETE FROM despesacontasapagar WHERE selecaodespesa = " + id + ";";

            Session.CreateSQLQuery(sql);
        }

        public void RemoverPorContasAPagar(List<ContasAPagar> contasAPagar)
        {
            var list = contasAPagar.Select(c => c.Id).ToList();
            Session.CreateQuery(String.Format("DELETE  FROM despesacontasapagar WHERE contaapagar IN (:idsList)", list))
                .SetParameterList("idsList", list.ToArray())
                .ExecuteUpdate();
        }
    }
}
