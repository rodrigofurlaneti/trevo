using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoLocacaoRepositorio : NHibRepository<PedidoLocacao>, IPedidoLocacaoRepositorio
    {
        public PedidoLocacaoRepositorio(NHibContext context) : base(context)
        {   
        }

        public IList<PedidoLocacao> ListarPedidoLocacaoFiltro(int? idUnidade, int? idTipoLocacao)
        {
            var sql = new StringBuilder();

            var colunas = new List<string> {
                "pl.Id IdPedidoLocacao", "pl.DataInsercao DataInsercaoPedidoLocacao", "pl.TipoLocacao IdTipoLocacao", "tl.Descricao DescricaoTipoLocacao"
                , "pl.Valor ValorPedidoLocacao", "pl.ValorTotal ValorTotalPedidoLocacao", "pl.TipoReajuste"
                , "pl.Desconto IdDesconto", "d.Descricao DescricaoDesconto", "d.Valor ValorDesconto", "pl.PossuiFiador", "pl.PossuiCicloMensal", "pl.NomeFiador", "pl.FormaGarantia FormaGarantiaPedidoLocacao"
                , "pl.FormaPagamento FormaPagamentoPedidoLocacao", "pl.Status StatusPedidoLocacao", "pl.Ativo AtivoPedidoLocacao"
                , "pl.DataPrimeiroPagamento", "pl.DataDemaisPagamentos", "pl.DataReajuste", "pl.ValorPrimeiroPagamento", "pl.ValorReajuste"
                , "u.Id IdUnidade", "u.Nome NomeUnidade", "u.Codigo CodigoUnidade"
                , "c.Id IdCliente", "c.NomeFantasia", "c.RazaoSocial", "p.Id IdPessoa", "p.Nome NomePessoa"
            };

            sql.Append($"SELECT {string.Join(", ", colunas.ToArray())} ");
            sql.Append(" FROM PedidoLocacao pl (NOLOCK) ");
            sql.Append(" INNER JOIN Cliente c (nolock) on c.Id = pl.Cliente ");
            sql.Append(" INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append(" INNER JOIN Unidade u (NOLOCK) on u.Id = pl.Unidade ");
            sql.Append(" INNER JOIN TipoLocacao tl (NOLOCK) on tl.Id = pl.TipoLocacao ");
            sql.Append(" LEFT JOIN Desconto d (NOLOCK) on d.Id = pl.Desconto ");
            sql.Append("WHERE 1=1 ");

            if (idUnidade.HasValue && idUnidade.Value > 0)
                sql.Append($" AND u.Id = {idUnidade.Value} ");
            if (idTipoLocacao.HasValue && idTipoLocacao.Value > 0)
                sql.Append($" AND tl.Id = {idTipoLocacao.Value} ");

            sql.Append($"GROUP BY {string.Join(",", colunas.Select(x => (x.Contains(" ") ? x.Split(' ').First() : x)).ToList())} ");

            var query = Session.CreateSQLQuery(sql.ToString()).SetCacheable(true);

            return ConverterResultadoPesquisaEmObjetoSimples(query.List<object[]>(), colunas)?.ToList() ?? new List<PedidoLocacao>();
        }

        public IList<PedidoLocacao> ConverterResultadoPesquisaEmObjetoSimples(IList<object[]> results, List<string> colunas)
        {
            var lista = new List<PedidoLocacao>();
            foreach (object[] p in results)
            {
                if (lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("pl.Id IdPedidoLocacao")].ToString())))
                    continue;

                var item = new PedidoLocacao
                {
                    Id = Convert.ToInt32(p[colunas.IndexOf("pl.Id IdPedidoLocacao")].ToString()),
                    DataInsercao = p[colunas.IndexOf("pl.DataInsercao DataInsercaoPedidoLocacao")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("pl.DataInsercao DataInsercaoPedidoLocacao")].ToString()),
                    TipoLocacao = new TipoLocacao
                    {
                        Id = Convert.ToInt32(p[colunas.IndexOf("pl.TipoLocacao IdTipoLocacao")]?.ToString() ?? "0"),
                        Descricao = p[colunas.IndexOf("tl.Descricao DescricaoTipoLocacao")]?.ToString() ?? string.Empty
                    },
                    Valor = p[colunas.IndexOf("pl.Valor ValorPedidoLocacao")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pl.Valor ValorPedidoLocacao")].ToString()),
                    ValorTotal = p[colunas.IndexOf("pl.ValorTotal ValorTotalPedidoLocacao")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pl.ValorTotal ValorTotalPedidoLocacao")].ToString()),
                    ValorReajuste = p[colunas.IndexOf("pl.ValorReajuste")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pl.ValorReajuste")].ToString()),
                    ValorPrimeiroPagamento = p[colunas.IndexOf("pl.ValorPrimeiroPagamento")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("pl.ValorPrimeiroPagamento")].ToString()),
                    TipoReajuste = p[colunas.IndexOf("pl.TipoReajuste")]?.ToString() == null ? TipoReajuste.Monetario : (TipoReajuste)Convert.ToInt32(p[colunas.IndexOf("pl.TipoReajuste")].ToString()),
                    Desconto = new Desconto
                    {
                        Id = Convert.ToInt32(p[colunas.IndexOf("pl.Desconto IdDesconto")]?.ToString() ?? "0"),
                        Descricao = p[colunas.IndexOf("d.Descricao DescricaoDesconto")]?.ToString() ?? string.Empty,
                        Valor = p[colunas.IndexOf("d.Valor ValorDesconto")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("d.Valor ValorDesconto")].ToString()),
                    },
                    PossuiFiador = p[colunas.IndexOf("pl.PossuiFiador")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("pl.PossuiFiador")]?.ToString()) : false,
                    NomeFiador = p[colunas.IndexOf("pl.NomeFiador")]?.ToString() ?? string.Empty,
                    FormaGarantia = p[colunas.IndexOf("pl.FormaGarantia FormaGarantiaPedidoLocacao")]?.ToString() ?? string.Empty,
                    FormaPagamento = p[colunas.IndexOf("pl.FormaPagamento FormaPagamentoPedidoLocacao")]?.ToString() == null ? FormaPagamento.Cheque : (FormaPagamento)Convert.ToInt32(p[colunas.IndexOf("pl.FormaPagamento FormaPagamentoPedidoLocacao")].ToString()),
                    PossuiCicloMensal = p[colunas.IndexOf("pl.PossuiCicloMensal")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("pl.PossuiCicloMensal")]?.ToString()) : false,
                    Status = p[colunas.IndexOf("pl.Status StatusPedidoLocacao")]?.ToString() == null ? StatusSolicitacao.Aguardando : (StatusSolicitacao)Convert.ToInt32(p[colunas.IndexOf("pl.Status StatusPedidoLocacao")].ToString()),
                    Ativo = p[colunas.IndexOf("pl.Ativo AtivoPedidoLocacao")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("pl.Ativo AtivoPedidoLocacao")]?.ToString()) : false,
                    DataReajuste = p[colunas.IndexOf("pl.DataReajuste")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("pl.DataReajuste")].ToString()),
                    DataDemaisPagamentos = p[colunas.IndexOf("pl.DataDemaisPagamentos")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("pl.DataDemaisPagamentos")].ToString()),
                    DataPrimeiroPagamento = p[colunas.IndexOf("pl.DataPrimeiroPagamento")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("pl.DataPrimeiroPagamento")].ToString()),
                    
                    Unidade = new Unidade
                    {
                        Id = p[colunas.IndexOf("u.Id IdUnidade")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("u.Id IdUnidade")].ToString()),
                        Codigo = p[colunas.IndexOf("u.Codigo CodigoUnidade")]?.ToString() ?? string.Empty,
                        Nome = p[colunas.IndexOf("u.Nome NomeUnidade")]?.ToString() ?? string.Empty,
                    },
                    Cliente = new Cliente
                    {
                        Id = p[colunas.IndexOf("c.Id IdCliente")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("c.Id IdCliente")].ToString()),
                        RazaoSocial = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() ?? string.Empty,
                        NomeFantasia = p[colunas.IndexOf("c.NomeFantasia")]?.ToString() ?? string.Empty,
                        TipoPessoa = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() != null || p[colunas.IndexOf("c.NomeFantasia")]?.ToString() != null ? TipoPessoa.Juridica : TipoPessoa.Fisica,
                        Pessoa = new Pessoa
                        {
                            Id = p[colunas.IndexOf("p.Id IdPessoa")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("p.Id IdPessoa")].ToString()),
                            Nome = p[colunas.IndexOf("p.Nome NomePessoa")]?.ToString() ?? string.Empty,
                        }
                    }
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}