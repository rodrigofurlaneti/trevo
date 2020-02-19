using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using NHibernate.Transform;

namespace Repositorio
{
    public class ContaPagarRepositorio : NHibRepository<ContasAPagar>, IContaPagarRepositorio
    {
        public ContaPagarRepositorio(NHibContext context)
            : base(context)
        {
        }

        public IList<ContasAPagar> ListarContasPagar(int mes, int idUnidade)
        {
            var sql = new StringBuilder();

            sql.Append("    select cp.Id, cp.TipoPagamento, cp.DataVencimento, cp.Valor, cp.FormaPagamento, cp.NumeroParcela, cp.Observacoes, cp.PodePagarEmEspecie, cp.ValorSolicitado, cp.StatusConta,cp.Ativo,cp.CodigoAgrupadorParcela,cp.NumeroRecibo,cp.ContaFinanceira,cp.Departamento,cp.Fornecedor,cp.Unidade,cp.ContaContabil,cp.DataPagamento,cp.Ignorado,cp.TipoDocumentoConta,cp.NumeroDocumento from contasapagar cp ");
            sql.Append("    left join despesacontasapagar dcp on cp.id = dcp.ContaAPagar");
            sql.Append("    left join selecaodespesa sd on sd.id = dcp.SelecaoDespesa");
            sql.Append("    WHERE cp.Unidade = " + idUnidade);
            sql.Append("    AND MONTH(cp.datapagamento) = " + mes);

            var query = Session.CreateSQLQuery(sql.ToString()).SetResultTransformer(Transformers.AliasToBean(typeof(ContasAPagar)));

            var retorno = query.List<ContasAPagar>()?.ToList() ?? new List<ContasAPagar>();

            return retorno;
        }

        public IList<ContasAPagar> ListarContasPagar(int? idContaFinanceira)
        {
            var sql = new StringBuilder();

            sql.Append("    SELECT L.Id FROM CONTAPAGAR L");
            sql.Append("    INNER JOIN CONTAFINANCEIRA AS CF ON CF.ID = L.CONTAFINANCEIRA");
            sql.Append("    WHERE 1 = 1 AND L.DATA IS NULL");

            if (idContaFinanceira != null && idContaFinanceira != 0)
                sql.Append($" AND CF.Id = " + idContaFinanceira);
            
            var query = Session.CreateSQLQuery(sql.ToString());

            var retornoIds = query.List<int>()?.ToList() ?? new List<int>();

            return !retornoIds.Any() ? new List<ContasAPagar>() : Session.CreateQuery($"SELECT L FROM CONTAPAGAR L WHERE ID in ({string.Join(",", retornoIds)})")?.List<ContasAPagar>() ?? new List<ContasAPagar>();
        }
    }
}
