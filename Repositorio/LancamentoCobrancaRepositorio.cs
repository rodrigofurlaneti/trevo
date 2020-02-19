using Core.Extensions;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositorio
{
    public class LancamentoCobrancaRepositorio : NHibRepository<LancamentoCobranca>, ILancamentoCobrancaRepositorio
    {
        public LancamentoCobrancaRepositorio(NHibContext context)
            : base(context)
        {
        }

        public IList<LancamentoCobranca> ListarLancamentosCobranca(int? idContaFinanceira, TipoServico? tipoServico,
            DateTime? dataVencimentoIni, DateTime? dataVencimentoFim, int? idCliente, string documento)
        {
            var sql = new StringBuilder();

            sql.Append("    SELECT L.Id FROM LANCAMENTOCOBRANCA L");
            sql.Append("    INNER JOIN CONTAFINANCEIRA AS CF ON CF.ID = L.CONTAFINANCEIRA");
            sql.Append("    INNER JOIN CLIENTE AS CLI ON CLI.ID = L.CLIENTE");
            sql.Append("    INNER JOIN PESSOA AS P ON CLI.PESSOA = P.ID");
            sql.Append("    LEFT JOIN PESSOADOCUMENTO AS PD ON PD.PESSOA = P.ID");
            sql.Append($"   LEFT JOIN DOCUMENTO AS DOC ON PD.DOCUMENTO_ID = DOC.ID AND (DOC.TIPO = {(int)TipoDocumento.Cpf} OR DOC.TIPO = {(int)TipoDocumento.Cnpj})");
            sql.Append("    WHERE 1 = 1 ");

            if (idCliente != null && idCliente != 0)
                sql.Append($" AND CLI.Id = " + idCliente);

            if (idContaFinanceira != null && idContaFinanceira != 0)
                sql.Append($" AND CF.Id = " + idContaFinanceira);

            if (tipoServico != null && tipoServico.Value > 0)
                sql.Append($" AND L.TipoServico = " + (int)tipoServico.Value);

            if (dataVencimentoIni.HasValue && dataVencimentoIni.Value > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                sql.Append($" AND L.DataVencimento >= :dataVencimentoIni");
            if (dataVencimentoFim.HasValue && dataVencimentoFim.Value > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                sql.Append($" AND L.DataVencimento <= :dataVencimentoFim");

            if (!string.IsNullOrEmpty(documento))
                sql.Append($" AND DOC.NUMERO like '%{documento.ExtractLettersAndNumbers().FormatCpfCnpj()}%'");

            var query = Session.CreateSQLQuery(sql.ToString());

            if (dataVencimentoIni.HasValue && dataVencimentoIni.Value > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                query.SetParameter("dataVencimentoIni", dataVencimentoIni.Value.Date);
            if (dataVencimentoFim.HasValue && dataVencimentoFim.Value > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                query.SetParameter("dataVencimentoFim", dataVencimentoFim.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            var retornoIds = query.List<int>()?.ToList() ?? new List<int>();

            if (!retornoIds.Any())
                return new List<LancamentoCobranca>();

            //return Session.CreateQuery($"SELECT L FROM LancamentoCobranca L WHERE ID in ({string.Join(",", retornoIds)})")?.List<LancamentoCobranca>() ?? new List<LancamentoCobranca>();
            return BuscarLancamentosCobranca(null, 0, 0, null, 0, retornoIds);
        }

        public IList<LancamentoCobranca> ListarLancamentosCobranca(int? idContaFinanceira, TipoServico? tipoServico, int? idUnidade,
            StatusLancamentoCobranca? statusLancamentoCobranca, TipoFiltroGeracaoCNAB? tipoFiltroGeracaoCNAB, int? supervisor, int? cliente,
            string dataDe, string dataAte)
        {
            var sql = new StringBuilder();

            sql.Append("    SELECT L.Id FROM LANCAMENTOCOBRANCA L");
            sql.Append("    INNER JOIN CONTAFINANCEIRA AS CF ON CF.ID = L.CONTAFINANCEIRA");
            sql.Append("    INNER JOIN CLIENTE AS CLI ON CLI.ID = L.CLIENTE");
            sql.Append("    INNER JOIN PESSOA AS P ON CLI.PESSOA = P.ID");
            sql.Append("    LEFT JOIN PESSOADOCUMENTO AS PD ON PD.PESSOA = P.ID");
            sql.Append("    LEFT JOIN DOCUMENTO AS DOC ON PD.DOCUMENTO_ID = DOC.ID AND DOC.TIPO = 2");
            sql.Append("    LEFT JOIN UNIDADE AS U ON U.ID = L.UNIDADE");
            sql.Append("    WHERE 1 = 1 AND L.DATABAIXA IS NULL");

            if (idContaFinanceira != null && idContaFinanceira != 0)
                sql.Append($" AND CF.Id = " + idContaFinanceira);

            if (tipoServico != null && tipoServico.Value > 0)
                sql.Append($" AND L.TipoServico = " + (int)tipoServico.Value);

            if (idUnidade != null && idUnidade != 0)
                sql.Append($" AND U.Id = " + idUnidade);

            if (supervisor != null && supervisor != 0)
                sql.Append($" AND U.Funcionario = " + supervisor);

            if (cliente != null && cliente != 0)
                sql.Append($" AND CLI.ID = " + cliente);

            if (statusLancamentoCobranca != null)
                sql.Append($" AND L.StatusLancamentoCobranca = " + (int)statusLancamentoCobranca.Value);

            var dataInicial = new DateTime();
            var dataFinal = new DateTime();
            if (!string.IsNullOrEmpty(dataDe) && DateTime.TryParse(dataDe, out dataInicial) && dataInicial > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                sql.Append($" AND L.DataVencimento >= :dataVencimentoIni");

            if (!string.IsNullOrEmpty(dataAte) && DateTime.TryParse(dataAte, out dataFinal) && dataFinal > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                sql.Append($" AND L.DataVencimento <= :dataVencimentoFim");

            sql.Append($" AND L.PossueCnab = " + (int)tipoFiltroGeracaoCNAB.Value);

            var query = Session.CreateSQLQuery(sql.ToString());

            if (!string.IsNullOrEmpty(dataDe) && dataInicial > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                query.SetParameter("dataVencimentoIni", dataInicial.Date);
            if (!string.IsNullOrEmpty(dataAte) && dataFinal > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                query.SetParameter("dataVencimentoFim", dataFinal.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            var retornoIds = query.List<int>()?.ToList() ?? new List<int>();

            return !retornoIds.Any()
                ? new List<LancamentoCobranca>()
                : BuscarLancamentosCobranca(null, 0, 0, null, 0, retornoIds) ?? new List<LancamentoCobranca>();
        }

        public IList<LancamentoCobranca> BuscarLancamentosCobranca(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, int numeroContrato, List<int> listaIds = null)
        {
            var sql = new StringBuilder();

            var colunas = new List<string> {
                "l.Id LancamentoId", "l.DataInsercao", "l.DataGeracao", "l.DataCompetencia", "l.DataVencimento", "l.DataBaixa", "l.StatusLancamentoCobranca",
                "l.ValorContrato", "l.TipoValorMulta", "l.ValorMulta", "l.TipoValorJuros", "l.ValorJuros", "l.TipoServico",
                "l.PossueCnab", "l.ContaFinanceira ContaFinanceiraId", "l.Cliente ClienteId", "l.Unidade UnidadeId",
                "cf.Descricao DescricaoContaFinanceira", "c.RazaoSocial", "c.NomeFantasia", "c.Pessoa PessoaId", "p.Nome NomePessoa", "u.Codigo CodigoUnidade", "u.Nome NomeUnidade",
                "e.Id IdEndereco", "e.Cep", "e.Logradouro", "e.Numero NumeroEndereco", "e.Complemento", "e.Bairro",
                "ue.Id IdUnidadeEndereco", "ue.Cep UnidadeCep", "ue.Logradouro UnidadeLogradouro", "ue.Numero UnidadeNumeroEndereco", "ue.Complemento UnidadeComplemento", "ue.Bairro UnidadeBairro",
                "e.Cidade_id", "ci.Descricao Cidade", "es.Id IdEstado", "es.Descricao Estado", "es.Sigla EstadoSigla",
                "docCPF.IdCPF", "docCPF.CPF", "docCNPJ.IdCNPJ", "docCNPJ.CNPJ",
                "l.Recebimento IdRecebimentoLancamento", "pag.VALOR_PAGO ValorPago", "pag.VALOR_DESCONTO ValorDesconto", "pag.DataPagamento", "pag.NumeroRecibo", "cm.Contratos", "pag.TipoDescontoAcrescimo"
            };

            sql.Append($"SELECT {string.Join(", ", colunas.ToArray())} ");
            sql.Append("FROM LancamentoCobranca l (NOLOCK) ");
            sql.Append("INNER JOIN ContaFinanceira cf (NOLOCK) on cf.Id = l.ContaFinanceira ");
            sql.Append("INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade ");
            sql.Append("LEFT JOIN Endereco ue (NOLOCK) on ue.Id = u.Endereco ");
            sql.Append("INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente ");
            sql.Append("LEFT JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append("LEFT JOIN PessoaEndereco pe (NOLOCK) on pe.Pessoa = p.Id ");
            sql.Append("LEFT JOIN Endereco e (NOLOCK) on e.Id = pe.Endereco_id ");
            sql.Append("LEFT JOIN Cidade ci (NOLOCK) on ci.Id = e.Cidade_id ");
            sql.Append("LEFT JOIN Estado es (NOLOCK) on es.Id = ci.Estado_id ");

            if (numeroContrato > 0)
            {
                sql.Append("INNER JOIN LancamentoCobrancaContratoMensalista lc (NOLOCK) on lc.LancamentoCobranca = l.Id ");
                sql.Append("INNER JOIN ContratoMensalista cml (NOLOCK) on cml.Id = lc.ContratoMensalista ");
            }

            sql.Append("OUTER APPLY ");
            sql.Append("(   ");
            sql.Append("    SELECT STRING_AGG(cm.NumeroContrato, ',') Contratos ");
            sql.Append("            , SUM(cm.Valor * IIF(cm.NumeroVagas is null or cm.NumeroVagas = 0, 1, cm.NumeroVagas)) Total ");
            sql.Append("            , SUM(IIF(cm.NumeroVagas is null or cm.NumeroVagas = 0, 1, cm.NumeroVagas)) TotalVagas ");
            sql.Append("    FROM ContratoMensalista cm (NOLOCK) ");
            sql.Append("    INNER JOIN LancamentoCobrancaContratoMensalista lc (NOLOCK) on lc.ContratoMensalista = cm.Id ");
            sql.Append("    WHERE lc.LancamentoCobranca = l.id ");
            sql.Append(") cm ");

            sql.Append("OUTER APPLY ( ");
            sql.Append("    SELECT TOP 1 d.Id IdCPF, d.Numero CPF ");
            sql.Append("    FROM PessoaDocumento pd (NOLOCK) ");
            sql.Append("    INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id ");
            sql.Append("    WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("    GROUP BY d.Id, d.Numero ");
            sql.Append("    ORDER BY d.Id desc ");
            sql.Append(" ) docCPF ");
            sql.Append("OUTER APPLY ( ");
            sql.Append("    SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ ");
            sql.Append("    FROM PessoaDocumento pd (NOLOCK) ");
            sql.Append("    INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id ");
            sql.Append("    WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("    GROUP BY d.Id, d.Numero ");
            sql.Append("    ORDER BY d.Id desc ");
            sql.Append(" ) docCNPJ ");

            sql.Append("OUTER APPLY ( ");
            sql.Append("    SELECT CAST(ISNULL(SUM(pg.ValorPago), 0) as decimal(18,2)) VALOR_PAGO ");
            sql.Append("            , CAST(ISNULL(SUM(COALESCE(NULLIF(pg.ValorDesconto, 0), 0)), 0) as decimal(18,2)) VALOR_DESCONTO ");
            sql.Append("            , STRING_AGG(pg.NumeroRecibo, ',') NumeroRecibo ");
            sql.Append("            , MAX(pg.DataPagamento) DataPagamento ");
            sql.Append("            , MAX(pg.TipoDescontoAcrescimo) TipoDescontoAcrescimo ");
            sql.Append("    FROM Pagamento pg ");
            sql.Append("    WHERE pg.Recebimento = l.Recebimento ");
            sql.Append(" ) pag ");
                       
            sql.Append("WHERE 1=1 ");

            if (listaIds != null && listaIds.Any())
                sql.Append($" AND l.Id in ({string.Join(",", listaIds)}) ");
            else
            {
                if (numeroContrato > 0)
                    sql.Append($" AND cml.NumeroContrato = {numeroContrato} ");

                if (unidade != 0)
                    sql.Append($" AND u.Id = {unidade} ");

                if (cliente != 0)
                    sql.Append($" AND c.Id = {cliente} ");

                if (status != null)
                    sql.Append($" AND l.StatusLancamentoCobranca = {(int)status.Value} ");

                if (!string.IsNullOrEmpty(dataVencimento))
                    sql.Append($" AND l.DataVencimento = :dataVencimento ");
            }

            sql.Append($" GROUP BY {string.Join(",", colunas.Select(x => (x.Contains(" ") ? x.Split(' ').First() : x)).ToList())} ");

            var query = Session.CreateSQLQuery(sql.ToString());

            if (!string.IsNullOrEmpty(dataVencimento))
                query.SetParameter("dataVencimento", DateTime.Parse(dataVencimento));

            return ConverterResultadoPesquisaEmObjetoSimples(query.List(), colunas)?.ToList() ?? new List<LancamentoCobranca>();
        }

        public IList<LancamentoCobranca> BuscarLancamentosCobrancaLinq(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, List<int> listaIds = null)
        {
            var predicate = PredicateBuilder.True<LancamentoCobranca>();

            if (status.HasValue && status > 0)
                predicate = predicate.And(x => x.StatusLancamentoCobranca == status);

            if (unidade > 0)
                predicate = predicate.And(x => x.Unidade.Id == unidade);

            if (cliente > 0)
                predicate = predicate.And(x => x.Cliente.Id == cliente);

            if (cliente > 0)
                predicate = predicate.And(x => x.Cliente.Id == cliente);

            if (!string.IsNullOrEmpty(dataVencimento))
            {
                var data = DateTime.Parse(dataVencimento);

                if (data > DateTime.MinValue)
                    predicate = predicate.And(x => x.DataVencimento.Date == data.Date);
            }

            if (listaIds != null && listaIds.Any())
                predicate = predicate.And(x => listaIds.Contains(x.Id));

            return ListBy(predicate).ToList();
        }

        public void UpdateDetalhesCNAB(List<LancamentoCobranca> listaLancamentos)
        {
            if (listaLancamentos == null || !listaLancamentos.Any())
                return;

            var sql = new StringBuilder();

            sql.Append($"UPDATE LancamentoCobranca Set PossueCnab = 1, StatusLancamentoCobranca = {(int)StatusLancamentoCobranca.EmAberto} WHERE Id in ({string.Join(",", listaLancamentos.Select(x => x.Id))}) ");

            var query = Session.CreateSQLQuery(sql.ToString());

            var result = query.ExecuteUpdate();
        }

        public IList<DadosLancamentosVO> BuscarLancamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            var sql = new StringBuilder();

            //sql.Append($"( SELECT 	u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade ");
            //sql.Append("            , 'Aluguel' AS[Tipo Serviço] ");
            //sql.Append("            , 1 as [Quantidade Total] ");
            //sql.Append("            , CAST(SUM(pag.ValorPago) as decimal(18, 2)) as [Valor Total] ");
            //sql.Append("            , UPPER(MAX(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome))) as Cliente ");
            //sql.Append("            , 'NOVO' as [Sistema] ");
            //sql.Append("    FROM LancamentoCobranca l ");
            //sql.Append("    INNER JOIN Cliente c on c.Id = l.Cliente ");
            //sql.Append("    LEFT JOIN Pessoa p on p.id = c.Pessoa ");
            //sql.Append("    INNER JOIN Unidade u on u.id = l.Unidade ");
            //sql.Append("    LEFT JOIN Endereco e on e.id = u.Endereco ");
            //sql.Append("    LEFT JOIN pagamento pag on pag.Recebimento = l.Recebimento ");
            //sql.Append("    WHERE 1=1 ");
            //sql.Append("        AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");

            ////sql.Append("        AND pag.ValorPago > 0 ");
            //sql.Append("        AND l.StatusLancamentoCobranca = 8 ");

            //sql.Append("        AND l.TipoServico = 3 ");

            ////if (unidade > 0)
            ////    sql.Append($"        AND l.Unidade = {unidade} ");

            //sql.Append("    GROUP BY  ");
            //sql.Append("        u.Nome, e.Logradouro ");
            //sql.Append("UNION  ");

            sql.Append("( SELECT   ");
            sql.Append("     u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade   ");
            sql.Append("     , CASE  ");
            sql.Append("         WHEN l.TipoServico = 1 THEN 'Mensalista' 		 ");
            sql.Append("         WHEN l.TipoServico = 2 THEN 'Convênio'   		 ");
            sql.Append("         WHEN l.TipoServico = 3 THEN 'Aluguel'    		 ");
            sql.Append("         WHEN l.TipoServico = 4 THEN 'Evento'     		 ");
            sql.Append("         WHEN l.TipoServico = 5 THEN 'Avulso'     		 ");
            sql.Append("         WHEN l.TipoServico = 6 THEN 'Locação'			 ");
            sql.Append("         WHEN l.TipoServico = 7 THEN 'Seguro Reembolso'  ");
            sql.Append("         WHEN l.TipoServico = 8 THEN 'Outros'			 ");
            sql.Append("         WHEN l.TipoServico = 9 THEN 'Cartao de Acesso'  ");
            sql.Append("         END AS [Tipo Serviço]  ");
            sql.Append("     , CASE ");
            sql.Append(" 		WHEN l.TipoServico = 1 THEN MAX(ISNULL(lcm.NumeroVagas, 0)) ");
            sql.Append(" 		WHEN l.TipoServico = 2 THEN SUM(ISNULL(pdl.Quantidade, 0)) ");
            sql.Append(" 		ELSE COUNT(ISNULL(u.Nome, 0)) ");
            sql.Append(" 		END AS [Quantidade Total]  ");
            sql.Append("     , CAST(SUM(ISNULL(pag.ValorPago, 0)) as decimal(18, 2)) as [Valor Total]  ");
            sql.Append("     , UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente ");
            sql.Append("     , 'NOVO' as [Sistema]  ");
            sql.Append("     , l.id CobrancaId  ");
            sql.Append(" FROM LancamentoCobranca l   ");
            sql.Append(" INNER JOIN Cliente c on c.Id = l.Cliente  ");
            sql.Append(" INNER JOIN Pessoa p on p.id = c.Pessoa  ");
            sql.Append(" INNER JOIN Unidade u on u.id = l.Unidade  ");
            sql.Append(" LEFT JOIN Endereco e on e.id = u.Endereco ");
            sql.Append(" OUTER APPLY ( ");
            sql.Append(" 				SELECT DISTINCT ");
            sql.Append(" 					MAX(cm.Id) Id ");
            sql.Append(" 					, SUM(cm.NumeroVagas) NumeroVagas ");
            sql.Append(" 				FROM LancamentoCobrancaContratoMensalista lc ");
            sql.Append(" 				INNER JOIN ContratoMensalista cm on cm.Id = lc.ContratoMensalista ");
            sql.Append(" 				WHERE lc.LancamentoCobranca = l.Id ");
            sql.Append(" 			) lcm ");
            sql.Append(" OUTER APPLY ( ");
            sql.Append(" 				SELECT DISTINCT ");
            sql.Append(" 					pd.Id ");
            sql.Append(" 					, pd.Quantidade ");
            sql.Append(" 				FROM LancamentoCobrancaPedidoSelo lpd ");
            sql.Append(" 				INNER JOIN PedidoSelo pd on pd.Id = lpd.PedidoSelo ");
            sql.Append(" 				WHERE lpd.LancamentoCobranca_id = l.Id ");
            sql.Append(" 			) pdl ");
            sql.Append(" OUTER APPLY (   ");
            sql.Append("                 SELECT  ");
            sql.Append("                     SUM(cast(pg.ValorPago as decimal(18, 2))) ValorPago  ");
            sql.Append("                     , SUM(cast(pg.ValorDesconto as decimal(18, 2))) ValorDesconto  ");
            sql.Append("                     , MAX(pg.DataPagamento) DataPagamento  ");
            sql.Append("                 FROM Pagamento pg(NOLOCK)  ");
            sql.Append("                 WHERE pg.Recebimento = l.Recebimento  ");
            sql.Append("             ) pag   ");
            sql.Append(" WHERE 1=1  ");
            sql.Append("     AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("     AND l.StatusLancamentoCobranca = 8  ");
            //sql.Append("     AND l.TipoServico <> 3 ");
            
            //if (unidade > 0)
            //    sql.Append($"        AND l.Unidade = {unidade} ");

            sql.Append("    GROUP BY  ");
            sql.Append("        u.Nome, e.Logradouro, l.TipoServico, c.NomeFantasia, c.RazaoSocial, p.Nome, l.Id  ");
            sql.Append("UNION ");
            sql.Append("    SELECT ");
            sql.Append("        c.NM_UNIDADE as [Unidade], ");
            sql.Append("        c.TIPO as [Tipo Serviço], ");
            sql.Append("        COUNT(c.NM_UNIDADE) as [Quantidade Total], ");
            sql.Append("        SUM(CAST(ISNULL(c.VL_PAGTO, 0.00) as decimal(18,2))) as [Valor Total], ");
            sql.Append("        MAX(c.NM_CLIENTE) as Cliente, ");
            sql.Append("        'LEGADO' as [Sistema], ");
            sql.Append("        0 CobrancaId ");
            sql.Append("    FROM DADOS_COBRANCA_REL c ");
            sql.Append("    WHERE 1=1 ");
            sql.Append("        AND c.DT_VENCTO between :dataInicio and :dataFim ");

            sql.Append("        AND ISDATE(c.DT_PAGTO) = 1 ");

            sql.Append("    GROUP BY ");
            sql.Append("        c.NM_UNIDADE, c.TIPO ");
            sql.Append(") ");
            sql.Append("ORDER BY  ");
            sql.Append("      Unidade ");
            sql.Append("    , [Tipo Serviço] ");
            sql.Append("    , Cliente ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoRelatorio(query.List())?.ToList() ?? new List<DadosLancamentosVO>();
        }

        public IList<DadosLancamentosVO> BuscarLancamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            var sql = new StringBuilder();

            //sql.Append($"( SELECT 	u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade ");
            //sql.Append("            , 'Aluguel' AS[Tipo Serviço] ");
            //sql.Append("            , 1 as [Quantidade Total] ");
            //sql.Append("            , CAST(SUM(pag.ValorPago) as decimal(18, 2)) as [Valor Total] ");
            //sql.Append("            , UPPER(MAX(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome))) as Cliente ");
            //sql.Append("            , 'NOVO' as [Sistema] ");
            //sql.Append("    FROM LancamentoCobranca l ");
            //sql.Append("    INNER JOIN Cliente c on c.Id = l.Cliente ");
            //sql.Append("    LEFT JOIN Pessoa p on p.id = c.Pessoa ");
            //sql.Append("    INNER JOIN Unidade u on u.id = l.Unidade ");
            //sql.Append("    LEFT JOIN Endereco e on e.id = u.Endereco ");
            //sql.Append("    LEFT JOIN pagamento pag on pag.Recebimento = l.recebimento ");
            //sql.Append("    WHERE 1=1 ");
            //sql.Append("        AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");

            ////sql.Append("        AND pag.Id is null ");
            //sql.Append("     AND l.StatusLancamentoCobranca in (0, 1) ");

            //sql.Append("        AND l.TipoServico = 3 ");

            ////if (unidade > 0)
            ////    sql.Append($"        AND l.Unidade = {unidade} ");


            //sql.Append("    GROUP BY  ");
            //sql.Append("        u.Nome, e.Logradouro ");
            //sql.Append("UNION  ");

            sql.Append("( SELECT   ");
            sql.Append("     u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade   ");
            sql.Append("     , CASE  ");
            sql.Append("         WHEN l.TipoServico = 1 THEN 'Mensalista' 		 ");
            sql.Append("         WHEN l.TipoServico = 2 THEN 'Convênio'   		 ");
            sql.Append("         WHEN l.TipoServico = 3 THEN 'Aluguel'    		 ");
            sql.Append("         WHEN l.TipoServico = 4 THEN 'Evento'     		 ");
            sql.Append("         WHEN l.TipoServico = 5 THEN 'Avulso'     		 ");
            sql.Append("         WHEN l.TipoServico = 6 THEN 'Locação'			 ");
            sql.Append("         WHEN l.TipoServico = 7 THEN 'Seguro Reembolso'  ");
            sql.Append("         WHEN l.TipoServico = 8 THEN 'Outros'			 ");
            sql.Append("         WHEN l.TipoServico = 9 THEN 'Cartao de Acesso'  ");
            sql.Append("         END AS [Tipo Serviço]  ");
            sql.Append("     , CASE ");
            sql.Append(" 		WHEN l.TipoServico = 1 THEN MAX(ISNULL(lcm.NumeroVagas, 0)) ");
            sql.Append(" 		WHEN l.TipoServico = 2 THEN SUM(ISNULL(pdl.Quantidade, 0)) ");
            sql.Append(" 		ELSE COUNT(ISNULL(u.Nome, 0)) ");
            sql.Append(" 		END AS [Quantidade Total]  ");
            sql.Append("     , CAST(SUM(ISNULL(pag.ValorPago, 0)) as decimal(18, 2)) as [Valor Total]  ");
            //sql.Append(" 	 , CAST(SUM(ISNULL(l.ValorContrato, 0)) as decimal(18, 2)) TotalValorContrato ");
            sql.Append("     , UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente  ");
            sql.Append("     , 'NOVO' as [Sistema]  ");
            sql.Append("     , l.id CobrancaId  ");
            sql.Append(" FROM LancamentoCobranca l   ");
            sql.Append(" INNER JOIN Cliente c on c.Id = l.Cliente  ");
            sql.Append(" INNER JOIN Pessoa p on p.id = c.Pessoa  ");
            sql.Append(" INNER JOIN Unidade u on u.id = l.Unidade  ");
            sql.Append(" LEFT JOIN Endereco e on e.id = u.Endereco ");
            sql.Append(" OUTER APPLY ( ");
            sql.Append(" 				SELECT DISTINCT ");
            sql.Append(" 					MAX(cm.Id) Id ");
            sql.Append(" 					, SUM(cm.NumeroVagas) NumeroVagas");
            sql.Append(" 				FROM LancamentoCobrancaContratoMensalista lc ");
            sql.Append(" 				INNER JOIN ContratoMensalista cm on cm.Id = lc.ContratoMensalista ");
            sql.Append(" 				WHERE lc.LancamentoCobranca = l.Id ");
            sql.Append(" 			) lcm ");
            sql.Append(" OUTER APPLY ( ");
            sql.Append(" 				SELECT DISTINCT ");
            sql.Append(" 					pd.Id ");
            sql.Append(" 					, pd.Quantidade ");
            sql.Append(" 				FROM LancamentoCobrancaPedidoSelo lpd ");
            sql.Append(" 				INNER JOIN PedidoSelo pd on pd.Id = lpd.PedidoSelo ");
            sql.Append(" 				WHERE lpd.LancamentoCobranca_id = l.Id ");
            sql.Append(" 			) pdl ");
            sql.Append(" OUTER APPLY (   ");
            sql.Append("                 SELECT  ");
            sql.Append("                     SUM(cast(pg.ValorPago as decimal(18, 2))) ValorPago  ");
            sql.Append("                     , SUM(cast(pg.ValorDesconto as decimal(18, 2))) ValorDesconto  ");
            sql.Append("                     , MAX(pg.DataPagamento) DataPagamento  ");
            sql.Append("                 FROM Pagamento pg(NOLOCK)  ");
            sql.Append("                 WHERE pg.Recebimento = l.Recebimento  ");
            sql.Append("             ) pag   ");
            sql.Append(" WHERE 1=1  ");
            sql.Append("     AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");

            //sql.Append("        AND pag.Id is null ");
            sql.Append("     AND l.StatusLancamentoCobranca in (0, 1) ");

            //sql.Append("     AND l.TipoServico <> 3 ");

            //if (unidade > 0)
            //    sql.Append($"        AND l.Unidade = {unidade} ");

            sql.Append("    GROUP BY  ");
            sql.Append("        u.Nome, e.Logradouro, l.TipoServico, c.NomeFantasia, c.RazaoSocial, p.Nome, l.Id  ");
            sql.Append("UNION ");
            sql.Append("    SELECT ");
            sql.Append("        c.NM_UNIDADE as [Unidade], ");
            sql.Append("        c.TIPO as [Tipo Serviço], ");
            sql.Append("        COUNT(c.NM_UNIDADE) as [Quantidade Total], ");
            sql.Append("        SUM(CAST(ISNULL(c.VL_PAGTO, 0.00) as decimal(18,2))) as [Valor Total], ");
            sql.Append("        MAX(c.NM_CLIENTE) as Cliente, ");
            sql.Append("        'LEGADO' as [Sistema], ");
            sql.Append("        0 CobrancaId ");
            sql.Append("    FROM DADOS_COBRANCA_REL c ");
            sql.Append("    WHERE 1=1 ");
            sql.Append("        AND c.DT_VENCTO between :dataInicio and :dataFim ");

            sql.Append("        AND ISDATE(c.DT_PAGTO) <> 1 ");

            sql.Append("    GROUP BY ");
            sql.Append("        c.NM_UNIDADE, c.TIPO ");
            sql.Append(")  ");
            sql.Append("ORDER BY  ");
            sql.Append("      Unidade ");
            sql.Append("    , [Tipo Serviço] ");
            sql.Append("    , Cliente ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoRelatorio(query.List())?.ToList() ?? new List<DadosLancamentosVO>();
        }

        public IList<DadosPagamentoVO> BuscarPagamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico tipoServico, int unidade)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT DISTINCT ");
            sql.Append("    u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade ");
            sql.Append("    , UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente ");
            sql.Append("    , '' as Placa ");
            sql.Append("    , NULL as Contrato ");
            sql.Append("    , 0 as Boleto ");
            sql.Append("    , NULL as [Data Emissão] ");
            sql.Append("    , l.DataVencimento as [Data Vencto] ");
            sql.Append("    , NULL as [Data Inicio Vaga] ");
            sql.Append("    , '' as [Qtde Vagas] ");
            sql.Append("    , u.NumeroVaga as [Total Vagas Unidade] ");
            sql.Append("    , pag.ValorPago as Pago ");
            sql.Append("    , 'NOVO LC' as SISTEMA ");
            sql.Append("    , 'true' as BltPago ");
            sql.Append("    ,  dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal ");
            sql.Append("    , l.ValorContrato as ValorContrato ");
            sql.Append("    , pag.DataPagamento as DataPagamento ");
            sql.Append("    , pag.ValorDesconto as Desconto ");
            sql.Append("    , l.DataCompetencia as DataCompetencia ");
            sql.Append("    , l.Id CobrancaId ");
            sql.Append(" FROM LancamentoCobranca l (NOLOCK) ");
            sql.Append(" INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade ");
            sql.Append(" INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente ");
            sql.Append(" INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append(" OUTER APPLY (  ");
            sql.Append("     SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("     FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("     INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("     WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("     GROUP BY d.Id, d.Numero  ");
            sql.Append("     ORDER BY d.Id desc  ");
            sql.Append("  ) docCPF  ");
            sql.Append(" OUTER APPLY (  ");
            sql.Append("     SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ  ");
            sql.Append("     FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("     INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("     WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("     GROUP BY d.Id, d.Numero  ");
            sql.Append("     ORDER BY d.Id desc  ");
            sql.Append("  ) docCNPJ  ");
            sql.Append(" LEFT JOIN Endereco e (NOLOCK) on e.Id = u.Endereco ");
            sql.Append(" INNER JOIN Recebimento r (NOLOCK) on r.Id = l.Recebimento ");
            sql.Append(" OUTER APPLY ( ");
            sql.Append("                 SELECT ");
            sql.Append("                     SUM(cast(pg.ValorPago as decimal(18,2))) ValorPago ");
            sql.Append("                     , SUM(cast(pg.ValorDesconto as decimal(18,2))) ValorDesconto ");
            sql.Append("                     , MAX(pg.DataPagamento) DataPagamento ");
            sql.Append("                 FROM Pagamento pg (NOLOCK) ");
            sql.Append("                 WHERE pg.Recebimento = r.Id ");
            sql.Append("             ) pag ");
            sql.Append(" WHERE 1=1 ");
            sql.Append("     AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("     AND l.StatusLancamentoCobranca = 8 ");
            //sql.Append("    AND l.DataBaixa is not null ");

            sql.Append($"        AND l.TipoServico = {(int)tipoServico} ");

            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            ////sql.Append("    --AND pag.VLR_PAGTO > 0 ");
            //sql.Append("UNION ");
            //sql.Append("    SELECT DISTINCT ");
            //sql.Append("        c.NM_UNIDADE as [Unidade] ");
            //sql.Append("        , c.NM_CLIENTE as [Cliente] ");
            //sql.Append("        , c.NM_PLACA as [Placa] ");
            //sql.Append("        , c.NR_CONTRATO as [Contrato] ");
            //sql.Append("        , c.VL_BOLETO as [Boleto] ");
            //sql.Append("        , c.DT_EMISSAO as [Data Emissão] ");
            //sql.Append("        , c.DT_VENCTO as [Data Vencto] ");
            //sql.Append("        , c.DT_INI_VAGA as [Data Inicio Vaga] ");
            //sql.Append("        , c.QTDE_VAGAS as [Qtde Vagas] ");
            //sql.Append("        , c.QTDE_VAGAS_ZERADAS as [Total Vagas Unidade] ");
            //sql.Append("        , c.VL_PAGTO as [Pago] ");
            //sql.Append("        , 'LEGADO' as SISTEMA ");
            //sql.Append("    FROM DADOS_COBRANCA_REL c ");
            //sql.Append("    WHERE 1=1 ");
            //sql.Append("        AND c.DT_VENCTO between :dataInicio and :dataFim ");
            //sql.Append("        AND ISDATE(c.DT_PAGTO) = 1 ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoPagamentoRelatorio(query.List(), tipoServico)?.ToList() ?? new List<DadosPagamentoVO>();
        }
        public IList<DadosPagamentoVO> BuscarPagamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico tipoServico, int unidade, bool acrescentarCancelados = false)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT DISTINCT ");
            sql.Append("        u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade ");
            sql.Append("        , UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente ");
            sql.Append("        , '' as Placa ");
            sql.Append("        , NULL as Contrato ");
            sql.Append("        , 0 as Boleto ");
            sql.Append("        , NULL as [Data Emissão] ");
            sql.Append("        , l.DataVencimento as [Data Vencto] ");
            sql.Append("        , NULL as [Data Inicio Vaga] ");
            sql.Append("        , '' as [Qtde Vagas] ");
            sql.Append("        , u.NumeroVaga as [Total Vagas Unidade] ");
            sql.Append("        , pag.ValorPago as Pago ");
            sql.Append("        , 'NOVO LC' as SISTEMA ");
            sql.Append("        , 'false' as BltPago ");
            sql.Append("        ,  dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal ");
            sql.Append("        , l.ValorContrato as ValorContrato ");
            sql.Append("        , NULL as DataPagamento ");
            sql.Append("        , pag.ValorDesconto as Desconto ");
            sql.Append("        , l.DataCompetencia as DataCompetencia ");
            sql.Append("        , l.Id CobrancaId ");
            sql.Append("    FROM LancamentoCobranca l (NOLOCK) ");
            sql.Append("    INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade ");
            sql.Append("    LEFT JOIN Endereco e (NOLOCK) on e.Id = u.Endereco ");
            sql.Append("    INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente ");
            sql.Append("    INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCPF  ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCNPJ  ");
            sql.Append("    LEFT JOIN Pagamento pag (NOLOCK) on pag.Recebimento = l.Recebimento ");
            sql.Append("    WHERE 1=1 ");
            sql.Append("        AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            //sql.Append("        AND (pag.Id is null or pag.StatusPagamento = 0) ");

            //if (acrescentarCancelados)
            //    sql.Append("        AND l.StatusLancamentoCobranca in (0, 1, 5) ");
            //else
                sql.Append("        AND l.StatusLancamentoCobranca in (0, 1) ");
            
            sql.Append($"        AND l.TipoServico = {(int)tipoServico} ");

            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            //sql.Append("        AND cm.NumeroContrato not in (select NR_CONTRATO from DADOS_COBRANCA_REL) ");
            //sql.Append("UNION ");
            //sql.Append("    SELECT DISTINCT ");
            //sql.Append("        c.NM_UNIDADE as [Unidade] ");
            //sql.Append("        , c.NM_CLIENTE as [Cliente] ");
            //sql.Append("        , c.NM_PLACA as [Placa] ");
            //sql.Append("        , c.NR_CONTRATO as [Contrato] ");
            //sql.Append("        , c.VL_BOLETO as [Boleto] ");
            //sql.Append("        , c.DT_EMISSAO as [Data Emissão] ");
            //sql.Append("        , c.DT_VENCTO as [Data Vencto] ");
            //sql.Append("        , c.DT_INI_VAGA as [Data Inicio Vaga] ");
            //sql.Append("        , c.QTDE_VAGAS as [Qtde Vagas] ");
            //sql.Append("        , c.QTDE_VAGAS_ZERADAS as [Total Vagas Unidade] ");
            //sql.Append("        , c.VL_PAGTO as [Pago] ");
            //sql.Append("        , 'LEGADO' as SISTEMA ");
            //sql.Append("    FROM DADOS_COBRANCA_REL c ");
            //sql.Append("    WHERE 1=1 ");
            //sql.Append("        AND c.DT_VENCTO between :dataInicio and :dataFim ");
            //sql.Append("        AND ISDATE(c.DT_PAGTO) <> 1 ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoPagamentoRelatorio(query.List(), tipoServico)?.ToList() ?? new List<DadosPagamentoVO>();
        }

        public IList<DadosPagamentoVO> BuscarPagamentosPedidoLocacaoPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            var sql = new StringBuilder();

            var colunas = new List<string> {
                "u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade"
                , "UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente"
                , "pl.Id as Contrato"
                , "l.ValorContrato as [Valor Contrato]"
                , "pl.Valor as [Valor Pedido]"
                , "pl.ValorTotal as [Valor Total Pedido]"
                , "l.DataGeracao as [Data Emissão]"
                , "l.DataVencimento as [Data Vencto]"
                , "pl.DataVigenciaInicio as [Data Inicio]"
                , "pl.DataVigenciaFim as [Data Fim]"
                , "pl.TipoReajuste as [Tipo Reajuste]"
                , "pl.ValorReajuste as [Valor Reajuste]"
                , "pl.DataReajuste as [Reajuste]"
                , "pag.ValorPago as Pago"
                , "pag.ValorDesconto as Desconto"
                , "'true' as BltPago"
                , "dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal"
                , "pag.DataPagamento as DataPagamento"
                , "l.ValorMulta as VlrMulta"
                , "l.DataCompetencia as DataCompetencia"
                , "l.Id CobrancaId"
            };

            sql.Append($"SELECT DISTINCT {string.Join(", ", colunas.ToArray())} ");
            sql.Append("FROM LancamentoCobranca l (NOLOCK) ");
            sql.Append("INNER JOIN PedidoLocacaoLancamentoCobranca pll (NOLOCK) on pll.LancamentoCobrancaId = l.Id ");
            sql.Append("INNER JOIN PedidoLocacao pl(NOLOCK) on pl.Id = pll.PedidoLocacaoId ");
            sql.Append("INNER JOIN Unidade u(NOLOCK) on u.Id = l.Unidade ");
            sql.Append("INNER JOIN Cliente c(NOLOCK) on c.Id = l.Cliente ");
            sql.Append("INNER JOIN Pessoa p(NOLOCK) on p.Id = c.Pessoa ");
            sql.Append("OUTER APPLY (  ");
            sql.Append("    SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("    FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("    INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("    WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("    GROUP BY d.Id, d.Numero  ");
            sql.Append("    ORDER BY d.Id desc  ");
            sql.Append(" ) docCPF  ");
            sql.Append("OUTER APPLY (  ");
            sql.Append("    SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ  ");
            sql.Append("    FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("    INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("    WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("    GROUP BY d.Id, d.Numero  ");
            sql.Append("    ORDER BY d.Id desc  ");
            sql.Append(" ) docCNPJ  ");
            sql.Append("LEFT JOIN Endereco e(NOLOCK) on e.Id = u.Endereco ");
            sql.Append("OUTER APPLY ( ");
            sql.Append("                SELECT ");
            sql.Append("                    SUM(cast(pg.ValorPago as decimal(18,2))) ValorPago ");
            sql.Append("                    , SUM(cast(pg.ValorDesconto as decimal(18,2))) ValorDesconto ");
            sql.Append("                    , MAX(pg.DataPagamento) DataPagamento ");
            sql.Append("                FROM Pagamento pg (NOLOCK) ");
            sql.Append("                WHERE pg.Recebimento = l.Recebimento ");
            sql.Append("            ) pag ");
            sql.Append("WHERE 1=1 ");
            sql.Append("    AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("    AND l.StatusLancamentoCobranca = 8 ");
            sql.Append("    AND l.TipoServico = 6 ");

            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoPagamentoPedidoLocacaoRelatorio(query.List(), colunas, TipoServico.Locacao)?.ToList() ?? new List<DadosPagamentoVO>();
        }
        public IList<DadosPagamentoVO> BuscarPagamentosPedidoLocacaoEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade, bool acrescentarCancelados = false)
        {
            var sql = new StringBuilder();

            var colunas = new List<string>
            {
                "l.Id CobrancaId"
                , "u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade"
                , "UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente"
                , "pl.Id as Contrato"
                , "l.ValorContrato as [Valor Contrato]"
                , "pl.Valor as [Valor Pedido]"
                , "pl.ValorTotal as [Valor Total Pedido]"
                , "l.DataGeracao as [Data Emissão]"
                , "l.DataVencimento as [Data Vencto]"
                , "pl.DataVigenciaInicio as [Data Inicio]"
                , "pl.DataVigenciaFim as [Data Fim]"
                , "pl.TipoReajuste as [Tipo Reajuste]"
                , "pl.ValorReajuste as [Valor Reajuste]"
                , "pl.DataReajuste as [Reajuste]"
                , "'false' as BltPago"
                , "dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal"
                , "NULL as DataPagamento"
                , "l.ValorMulta as VlrMulta"
                , "0 as Pago"
                , "0 as Desconto"
                , "l.DataCompetencia as DataCompetencia"
            };

            sql.Append($"SELECT DISTINCT {string.Join(", ", colunas.ToArray())} ");
            sql.Append("    FROM LancamentoCobranca l (NOLOCK) ");
            sql.Append("    INNER JOIN PedidoLocacaoLancamentoCobranca pll (NOLOCK) on pll.LancamentoCobrancaId = l.Id ");
            sql.Append("    INNER JOIN PedidoLocacao pl(NOLOCK) on pl.Id = pll.PedidoLocacaoId ");
            sql.Append("    INNER JOIN Unidade u(NOLOCK) on u.Id = l.Unidade ");
            sql.Append("    INNER JOIN Cliente c(NOLOCK) on c.Id = l.Cliente ");
            sql.Append("    INNER JOIN Pessoa p(NOLOCK) on p.Id = c.Pessoa ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCPF  ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCNPJ  ");
            sql.Append("    LEFT JOIN Endereco e(NOLOCK) on e.Id = u.Endereco  ");
            sql.Append("    LEFT JOIN Pagamento pag (NOLOCK) on pag.Recebimento = l.Recebimento ");
            sql.Append("    WHERE 1=1 ");
            sql.Append("        AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("        AND l.TipoServico = 6 ");

            //if (acrescentarCancelados)
            //    sql.Append("        AND l.StatusLancamentoCobranca in (0, 1, 5) ");
            //else
                sql.Append("        AND l.StatusLancamentoCobranca in (0, 1) ");

            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoPagamentoPedidoLocacaoRelatorio(query.List(), colunas, TipoServico.Locacao)?.ToList() ?? new List<DadosPagamentoVO>();
        }

        public IList<DadosPagamentoVO> BuscarPagamentosPedidoSeloPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            var sql = new StringBuilder();

            var colunas = new List<string>
            {
                  "u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade"
                , "UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente"
                , "ps.Id as Contrato"
                , "l.ValorContrato as Boleto"
                , "l.DataGeracao as [Data Emissão]"
                , "l.DataVencimento as [Data Vencto]"
                , "ps.Quantidade as [Quantidade]"
                , "ps.ValidadePedido as [Validade Pedido]"
                , "ts.Nome as [Tipo Selo]"
                , "ps.TipoPedidoSelo as [Tipo Pedido Selo]"
                , "pag.ValorPago as Pago"
                , "pag.ValorDesconto as Desconto"
                , "'true' as BltPago"
                , "dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal"
                , "pag.DataPagamento as DataPagamento"
                , "con.Descricao as NomeConvenio"
                , "l.DataCompetencia as DataCompetencia"
                , "l.Id CobrancaId"
            };

            sql.Append($"SELECT DISTINCT {string.Join(", ", colunas.ToArray())} ");
            sql.Append("FROM LancamentoCobranca l (NOLOCK) ");
            sql.Append("LEFT JOIN LancamentoCobrancaPedidoSelo lps (NOLOCK) on lps.LancamentoCobranca_id = l.Id ");
            sql.Append("LEFT JOIN PedidoSelo ps (NOLOCK) on ps.Id = lps.PedidoSelo ");
            sql.Append("LEFT JOIN TipoSelo ts (NOLOCK) on ts.Id = ps.TipoSelo ");
            sql.Append("LEFT JOIN EmissaoSelo es (NOLOCK) on es.PedidoSelo = ps.Id ");
            sql.Append("INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade ");
            sql.Append("INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente ");
            sql.Append("INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append("LEFT JOIN Convenios con (NOLOCK) on con.Id = ps.Convenio ");
            sql.Append("OUTER APPLY (  ");
            sql.Append("    SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("    FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("    INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("    WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("    GROUP BY d.Id, d.Numero  ");
            sql.Append("    ORDER BY d.Id desc  ");
            sql.Append(" ) docCPF  ");
            sql.Append("OUTER APPLY (  ");
            sql.Append("    SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ  ");
            sql.Append("    FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("    INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("    WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("    GROUP BY d.Id, d.Numero  ");
            sql.Append("    ORDER BY d.Id desc  ");
            sql.Append(" ) docCNPJ  ");
            sql.Append("LEFT JOIN Endereco e (NOLOCK) on e.Id = u.Endereco ");
            sql.Append("OUTER APPLY ( ");
            sql.Append("                SELECT ");
            sql.Append("                    SUM(cast(pg.ValorPago as decimal(18,2))) ValorPago ");
            sql.Append("                    , SUM(cast(pg.ValorDesconto as decimal(18,2))) ValorDesconto ");
            sql.Append("                    , MAX(pg.DataPagamento) DataPagamento ");
            sql.Append("                FROM Pagamento pg (NOLOCK) ");
            sql.Append("                WHERE pg.Recebimento = l.Recebimento ");
            sql.Append("            ) pag ");
            sql.Append("WHERE 1=1 ");
            sql.Append("    AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("    AND l.StatusLancamentoCobranca = 8 ");
            sql.Append("    AND l.TipoServico = 2 ");

            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoPagamentoPedidoSeloRelatorio(query.List(), colunas, TipoServico.Convenio)?.ToList() ?? new List<DadosPagamentoVO>();
        }
        public IList<DadosPagamentoVO> BuscarPagamentosPedidoSeloEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade, bool acrescentarCancelados = false)
        {
            var sql = new StringBuilder();

            var colunas = new List<string>
            {
                "l.Id CobrancaId"
                , "u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade"
                , "UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente"
                , "ps.Id as Contrato"
                , "l.ValorContrato as Boleto"
                , "l.DataGeracao as [Data Emissão]"
                , "l.DataVencimento as [Data Vencto]"
                , "ps.Quantidade as [Quantidade]"
                , "ps.ValidadePedido as [Validade Pedido]"
                , "ts.Nome as [Tipo Selo]"
                , "ps.TipoPedidoSelo as [Tipo Pedido Selo]"
                , "'false' as BltPago"
                , "dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal"
                , "NULL as DataPagamento"
                , "con.Descricao as NomeConvenio"
                , "0 as Pago"
                , "0 as Desconto"
                , "l.DataCompetencia as DataCompetencia"
            };

            sql.Append($"SELECT DISTINCT {string.Join(", ", colunas.ToArray())} ");
            sql.Append("    FROM LancamentoCobranca l (NOLOCK) ");
            sql.Append("    LEFT JOIN LancamentoCobrancaPedidoSelo lps (NOLOCK) on lps.LancamentoCobranca_id = l.Id ");
            sql.Append("    LEFT JOIN PedidoSelo ps (NOLOCK) on ps.Id = lps.PedidoSelo ");
            sql.Append("    LEFT JOIN TipoSelo ts (NOLOCK) on ts.Id = ps.TipoSelo ");
            sql.Append("    LEFT JOIN EmissaoSelo es (NOLOCK) on es.PedidoSelo = ps.Id ");
            sql.Append("    INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade ");
            sql.Append("    INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente ");
            sql.Append("    INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append("    LEFT JOIN Convenios con (NOLOCK) on con.Id = ps.Convenio ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCPF  ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCNPJ  ");
            sql.Append("    LEFT JOIN Endereco e (NOLOCK) on e.Id = u.Endereco ");
            //sql.Append("    LEFT JOIN Pagamento pag (NOLOCK) on pag.Recebimento = l.Recebimento ");
            sql.Append("    WHERE 1=1 ");
            sql.Append("        AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("        AND l.TipoServico = 2 ");

            //if (acrescentarCancelados)
            //    sql.Append("        AND l.StatusLancamentoCobranca in (0, 1, 5) ");
            //else
                sql.Append("        AND l.StatusLancamentoCobranca in (0, 1) ");

            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoPagamentoPedidoSeloRelatorio(query.List(), colunas, TipoServico.Convenio)?.ToList() ?? new List<DadosPagamentoVO>();
        }

        public IList<DadosPagamentoVO> BuscarPagamentosContratoMensalistasPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            var sql = new StringBuilder();
            
            var colunas = new List<string>
            {
                "l.Id CobrancaId"
                , "u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade"
                , "UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente"
                , "v.Placa as Placa"
                , "l.ValorContrato as [Valor Contrato]"
                , "cm.NumeroContrato as Contrato"
                , "cm.Valor as Boleto"
                , "cm.DataInsercao as [Data Emissão]"
                , "l.DataVencimento as [Data Vencto]"
                , "cm.DataInicio as [Data Inicio Vaga]"
                , "cm.NumeroVagas as [Qtde Vagas]"
                , "u.NumeroVaga as [Total Vagas Unidade]"
                , "pag.ValorPago as Pago"
                , "pag.ValorDesconto as Desconto"
                , "'true' as BltPago"
                , "dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal"
                , "l.TipoServico"
                , "pag.DataPagamento as DataPagamento"
                , "l.DataCompetencia as DataCompetencia"
            };

            sql.Append($"SELECT * FROM ( ");
            sql.Append($" SELECT {string.Join(", ", colunas.ToArray())} ");
            sql.Append("  FROM LancamentoCobranca l (NOLOCK)  ");
            sql.Append("  INNER JOIN LancamentoCobrancaContratoMensalista lc (NOLOCK) on lc.LancamentoCobranca = l.Id ");
            sql.Append("  INNER JOIN ContratoMensalista cm (NOLOCK) on cm.Id = lc.ContratoMensalista  ");
            sql.Append("  INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade  ");
            sql.Append("  INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente  ");
            sql.Append("  INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa  ");
            sql.Append("  OUTER APPLY (   ");
            sql.Append("      SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("      FROM PessoaDocumento pd (NOLOCK)   ");
            sql.Append("      INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id   ");
            sql.Append("      WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id  ");
            sql.Append("      GROUP BY d.Id, d.Numero  ");
            sql.Append("      ORDER BY d.Id desc   ");
            sql.Append("   ) docCPF   ");
            sql.Append("  OUTER APPLY (   ");
            sql.Append("      SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ   ");
            sql.Append("      FROM PessoaDocumento pd (NOLOCK)   ");
            sql.Append("      INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id   ");
            sql.Append("      WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id  ");
            sql.Append("      GROUP BY d.Id, d.Numero  ");
            sql.Append("      ORDER BY d.Id desc   ");
            sql.Append("   ) docCNPJ   ");
            sql.Append("  OUTER APPLY  ");
            sql.Append("  (  ");
            sql.Append("      SELECT top 1 v.Placa  ");
            sql.Append("      FROM ContratoMensalistaVeiculo cmv  ");
            sql.Append("      LEFT JOIN Veiculo v (NOLOCK) on v.Id = cmv.Veiculo_id   ");
            sql.Append("      WHERE cmv.ContratoMensalista_id = cm.Id and v.Placa is not null and v.Placa <> '' ");
            sql.Append("      ORDER BY v.Id desc  ");
            sql.Append("  ) v  ");
            sql.Append("  LEFT JOIN Endereco e (NOLOCK) on e.Id = u.Endereco  ");
            sql.Append("  LEFT JOIN Recebimento r (NOLOCK) on r.Id = l.Recebimento  ");
            sql.Append("  OUTER APPLY (  ");
            sql.Append("                  SELECT  ");
            sql.Append("                      SUM(cast(pg.ValorPago as decimal(18,2))) ValorPago  ");
            sql.Append("                      , SUM(cast(pg.ValorDesconto as decimal(18,2))) ValorDesconto  ");
            sql.Append("                      , MAX(pg.DataPagamento) DataPagamento  ");
            sql.Append("                  FROM Pagamento pg (NOLOCK)  ");
            sql.Append("                  WHERE pg.Recebimento = r.Id  ");
            sql.Append("              ) pag  ");
            sql.Append("  WHERE 1=1  ");
            sql.Append("      AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim)))  ");
            sql.Append("      AND l.StatusLancamentoCobranca = 8  ");
            sql.Append("      AND l.TipoServico = 1 ");
            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            sql.Append("  UNION ");
                          
            sql.Append("  SELECT ");
            sql.Append("        l.Id CobrancaId ");
            sql.Append("        , u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade ");
            sql.Append("        , UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente ");
            sql.Append("        , '' as Placa ");
            sql.Append("        , 0 as Contrato ");
            sql.Append("        , 0 as Boleto ");
            sql.Append("        , l.ValorContrato as [Valor Contrato] ");
            sql.Append("        , null as [Data Emissão] ");
            sql.Append("        , l.DataVencimento as [Data Vencto] ");
            sql.Append("        , null as [Data Inicio Vaga] ");
            sql.Append("        , 0 as [Qtde Vagas] ");
            sql.Append("        , u.NumeroVaga as [Total Vagas Unidade] ");
            sql.Append("        , pag.ValorPago as Pago ");
            sql.Append("        , pag.ValorDesconto as Desconto ");
            sql.Append("        , 'false' as BltPago ");
            sql.Append("        , dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal ");
            sql.Append("        , l.TipoServico ");
            sql.Append("        , NULL as DataPagamento ");
            sql.Append("        , l.DataCompetencia as DataCompetencia ");
            sql.Append("  FROM LancamentoCobranca l (NOLOCK)  ");
            sql.Append("  INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade  ");
            sql.Append("  INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente  ");
            sql.Append("  INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa  ");
            sql.Append("  OUTER APPLY (   ");
            sql.Append("      SELECT TOP 1 d.Id IdCPF, d.Numero CPF   ");
            sql.Append("      FROM PessoaDocumento pd (NOLOCK)   ");
            sql.Append("      INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id   ");
            sql.Append("      WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id  ");
            sql.Append("      GROUP BY d.Id, d.Numero  ");
            sql.Append("      ORDER BY d.Id desc   ");
            sql.Append("   ) docCPF   ");
            sql.Append("  OUTER APPLY (   ");
            sql.Append("      SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ   ");
            sql.Append("      FROM PessoaDocumento pd (NOLOCK)   ");
            sql.Append("      INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id   ");
            sql.Append("      WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id  ");
            sql.Append("      GROUP BY d.Id, d.Numero  ");
            sql.Append("      ORDER BY d.Id desc   ");
            sql.Append("   ) docCNPJ   ");
            sql.Append("  LEFT JOIN Endereco e (NOLOCK) on e.Id = u.Endereco  ");
            sql.Append("  LEFT JOIN Recebimento r (NOLOCK) on r.Id = l.Recebimento  ");
            sql.Append("  OUTER APPLY (  ");
            sql.Append("                  SELECT  ");
            sql.Append("                      SUM(cast(pg.ValorPago as decimal(18,2))) ValorPago  ");
            sql.Append("                      , SUM(cast(pg.ValorDesconto as decimal(18,2))) ValorDesconto  ");
            sql.Append("                      , MAX(pg.DataPagamento) DataPagamento  ");
            sql.Append("                  FROM Pagamento pg (NOLOCK)  ");
            sql.Append("                  WHERE pg.Recebimento = r.Id  ");
            sql.Append("              ) pag  ");
            sql.Append("  WHERE 1=1  ");
            sql.Append("      AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim)))  ");
            sql.Append("      AND l.StatusLancamentoCobranca = 8  ");
            sql.Append("      AND l.TipoServico = 9 ");

            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            sql.Append("  ) as la ");
            
            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoPagamentoContratoMensalistaRelatorio(query.List(), colunas, TipoServico.Mensalista)?.ToList() ?? new List<DadosPagamentoVO>();
        }
        public IList<DadosPagamentoVO> BuscarPagamentosContratoMensalistasEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade, bool acrescentarCancelados = false)
        {
            var sql = new StringBuilder();
            
            var colunas = new List<string>
            {
                "l.Id CobrancaId"
                , "u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade"
                , "UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente"
                , "v.Placa as Placa"
                , "cm.NumeroContrato as Contrato"
                , "cm.Valor as Boleto"
                , "l.ValorContrato as [Valor Contrato]"
                , "cm.DataInsercao as [Data Emissão]"
                , "l.DataVencimento as [Data Vencto]"
                , "cm.DataInicio as [Data Inicio Vaga]"
                , "cm.NumeroVagas as [Qtde Vagas]"
                , "u.NumeroVaga as [Total Vagas Unidade]"
                , "pag.ValorPago as Pago"
                , "pag.ValorDesconto as Desconto"
                , "'false' as BltPago"
                , "dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal"
                , "l.TipoServico"
                , "NULL as DataPagamento"
                , "l.DataCompetencia as DataCompetencia"
            };

            sql.Append("SELECT * FROM ( ");
            sql.Append($"   SELECT {string.Join(", ", colunas.ToArray())} ");
            sql.Append("    FROM LancamentoCobranca l (NOLOCK) ");
            sql.Append("    INNER JOIN LancamentoCobrancaContratoMensalista lc (NOLOCK) on lc.LancamentoCobranca = l.Id ");
            sql.Append("    INNER JOIN ContratoMensalista cm (NOLOCK) on cm.Id = lc.ContratoMensalista ");
            sql.Append("    OUTER APPLY ");
            sql.Append("    ( ");
            sql.Append("        SELECT top 1 v.Placa ");
            sql.Append("        FROM ContratoMensalistaVeiculo cmv ");
            sql.Append("        LEFT JOIN Veiculo v (NOLOCK) on v.Id = cmv.Veiculo_id  ");
            sql.Append("        WHERE cmv.ContratoMensalista_id = cm.Id and v.Placa is not null and v.Placa <> '' ");
            sql.Append("        ORDER BY v.Id desc ");
            sql.Append("    ) v ");
            sql.Append("    INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade ");
            sql.Append("    LEFT JOIN Endereco e (NOLOCK) on e.Id = u.Endereco ");
            sql.Append("    INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente ");
            sql.Append("    INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCPF  ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCNPJ  ");
            sql.Append("    LEFT JOIN Pagamento pag (NOLOCK) on pag.Recebimento = l.Recebimento ");
            sql.Append("    WHERE 1=1 ");
            sql.Append("        AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("        AND l.TipoServico = 1 ");

            //if (acrescentarCancelados)
            //    sql.Append("        AND l.StatusLancamentoCobranca in (0, 1, 5) ");
            //else
                sql.Append("        AND l.StatusLancamentoCobranca in (0, 1) ");

            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            sql.Append("  UNION ");

            sql.Append("    SELECT ");
            sql.Append("        l.Id CobrancaId ");
            sql.Append("        , u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade ");
            sql.Append("        , UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente ");
            sql.Append("        , '' as Placa ");
            sql.Append("        , 009 as Contrato ");
            sql.Append("        , 0 as Boleto ");
            sql.Append("        , l.ValorContrato as [Valor Contrato] ");
            sql.Append("        , null as [Data Emissão] ");
            sql.Append("        , l.DataVencimento as [Data Vencto] ");
            sql.Append("        , null as [Data Inicio Vaga] ");
            sql.Append("        , 0 as [Qtde Vagas] ");
            sql.Append("        , u.NumeroVaga as [Total Vagas Unidade] ");
            sql.Append("        , pag.ValorPago as Pago ");
            sql.Append("        , pag.ValorDesconto as Desconto ");
            sql.Append("        , 'false' as BltPago ");
            sql.Append("        , dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal ");
            sql.Append("        , l.TipoServico ");
            sql.Append("        , NULL as DataPagamento ");
            sql.Append("        , l.DataCompetencia as DataCompetencia ");
            sql.Append("    FROM LancamentoCobranca l (NOLOCK) ");
            sql.Append("    INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade ");
            sql.Append("    LEFT JOIN Endereco e (NOLOCK) on e.Id = u.Endereco ");
            sql.Append("    INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente ");
            sql.Append("    INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCPF  ");
            sql.Append("    OUTER APPLY (  ");
            sql.Append("        SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ  ");
            sql.Append("        FROM PessoaDocumento pd (NOLOCK)  ");
            sql.Append("        INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id  ");
            sql.Append("        WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("        GROUP BY d.Id, d.Numero  ");
            sql.Append("        ORDER BY d.Id desc  ");
            sql.Append("     ) docCNPJ  ");
            sql.Append("    LEFT JOIN Pagamento pag (NOLOCK) on pag.Recebimento = l.Recebimento ");
            sql.Append("    WHERE 1=1 ");
            sql.Append("        AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim))) ");
            sql.Append("        AND l.TipoServico = 9 ");

            //if (acrescentarCancelados)
            //    sql.Append("        AND l.StatusLancamentoCobranca in (0, 1, 5) ");
            //else
                sql.Append("        AND l.StatusLancamentoCobranca in (0, 1) ");

            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");

            sql.Append(" ) as la ");

            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoPagamentoContratoMensalistaRelatorio(query.List(), colunas, TipoServico.Mensalista)?.ToList() ?? new List<DadosPagamentoVO>();
        }

        public IList<DadosPagamentoVO> BuscarPagamentosEfetuadosConferenciaContratoMensalistasRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            var sql = new StringBuilder();

            var colunas = new List<string>
            {
                  "u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade"
                , "UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente"
                , "v.Placa as Placa"
                , "l.ValorContrato as [Valor Contrato]"
                , "cm.NumeroContrato as Contrato"
                , "cm.Valor as Boleto"
                , "cm.DataInsercao as [Data Emissão]"
                , "l.DataVencimento as [Data Vencto]"
                , "cm.DataInicio as [Data Inicio Vaga]"
                , "cm.NumeroVagas as [Qtde Vagas]"
                , "u.NumeroVaga as [Total Vagas Unidade]"
                , "pag.ValorPago as Pago"
                , "pag.ValorDesconto as Desconto"
                , "'true' as BltPago"
                , "dbo.formatarCNPJCPF(IIF(c.TipoPessoa = 1, docCPF.CPF, docCNPJ.CNPJ)) DocumentoPessoal"
                , "l.TipoServico"
                , "pag.DataPagamento as DataPagamento"
                , "l.DataCompetencia as DataCompetencia"
                , "l.Id CobrancaId"
            };
            
            sql.Append($" SELECT {string.Join(", ", colunas.ToArray())} ");
            sql.Append("  FROM LancamentoCobranca l (NOLOCK)  ");
            sql.Append("  INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade  ");
            sql.Append("  INNER JOIN Cliente c (NOLOCK) on c.Id = l.Cliente  ");
            sql.Append("  INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa  ");
            sql.Append("  OUTER APPLY (   ");
            sql.Append("      SELECT TOP 1 d.Id IdCPF, d.Numero CPF  ");
            sql.Append("      FROM PessoaDocumento pd (NOLOCK)   ");
            sql.Append("      INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id   ");
            sql.Append("      WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id  ");
            sql.Append("      GROUP BY d.Id, d.Numero  ");
            sql.Append("      ORDER BY d.Id desc   ");
            sql.Append("   ) docCPF   ");
            sql.Append("  OUTER APPLY (   ");
            sql.Append("      SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ   ");
            sql.Append("      FROM PessoaDocumento pd (NOLOCK)   ");
            sql.Append("      INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id   ");
            sql.Append("      WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id  ");
            sql.Append("      GROUP BY d.Id, d.Numero  ");
            sql.Append("      ORDER BY d.Id desc   ");
            sql.Append("   ) docCNPJ   ");
            sql.Append("  INNER JOIN LancamentoCobrancaContratoMensalista lc (NOLOCK) on lc.LancamentoCobranca = l.Id ");
            sql.Append("  INNER JOIN ContratoMensalista cm (NOLOCK) on cm.Id = lc.ContratoMensalista  ");
            sql.Append("  OUTER APPLY  ");
            sql.Append("  (  ");
            sql.Append("      SELECT top 1 v.Placa  ");
            sql.Append("      FROM ContratoMensalistaVeiculo cmv  ");
            sql.Append("      LEFT JOIN Veiculo v (NOLOCK) on v.Id = cmv.Veiculo_id   ");
            sql.Append("      WHERE cmv.ContratoMensalista_id = cm.Id and v.Placa is not null and v.Placa <> '' ");
            sql.Append("      ORDER BY v.Id desc  ");
            sql.Append("  ) v  ");
            sql.Append("  LEFT JOIN Endereco e (NOLOCK) on e.Id = u.Endereco  ");
            sql.Append("  LEFT JOIN Recebimento r (NOLOCK) on r.Id = l.Recebimento  ");
            sql.Append("  OUTER APPLY (  ");
            sql.Append("                  SELECT  ");
            sql.Append("                      SUM(cast(pg.ValorPago as decimal(18,2))) ValorPago  ");
            sql.Append("                      , SUM(cast(pg.ValorDesconto as decimal(18,2))) ValorDesconto  ");
            sql.Append("                      , MAX(pg.DataPagamento) DataPagamento  ");
            sql.Append("                  FROM Pagamento pg (NOLOCK)  ");
            sql.Append("                  WHERE pg.Recebimento = r.Id  ");
            sql.Append("              ) pag  ");
            sql.Append("  WHERE 1=1  ");
            sql.Append("      AND ((Month(l.DataCompetencia) = month(:dataInicio) AND YEAR(l.DataCompetencia) = year(:dataInicio)) OR (Month(l.DataCompetencia) = month(:dataFim) AND YEAR(l.DataCompetencia) = year(:dataFim)))  ");
            sql.Append("      AND l.StatusLancamentoCobranca = 8  ");
            sql.Append("      AND l.TipoServico = 1 ");
            
            if (unidade > 0)
                sql.Append($"        AND l.Unidade = {unidade} ");
            
            var query = Session.CreateSQLQuery(sql.ToString());

            query.SetParameter("dataInicio", dataInicio.Date);
            query.SetParameter("dataFim", dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return ConverterResultadoPagamentoContratoMensalistaRelatorio(query.List(), colunas, TipoServico.Mensalista)?.ToList() ?? new List<DadosPagamentoVO>();
        }

        public IList<DadosPagamentoVO> ConverterResultadoPagamentoRelatorio(IList results, TipoServico tipoServico)
        {
            var lista = new List<DadosPagamentoVO>();
            foreach (object[] p in results)
            {
                var item = new DadosPagamentoVO
                {
                    Unidade = p[0]?.ToString() ?? string.Empty,
                    Cliente = p[1]?.ToString() ?? string.Empty,
                    Placa = p[2]?.ToString() ?? string.Empty,
                    Contrato = p[3]?.ToString() ?? string.Empty,
                    VlrContrato = string.IsNullOrEmpty(p[4]?.ToString()) ? 0 : Convert.ToDecimal(p[4]?.ToString()),
                    VlrBoleto = string.IsNullOrEmpty(p[14]?.ToString()) ? 0 : Convert.ToDecimal(p[14]?.ToString()),
                    DataEmissao = string.IsNullOrEmpty(p[5]?.ToString()) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[5]?.ToString()),
                    DataVencimento = string.IsNullOrEmpty(p[6]?.ToString()) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[6]?.ToString()),
                    DataCompetencia = string.IsNullOrEmpty(p[17]?.ToString()) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[17]?.ToString()),
                    DataInicioVaga = string.IsNullOrEmpty(p[7]?.ToString()) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[7]?.ToString()),
                    Quantidade = string.IsNullOrEmpty(p[8]?.ToString()) ? 0 : Convert.ToInt32(p[8]?.ToString()),
                    TotalVagasUnidade = string.IsNullOrEmpty(p[9]?.ToString()) ? 0 : Convert.ToInt32(p[9]?.ToString()),
                    VlrPago = string.IsNullOrEmpty(p[10]?.ToString()) ? 0 : Convert.ToDecimal(p[10]?.ToString()),
                    VlrDesconto = string.IsNullOrEmpty(p[16]?.ToString()) ? 0 : Convert.ToDecimal(p[16]?.ToString()),
                    Pago = p[12]?.ToString() != null ? Convert.ToBoolean(p[12]?.ToString()) : false,
                    Documento = p[13]?.ToString() ?? string.Empty,
                    TipoServico = tipoServico,
                    DataPagamento = string.IsNullOrEmpty(p[15]?.ToString()) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[15]?.ToString()),
                    CobrancaId = p[18]?.ToString() == null ? 0 : Convert.ToInt32(p[18].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }
        public IList<DadosPagamentoVO> ConverterResultadoPagamentoContratoMensalistaRelatorio(IList results, List<string> colunas, TipoServico tipoServico)
        {
            var lista = new List<DadosPagamentoVO>();
            foreach (object[] p in results)
            {
                var item = new DadosPagamentoVO
                {
                    Unidade = p[colunas.IndexOf("u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade")]?.ToString() ?? string.Empty,
                    Cliente = p[colunas.IndexOf("UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente")]?.ToString() ?? string.Empty,
                    Placa = p[colunas.FindIndex(x => x.Contains("as Placa"))]?.ToString() ?? string.Empty,
                    Contrato = p[colunas.FindIndex(x => x.Contains("as Contrato"))]?.ToString() ?? string.Empty,
                    VlrContrato = p[colunas.IndexOf("l.ValorContrato as [Valor Contrato]")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("l.ValorContrato as [Valor Contrato]")]?.ToString()),
                    VlrBoleto = p[colunas.FindIndex(x => x.Contains("as Boleto"))]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.FindIndex(x => x.Contains("as Boleto"))]?.ToString()),
                    DataEmissao = p[colunas.FindIndex(x => x.Contains("as [Data Emissão]"))]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.FindIndex(x => x.Contains("as [Data Emissão]"))]?.ToString()),
                    DataVencimento = p[colunas.IndexOf("l.DataVencimento as [Data Vencto]")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("l.DataVencimento as [Data Vencto]")]?.ToString()),
                    DataCompetencia = p[colunas.IndexOf("l.DataCompetencia as DataCompetencia")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("l.DataCompetencia as DataCompetencia")]?.ToString()),
                    DataInicioVaga = p[colunas.FindIndex(x => x.Contains("as [Data Inicio Vaga]"))]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.FindIndex(x => x.Contains("as [Data Inicio Vaga]"))]?.ToString()),
                    Quantidade = p[colunas.FindIndex(x => x.Contains("as [Qtde Vagas]"))]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.FindIndex(x => x.Contains("as [Qtde Vagas]"))]?.ToString()),
                    TotalVagasUnidade = p[colunas.IndexOf("u.NumeroVaga as [Total Vagas Unidade]")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("u.NumeroVaga as [Total Vagas Unidade]")]?.ToString()),
                    VlrPago = colunas.IndexOf("pag.ValorPago as Pago") < 0 || p[colunas.IndexOf("pag.ValorPago as Pago")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pag.ValorPago as Pago")]?.ToString()),
                    Pago = p[colunas.FindIndex(x => x.Contains("as BltPago"))]?.ToString() != null ? Convert.ToBoolean(p[colunas.FindIndex(x => x.Contains("as BltPago"))]?.ToString()) : false,
                    Documento = p[colunas.FindIndex(x => x.Contains("DocumentoPessoal"))]?.ToString() ?? string.Empty,
                    TipoServico = (TipoServico)Convert.ToInt32(p[colunas.IndexOf("l.TipoServico")].ToString()),
                    DataPagamento = p[colunas.FindIndex(x => x.Contains("as DataPagamento"))]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.FindIndex(x => x.Contains("as DataPagamento"))]?.ToString()),
                    VlrDesconto = p[colunas.FindIndex(x => x.Contains("as Desconto"))]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.FindIndex(x => x.Contains("as Desconto"))]?.ToString()),

                    CobrancaId = p[colunas.IndexOf("l.Id CobrancaId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("l.Id CobrancaId")].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }
        public IList<DadosPagamentoVO> ConverterResultadoPagamentoPedidoLocacaoRelatorio(IList results, List<string> colunas, TipoServico tipoServico)
        {
            var lista = new List<DadosPagamentoVO>();
            foreach (object[] p in results)
            {
                var item = new DadosPagamentoVO
                {
                    Unidade = p[colunas.IndexOf("u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade")]?.ToString() ?? string.Empty,
                    Cliente = p[colunas.IndexOf("UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente")]?.ToString() ?? string.Empty,
                    Contrato = p[colunas.IndexOf("pl.Id as Contrato")]?.ToString() ?? string.Empty,
                    VlrContrato = p[colunas.IndexOf("l.ValorContrato as [Valor Contrato]")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("l.ValorContrato as [Valor Contrato]")]?.ToString()),
                    VlrPedido = p[colunas.IndexOf("pl.Valor as [Valor Pedido]")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pl.Valor as [Valor Pedido]")]?.ToString()),
                    VlrTotal = p[colunas.IndexOf("pl.ValorTotal as [Valor Total Pedido]")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pl.ValorTotal as [Valor Total Pedido]")]?.ToString()),

                    DataGeracao = p[colunas.IndexOf("l.DataGeracao as [Data Emissão]")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("l.DataGeracao as [Data Emissão]")]?.ToString()),

                    DataVencimento = p[colunas.IndexOf("l.DataVencimento as [Data Vencto]")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("l.DataVencimento as [Data Vencto]")]?.ToString()),
                    DataCompetencia = p[colunas.IndexOf("l.DataCompetencia as DataCompetencia")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("l.DataCompetencia as DataCompetencia")]?.ToString()),
                    DataInicioVaga = p[colunas.IndexOf("pl.DataVigenciaInicio as [Data Inicio]")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("pl.DataVigenciaInicio as [Data Inicio]")]?.ToString()),
                    DataFimVaga = p[colunas.IndexOf("pl.DataVigenciaFim as [Data Fim]")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("pl.DataVigenciaFim as [Data Fim]")]?.ToString()),

                    TipoReajuste = ((TipoReajuste)Convert.ToInt32(p[colunas.IndexOf("pl.TipoReajuste as [Tipo Reajuste]")].ToString())).ToDescription(),
                    VlrReajuste = p[colunas.IndexOf("pl.ValorReajuste as [Valor Reajuste]")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pl.ValorReajuste as [Valor Reajuste]")]?.ToString()),
                    DataReajuste = p[colunas.IndexOf("pl.DataReajuste as [Reajuste]")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("pl.DataReajuste as [Reajuste]")]?.ToString()),
                    VlrPago = colunas.IndexOf("pag.ValorPago as Pago") < 0 || p[colunas.IndexOf("pag.ValorPago as Pago")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pag.ValorPago as Pago")]?.ToString()),
                    Pago = p[colunas.FindIndex(x => x.Contains("as BltPago"))]?.ToString() != null ? Convert.ToBoolean(p[colunas.FindIndex(x => x.Contains("as BltPago"))]?.ToString()) : false,
                    Documento = p[colunas.FindIndex(x => x.Contains("DocumentoPessoal"))]?.ToString() ?? string.Empty,
                    TipoServico = tipoServico,
                    DataPagamento = p[colunas.FindIndex(x => x.Contains("as DataPagamento"))]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.FindIndex(x => x.Contains("as DataPagamento"))]?.ToString()),
                    VlrMulta = p[colunas.FindIndex(x => x.Contains("as VlrMulta"))]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.FindIndex(x => x.Contains("as VlrMulta"))]?.ToString()),
                    VlrDesconto = p[colunas.FindIndex(x => x.Contains("as Desconto"))]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.FindIndex(x => x.Contains("as Desconto"))]?.ToString()),

                    CobrancaId = p[colunas.IndexOf("l.Id CobrancaId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("l.Id CobrancaId")].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }
        public IList<DadosPagamentoVO> ConverterResultadoPagamentoPedidoSeloRelatorio(IList results, List<string> colunas, TipoServico tipoServico)
        {
            var lista = new List<DadosPagamentoVO>();
            foreach (object[] p in results)
            {
                var item = new DadosPagamentoVO
                {
                    Unidade = p[colunas.IndexOf("u.Nome + ' - ' + UPPER(e.Logradouro) as Unidade")]?.ToString() ?? string.Empty,
                    Cliente = p[colunas.IndexOf("UPPER(COALESCE(COALESCE(nullif(c.NomeFantasia, ''), nullif(c.RazaoSocial, '')), p.Nome)) as Cliente")]?.ToString() ?? string.Empty,
                    Contrato = p[colunas.IndexOf("ps.Id as Contrato")]?.ToString() ?? string.Empty,
                    VlrContrato = p[colunas.IndexOf("l.ValorContrato as Boleto")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("l.ValorContrato as Boleto")]?.ToString()),

                    DataGeracao = p[colunas.IndexOf("l.DataGeracao as [Data Emissão]")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("l.DataGeracao as [Data Emissão]")]?.ToString()),
                    DataVencimento = p[colunas.IndexOf("l.DataVencimento as [Data Vencto]")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("l.DataVencimento as [Data Vencto]")]?.ToString()),
                    DataCompetencia = p[colunas.IndexOf("l.DataCompetencia as DataCompetencia")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.IndexOf("l.DataCompetencia as DataCompetencia")]?.ToString()),

                    Quantidade = p[colunas.IndexOf("ps.Quantidade as [Quantidade]")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("ps.Quantidade as [Quantidade]")]?.ToString()),
                    ValidadePedido = p[colunas.IndexOf("ps.ValidadePedido as [Validade Pedido]")]?.ToString() == null ? new DateTime?() : DateTime.Parse(p[colunas.IndexOf("ps.ValidadePedido as [Validade Pedido]")]?.ToString()),
                    TipoSelo = p[colunas.IndexOf("ts.Nome as [Tipo Selo]")]?.ToString() ?? string.Empty,
                    TipoPedidoSelo = p[colunas.IndexOf("ps.TipoPedidoSelo as [Tipo Pedido Selo]")]?.ToString() == null ? TipoPedidoSelo.Emissao.ToDescription() : ((TipoPedidoSelo)Convert.ToInt32(p[colunas.IndexOf("ps.TipoPedidoSelo as [Tipo Pedido Selo]")].ToString())).ToDescription(),
                    VlrPago = colunas.IndexOf("pag.ValorPago as Pago") < 0 || p[colunas.IndexOf("pag.ValorPago as Pago")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pag.ValorPago as Pago")]?.ToString()),
                    Pago = p[colunas.FindIndex(x => x.Contains("as BltPago"))]?.ToString() != null ? Convert.ToBoolean(p[colunas.FindIndex(x => x.Contains("as BltPago"))]?.ToString()) : false,
                    Documento = p[colunas.FindIndex(x => x.Contains("DocumentoPessoal"))]?.ToString() ?? string.Empty,
                    TipoServico = tipoServico,
                    DataPagamento = p[colunas.FindIndex(x => x.Contains("as DataPagamento"))]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Parse(p[colunas.FindIndex(x => x.Contains("as DataPagamento"))]?.ToString()),
                    NomeConvenio = p[colunas.IndexOf("con.Descricao as NomeConvenio")]?.ToString() ?? string.Empty,
                    VlrDesconto = p[colunas.FindIndex(x => x.Contains("as Desconto"))]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.FindIndex(x => x.Contains("as Desconto"))]?.ToString()),

                    CobrancaId = p[colunas.IndexOf("l.Id CobrancaId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("l.Id CobrancaId")].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }

        public IList<DadosLancamentosVO> ConverterResultadoRelatorio(IList results)
        {
            var lista = new List<DadosLancamentosVO>();
            foreach (object[] p in results)
            {
                var item = new DadosLancamentosVO
                {
                    Unidade = p[0]?.ToString() ?? string.Empty,
                    TipoServico = p[1]?.ToString() ?? string.Empty,
                    QuantidadeTotal = p[2]?.ToString() == null ? 0 : Convert.ToInt32(p[2]?.ToString()),
                    ValorTotal = p[3]?.ToString() == null ? 0 : Convert.ToDecimal(p[3]?.ToString()),
                    Cliente = p[4]?.ToString() ?? string.Empty
                };

                lista.Add(item);
            }

            return lista;
        }
        public IList<LancamentoCobranca> ConverterResultadoPesquisaEmObjeto(IList results)
        {
            var lista = new List<LancamentoCobranca>();
            foreach (object[] p in results)
            {
                var item = new LancamentoCobranca
                {
                    Id = Convert.ToInt32(p[0].ToString()),
                    //CodContrato = p[1].ToString(),
                    //ValorContrato = Convert.ToDecimal(p[2]),
                    //DataVencimento = Convert.ToDateTime(p[3]),
                    //StatusContrato = (StatusContrato)Convert.ToInt32(p[4]),
                    //Devedor = new Devedor { Pessoa = new Pessoa { DocumentoCpf = p[5].ToString() } },
                    //TemPromessaValida = p[6] != null
                };

                lista.Add(item);
            }

            return lista;
        }
        public IList<LancamentoCobranca> ConverterResultadoPesquisaEmObjetoSimples(IList results, List<string> colunas)
        {
            var lista = new List<LancamentoCobranca>();
            foreach (object[] p in results)
            {
                if (lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("l.Id LancamentoId")].ToString())))
                    continue;

                var item = new LancamentoCobranca
                {
                    Id = Convert.ToInt32(p[colunas.IndexOf("l.Id LancamentoId")].ToString()),
                    DataInsercao = p[colunas.IndexOf("l.DataInsercao")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("l.DataInsercao")].ToString()),
                    DataGeracao = p[colunas.IndexOf("l.DataGeracao")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("l.DataGeracao")].ToString()),
                    DataBaixa = p[colunas.IndexOf("l.DataBaixa")]?.ToString() == null ? new DateTime?() : Convert.ToDateTime(p[colunas.IndexOf("l.DataBaixa")].ToString()),
                    DataCompetencia = p[colunas.IndexOf("l.DataCompetencia")]?.ToString() == null ? new DateTime?() : Convert.ToDateTime(p[colunas.IndexOf("l.DataCompetencia")].ToString()),
                    DataVencimento = p[colunas.IndexOf("l.DataVencimento")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("l.DataVencimento")].ToString()),
                    StatusLancamentoCobranca = (StatusLancamentoCobranca)Convert.ToInt32(p[colunas.IndexOf("l.StatusLancamentoCobranca")].ToString()),
                    PossueCnab = p[colunas.IndexOf("l.PossueCnab")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("l.PossueCnab")]?.ToString()) : false,
                    TipoServico = p[colunas.IndexOf("l.TipoServico")]?.ToString() == null ? TipoServico.Mensalista : (TipoServico)Convert.ToInt32(p[colunas.IndexOf("l.TipoServico")].ToString()),
                    ValorContrato = p[colunas.IndexOf("l.ValorContrato")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("l.ValorContrato")].ToString()),
                    TipoValorMulta = p[colunas.IndexOf("l.TipoValorMulta")]?.ToString() == null ? TipoValor.Monetario : (TipoValor)Convert.ToInt32(p[colunas.IndexOf("l.TipoValorMulta")].ToString()),
                    ValorMulta = p[colunas.IndexOf("l.ValorMulta")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("l.ValorMulta")].ToString()),
                    TipoValorJuros = p[colunas.IndexOf("l.TipoValorJuros")]?.ToString() == null ? TipoValor.Monetario : (TipoValor)Convert.ToInt32(p[colunas.IndexOf("l.TipoValorJuros")].ToString()),
                    ValorJuros = p[colunas.IndexOf("l.ValorJuros")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("l.ValorJuros")].ToString()),
                    
                    NumerosContratos = p[colunas.IndexOf("cm.Contratos")]?.ToString() ?? string.Empty,

                    ContaFinanceira = new ContaFinanceira
                    {
                        Id = p[colunas.IndexOf("l.ContaFinanceira ContaFinanceiraId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("l.ContaFinanceira ContaFinanceiraId")].ToString()),
                        Descricao = p[colunas.IndexOf("cf.Descricao DescricaoContaFinanceira")]?.ToString() ?? string.Empty
                    },
                    Unidade = new Unidade
                    {
                        Id = p[colunas.IndexOf("l.Unidade UnidadeId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("l.Unidade UnidadeId")].ToString()),
                        Codigo = p[colunas.IndexOf("u.Codigo CodigoUnidade")]?.ToString() ?? string.Empty,
                        Nome = p[colunas.IndexOf("u.Nome NomeUnidade")]?.ToString() ?? string.Empty,
                        Endereco = new Endereco
                        {
                            Id = p[colunas.IndexOf("ue.Id IdUnidadeEndereco")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("ue.Id IdUnidadeEndereco")].ToString()),
                            Cep = p[colunas.IndexOf("ue.Cep UnidadeCep")]?.ToString() ?? string.Empty,
                            Logradouro = p[colunas.IndexOf("ue.Logradouro UnidadeLogradouro")]?.ToString() ?? string.Empty,
                            Numero = p[colunas.IndexOf("ue.Numero UnidadeNumeroEndereco")]?.ToString() ?? string.Empty,
                            Complemento = p[colunas.IndexOf("ue.Complemento UnidadeComplemento")]?.ToString() ?? string.Empty,
                            Bairro = p[colunas.IndexOf("ue.Bairro UnidadeBairro")]?.ToString() ?? string.Empty
                        }
                    },
                    Cliente = new Cliente
                    {
                        Id = p[colunas.IndexOf("l.Cliente ClienteId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("l.Cliente ClienteId")].ToString()),
                        RazaoSocial = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() ?? string.Empty,
                        NomeFantasia = p[colunas.IndexOf("c.NomeFantasia")]?.ToString() ?? string.Empty,
                        TipoPessoa = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() != null || p[colunas.IndexOf("c.NomeFantasia")]?.ToString() != null ? TipoPessoa.Juridica : TipoPessoa.Fisica,
                        Pessoa = new Pessoa
                        {
                            Id = p[colunas.IndexOf("c.Pessoa PessoaId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("c.Pessoa PessoaId")].ToString()),
                            Nome = p[colunas.IndexOf("p.Nome NomePessoa")]?.ToString() ?? string.Empty,

                            Documentos = new List<PessoaDocumento>
                            {
                                new PessoaDocumento
                                {
                                    Tipo = TipoDocumento.Cpf,
                                    Documento = new Documento(TipoDocumento.Cpf,
                                                                p[colunas.IndexOf("docCPF.CPF")]?.ToString() ?? string.Empty,
                                                                DateTime.MinValue,
                                                                p[colunas.IndexOf("docCPF.IdCPF")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("docCPF.IdCPF")].ToString()),
                                                                null, null, null, false)
                                },
                                new PessoaDocumento
                                {
                                    Tipo = TipoDocumento.Cnpj,
                                    Documento = new Documento(TipoDocumento.Cnpj,
                                                                p[colunas.IndexOf("docCNPJ.CNPJ")]?.ToString() ?? string.Empty,
                                                                DateTime.MinValue,
                                                                p[colunas.IndexOf("docCNPJ.IdCNPJ")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("docCNPJ.IdCNPJ")].ToString()),
                                                                null, null, null, false)
                                }
                            },

                            Enderecos = new List<PessoaEndereco> {
                                new PessoaEndereco
                                {
                                    Endereco = new Endereco
                                    {
                                        Id = p[colunas.IndexOf("e.Id IdEndereco")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("e.Id IdEndereco")].ToString()),
                                        Cep = p[colunas.IndexOf("e.Cep")]?.ToString() ?? string.Empty,
                                        Logradouro = p[colunas.IndexOf("e.Logradouro")]?.ToString() ?? string.Empty,
                                        Numero = p[colunas.IndexOf("e.Numero NumeroEndereco")]?.ToString() ?? string.Empty,
                                        Complemento = p[colunas.IndexOf("e.Complemento")]?.ToString() ?? string.Empty,
                                        Bairro = p[colunas.IndexOf("e.Bairro")]?.ToString() ?? string.Empty,

                                        Cidade = new Cidade
                                        {
                                            Id = p[colunas.IndexOf("e.Cidade_id")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("e.Cidade_id")].ToString()),
                                            Descricao = p[colunas.IndexOf("ci.Descricao Cidade")]?.ToString() ?? string.Empty,
                                            Estado = new Estado
                                            {
                                                Id = p[colunas.IndexOf("es.Id IdEstado")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("es.Id IdEstado")].ToString()),
                                                Descricao = p[colunas.IndexOf("es.Descricao Estado")]?.ToString() ?? string.Empty,
                                                Sigla = p[colunas.IndexOf("es.Sigla EstadoSigla")]?.ToString() ?? string.Empty,
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Recebimento = new Recebimento
                    {
                        Id = p[colunas.IndexOf("l.Recebimento IdRecebimentoLancamento")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("l.Recebimento IdRecebimentoLancamento")].ToString()),
                        Pagamentos = new List<Pagamento>
                        {
                            new Pagamento
                            {
                                NumeroRecibo = p[colunas.IndexOf("pag.NumeroRecibo")]?.ToString() ?? string.Empty,
                                DataPagamento = p[colunas.IndexOf("pag.DataPagamento")]?.ToString() == null ? new DateTime() : Convert.ToDateTime(p[colunas.IndexOf("pag.DataPagamento")].ToString()),
                                ValorPago = p[colunas.IndexOf("pag.VALOR_PAGO ValorPago")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pag.VALOR_PAGO ValorPago")].ToString()),
                                ValorDivergente = p[colunas.IndexOf("pag.VALOR_DESCONTO ValorDesconto")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pag.VALOR_DESCONTO ValorDesconto")].ToString()),
                                TipoDescontoAcrescimo = p[colunas.IndexOf("pag.TipoDescontoAcrescimo")]?.ToString() == null ? new TipoDescontoAcrescimo?() : (TipoDescontoAcrescimo)Convert.ToInt32(p[colunas.IndexOf("pag.TipoDescontoAcrescimo")].ToString()),
                            }
                        }
                    }
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}