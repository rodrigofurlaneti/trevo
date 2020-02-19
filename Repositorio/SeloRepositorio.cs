using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class SeloRepositorio : NHibRepository<Selo>, ISeloRepositorio
    {
        public SeloRepositorio(NHibContext context) : base(context)
        {

        }

        public IList<DadosSelosVO> BuscarSelosPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT  ");
            sql.Append("    la.Unidade, ");
            sql.Append("    la.Convênio, ");
            sql.Append("    la.Cliente, ");
            sql.Append("    la.[Data Pagamento], ");
            sql.Append("    la.[Quantidade Selos], ");
            sql.Append("    la.Periodo, ");
            sql.Append("    IIF(la.IndexCobrancaId > 1, '0', la.[Valor Pago]) [Valor Pago], ");
            sql.Append("    la.SISTEMA, ");
            sql.Append("    la.Id ");
            sql.Append("FROM ");
            sql.Append("( ");
            sql.Append("  SELECT ");
            sql.Append("      u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade  ");
            sql.Append("      , ISNULL(pd.[Convênio], '') [Convênio] ");
            sql.Append("      , UPPER(COALESCE(COALESCE(nullif(cli.NomeFantasia, ''), nullif(cli.RazaoSocial, '')), p.Nome)) as Cliente  ");
            sql.Append("      , l.DataBaixa as [Data Pagamento]  ");
            sql.Append("      , SUM(ISNULL(pd.QTD_SELOS, 0)) as [Quantidade Selos]  ");
            sql.Append("      , ISNULL(pd.Periodo, '') Periodo ");
            sql.Append("      , CAST(CAST(SUM(ISNULL(pag.ValorPago, 0)) as decimal(18, 2)) as varchar(20)) [Valor Pago]  ");
            sql.Append("      , 'NOVO' as SISTEMA  ");
            sql.Append("      , l.Id ");
            sql.Append("      , ROW_NUMBER() OVER(partition by l.Id, u.nome, UPPER(COALESCE(COALESCE(nullif(cli.NomeFantasia, ''), nullif(cli.RazaoSocial, '')), p.Nome)) order by l.Id) IndexCobrancaId ");
            sql.Append("  FROM LancamentoCobranca l ");
            sql.Append("  OUTER APPLY ( ");
            sql.Append("  	            SELECT DISTINCT  ");
            sql.Append("  	            	pd.Id ");
            sql.Append("  	            	, pd.Quantidade QTD_SELOS ");
            sql.Append("  	            	, tp.Nome as Periodo ");
            sql.Append("  	            	, c.Descricao as [Convênio] ");
            sql.Append("  	            FROM LancamentoCobrancaPedidoSelo lp ");
            sql.Append("  	            INNER JOIN PedidoSelo pd on pd.id = lp.PedidoSelo ");
            sql.Append("  	            INNER JOIN EmissaoSelo em on em.PedidoSelo = pd.Id ");
            sql.Append("  	            INNER JOIN Selo s on s.EmissaoSelo = em.Id ");
            sql.Append("  	            INNER JOIN Convenios c on c.Id = pd.Convenio  ");
            sql.Append("  	            INNER JOIN TipoSelo tp on tp.id = pd.TipoSelo  ");
            sql.Append("  	            WHERE lp.LancamentoCobranca_id = l.Id ");
            sql.Append("              ) pd ");
            sql.Append("  INNER JOIN Cliente cli on cli.Id = l.Cliente  ");
            sql.Append("  INNER JOIN Pessoa p on p.Id = cli.Pessoa  ");
            sql.Append("  INNER JOIN Unidade u on u.Id = l.Unidade  ");
            sql.Append("  LEFT JOIN Endereco e on e.Id = u.Endereco  ");
            sql.Append("  OUTER APPLY ( ");
            sql.Append("                   SELECT  ");
            sql.Append("                       SUM(cast(pg.ValorPago as decimal(18, 2))) ValorPago  ");
            sql.Append("                       , SUM(cast(pg.ValorDesconto as decimal(18, 2))) ValorDesconto  ");
            sql.Append("                       , MAX(pg.DataPagamento) DataPagamento  ");
            sql.Append("                   FROM Pagamento pg(NOLOCK) ");
            sql.Append("                   WHERE pg.Recebimento = l.Recebimento  ");
            sql.Append("               ) pag   ");
            sql.Append("  WHERE 1=1  ");
            sql.Append("   AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("   AND l.StatusLancamentoCobranca = 8  ");
            sql.Append("   AND l.TipoServico = 2 ");
            //sql.Append(" AND pag.ValorPago > 0 ");

            //if (unidade > 0)
            //  sql.Append($"    AND l.Unidade = {unidade} ");

            sql.Append("  GROUP BY u.Nome, e.Logradouro, pd.[Convênio], pd.Periodo, l.DataBaixa, cli.NomeFantasia, cli.RazaoSocial, p.Nome, l.Id ");

            //sql.Append("UNION ");
            //sql.Append("    SELECT ");
            //sql.Append("        c.NM_UNIDADE as [Unidade], ");
            //sql.Append("        '' as [Convênio], ");
            //sql.Append("        c.NM_CLIENTE as Cliente, ");
            //sql.Append("        c.DT_PAGTO as [Data Pagamento], ");
            //sql.Append("        0 as [Quantidade Selos], ");
            //sql.Append("        '' as [Periodo], ");
            //sql.Append("        CAST(SUM(CAST(c.VL_PAGTO as decimal(18,2))) as varchar(20)) as [Valor Total], ");
            //sql.Append("        'LEGADO' as SISTEMA, ");
            //sql.Append("        0 as Id, ");
            //sql.Append("        1 ");
            //sql.Append("    FROM DADOS_COBRANCA_REL c ");
            //sql.Append("    WHERE 1=1 ");
            //sql.Append("        AND c.DT_VENCTO between :dataInicio and :dataFim ");
            //sql.Append("        AND c.TIPO = 'SELO' ");

            //sql.Append("        AND ISDATE(c.DT_PAGTO) = 1 ");

            //sql.Append("    GROUP BY ");
            //sql.Append("        c.NM_UNIDADE, c.DT_PAGTO, c.NM_CLIENTE ");

            sql.Append(" ) la ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio);
            query.SetParameter("dataFim", dataFim);

            return ConverterResultadoRelatorio(query.List())?.ToList() ?? new List<DadosSelosVO>();
        }

        public IList<DadosSelosVO> BuscarSelosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT * FROM ( ");
            sql.Append("SELECT ");
            sql.Append("    u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade ");
            sql.Append("    , ISNULL(pd.[Convênio], '') as [Convênio] ");
            sql.Append("    , UPPER(COALESCE(COALESCE(nullif(cli.NomeFantasia, ''), nullif(cli.RazaoSocial, '')), p.Nome)) as Cliente ");
            sql.Append("    , l.DataBaixa as [Data Pagamento] ");
            sql.Append("    , SUM(ISNULL(pd.QTD_SELOS, 0)) as [Quantidade Selos] ");
            sql.Append("    , tp.Nome as Periodo ");
            sql.Append("    , CAST(CAST(SUM(ISNULL(pag.ValorPago, 0)) as decimal(18, 2)) as varchar(20)) [Valor Pago] ");
            sql.Append("    , 'NOVO' as SISTEMA ");
            sql.Append("    , l.Id ");
            sql.Append("    , ROW_NUMBER() OVER(partition by l.Id, u.nome, UPPER(COALESCE(COALESCE(nullif(cli.NomeFantasia, ''), nullif(cli.RazaoSocial, '')), p.Nome)) order by l.Id) IndexCobrancaId ");
            sql.Append("FROM Selo s ");
            sql.Append("INNER JOIN EmissaoSelo em on em.Id = s.Id ");
            sql.Append("INNER JOIN PedidoSelo pd on pd.id = em.PedidoSelo ");
            sql.Append("INNER JOIN Convenios c on c.Id = pd.Convenio ");
            sql.Append("INNER JOIN Cliente cli on cli.Id = pd.Cliente ");
            sql.Append("LEFT JOIN Pessoa p on p.Id = cli.Pessoa ");
            sql.Append("INNER JOIN TipoSelo tp on tp.id = pd.TipoSelo ");
            sql.Append("INNER JOIN Unidade u on u.Id = pd.Unidade ");
            sql.Append("LEFT JOIN Endereco e on e.Id = u.Endereco ");
            sql.Append("INNER JOIN LancamentoCobrancaPedidoSelo lp on lp.PedidoSelo = pd.Id ");
            sql.Append("INNER JOIN LancamentoCobranca l on l.Id = lp.LancamentoCobranca_id ");
            sql.Append("LEFT JOIN pagamento pag on pag.Recebimento = r.Id ");
            sql.Append("WHERE 1=1 ");
            sql.Append("    AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("     AND l.StatusLancamentoCobranca in (0, 1) ");
            //sql.Append("    AND l.DataBaixa is null ");
            //sql.Append("    AND (pag.Id is null or pag.StatusPagamento = 0) ");

            //if (unidade > 0)
            //    sql.Append($"    AND l.Unidade = {unidade} ");

            sql.Append("GROUP BY u.Nome, e.Logradouro, c.Descricao, tp.Nome, l.DataBaixa, cli.NomeFantasia, cli.RazaoSocial, p.Nome, l.Id ");

            //sql.Append("UNION ");
            //sql.Append("    SELECT ");
            //sql.Append("        c.NM_UNIDADE as [Unidade], ");
            //sql.Append("        '' as [Convênio], ");
            //sql.Append("        c.NM_CLIENTE as Cliente, ");
            //sql.Append("        c.DT_PAGTO as [Data Pagamento], ");
            //sql.Append("        0 as [Quantidade Selos], ");
            //sql.Append("        '' as [Periodo], ");
            //sql.Append("        SUM(CAST(c.VL_PAGTO as decimal(18,2))) as [Valor Total], ");
            //sql.Append("        'LEGADO' as SISTEMA, ");
            //sql.Append("        0 Id, ");
            //sql.Append("    FROM DADOS_COBRANCA_REL c ");
            //sql.Append("    WHERE 1=1 ");
            //sql.Append("        AND c.DT_VENCTO between :dataInicio and :dataFim ");
            //sql.Append("        AND c.TIPO = 'SELO' ");

            //sql.Append("        AND ISDATE(c.DT_PAGTO) <> 1 ");

            //sql.Append("    GROUP BY ");
            //sql.Append("        c.NM_UNIDADE, c.DT_PAGTO, c.NM_CLIENTE ");

            sql.Append(" ) la ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio);
            query.SetParameter("dataFim", dataFim);

            return ConverterResultadoRelatorio(query.List())?.ToList() ?? new List<DadosSelosVO>();
        }

        public IList<DadosSelosVO> ConverterResultadoRelatorio(IList results)
        {
            var lista = new List<DadosSelosVO>();
            foreach (object[] p in results)
            {
                var item = new DadosSelosVO
                {
                    Unidade = p[0]?.ToString() ?? string.Empty,
                    Convenio = p[1]?.ToString() ?? string.Empty,
                    Cliente = p[2]?.ToString() ?? string.Empty,
                    DataPagamento = p[3]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[3]?.ToString()),
                    QuantidadeSelos = p[4]?.ToString() == null ? 0 : Convert.ToDecimal(p[4]?.ToString()),
                    Periodo = p[5]?.ToString() ?? string.Empty,
                    ValorPago = p[6]?.ToString() == null ? 0 : Convert.ToDecimal(p[6]?.ToString()),
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}