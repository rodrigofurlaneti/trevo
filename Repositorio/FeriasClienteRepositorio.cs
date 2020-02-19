using System;
using System.Text;
using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class FeriasClienteRepositorio : NHibRepository<FeriasCliente>, IFeriasClienteRepositorio
    {
        public FeriasClienteRepositorio(NHibContext context)
            : base(context)
        {
        }

        public bool VerificaExistenciaBoletoMensalista(int idCliente, DateTime dataCompetencia)
        {
            var sql = new StringBuilder();
            sql.Append("  SELECT COUNT(*) TotalLancamentos ");
            sql.Append("  FROM LancamentoCobranca l ");
            sql.Append($" WHERE l.TipoServico = 1 and l.Cliente = {idCliente} and l.DataCompetencia = '{dataCompetencia.ToString("yyyy-MM-dd")}' ");

            var query = Session.CreateSQLQuery(sql.ToString());

            var result = query.UniqueResult<int>();

            return result > 0;
        }
    }
}