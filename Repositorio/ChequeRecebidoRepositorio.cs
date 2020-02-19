using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;

namespace Repositorio
{
    public class ChequeRecebidoRepositorio : NHibRepository<ChequeRecebido>, IChequeRecebidoRepositorio
    {
        public ChequeRecebidoRepositorio(NHibContext context) : base(context)
        {

        }

        public IList<ChequeRecebido> BuscarDadosSimples()
        {
            var sql = new StringBuilder();

            var colunas = new List<string> {
                "cr.Id IdChequeRecebido", "cr.Numero NumeroChequeRecebido", "cr.Agencia AgenciaChequeRecebido", "cr.DigitoAgencia DigitoAgenciaChequeRecebido", "cr.Conta ContaChequeRecebido", "cr.DigitoConta DigitoContaChequeRecebido"
                , "b.Id IdBanco", "b.Descricao DescricaoBanco", "b.CodigoBanco CodigoBancoBanco"
                , "cr.CPFCNPJ CpfCnpjChequeRecebido", "cr.Descricao EmitenteChequeRecebido", "cr.Valor ValorChequeRecebido", "cr.DataDeposito DataDepositoChequeRecebido", "cr.DataProtesto DataProtestoChequeRecebido"
                , "cr.CartorioProtestado CartorioProtestadoChequeRecebido", "cr.StatusCheque StatusChequeChequeRecebido"
                , "c.Id IdCliente", "c.NomeFantasia NomeFantasiaCliente", "c.RazaoSocial RazaoSocialChequeRecebido"
                , "l.Id IdLancamento", "l.TipoServico TipoServicoLancamento", "l.ValorContrato ValorContratoLancamento"
                , "l.TipoValorMulta", "l.ValorMulta ValorMultaLancamento", "l.TipoValorJuros", "l.ValorJuros ValorJurosLancamento", "l.DataGeracao DataGeracaoLancamento", "l.DataVencimento DataVencimentoLancamento", "l.DataCompetencia DataCompetenciaLancamento"
                , "l.DataBaixa", "l.StatusLancamentoCobranca", "l.PossueCnab"
                , "cf.Id IdContaFinanceira", "cf.Conta ContaContaFinanceira", "cf.Descricao DescricaoContaFinanceira"
                , "u.Id IdUnidade", "u.Nome NomeUnidade", "u.Codigo CodigoUnidade"
                , "p.Id IdPessoa", "p.Nome NomePessoa", "d1.Id IdCpf", "d1.Numero CpfPessoa", "d2.Id IdCnpj", "d2.Numero CnpjPessoa"
            };

            sql.Append($"SELECT {string.Join(", ", colunas.ToArray())} ");
            sql.Append(" FROM ChequeRecebido cr (NOLOCK) ");
            sql.Append(" INNER JOIN Cliente c (nolock) on c.Id = cr.Cliente ");
            sql.Append(" INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append(" INNER JOIN Banco b (NOLOCK) on b.Id = cr.BancoCheque ");
            sql.Append(" INNER JOIN ChequeRecebidoLancamentoCobranca cl (NOLOCK) on cl.ChequeRecebido = cr.Id ");
            sql.Append(" INNER JOIN LancamentoCobranca l (NOLOCK) on l.Id = cl.LancamentoCobranca_id ");
            sql.Append(" INNER JOIN ContaFinanceira cf (NOLOCK) on cf.Id = l.ContaFinanceira ");
            sql.Append(" INNER JOIN Unidade u (NOLOCK) on u.Id = l.Unidade ");
            sql.Append(" LEFT JOIN PessoaDocumento pd1 (NOLOCK) on pd1.Pessoa = p.Id ");
            sql.Append(" LEFT JOIN Documento d1 (NOLOCK) on d1.Id = pd1.Documento_id and d1.Tipo = 2 ");
            sql.Append(" LEFT JOIN PessoaDocumento pd2 (NOLOCK) on pd2.Pessoa = p.Id ");
            sql.Append(" LEFT JOIN Documento d2 (NOLOCK) on d2.Id = pd2.Documento_id and d2.Tipo = 3 ");
            sql.Append("WHERE 1=1 ");

            sql.Append($"GROUP BY {string.Join(",", colunas.Select(x => (x.Contains(" ") ? x.Split(' ').First() : x)).ToList())} ");

            var query = Session.CreateSQLQuery(sql.ToString()).SetCacheable(true);

            return ConverterResultadoPesquisaEmObjetoSimples(query.List<object[]>(), colunas)?.ToList() ?? new List<ChequeRecebido>();
        }

        public IList<ChequeRecebido> ConverterResultadoPesquisaEmObjetoSimples(IList<object[]> results, List<string> colunas)
        {
            var lista = new List<ChequeRecebido>();
            Parallel.ForEach(results, (object[] p) =>
            {
                lock (lista)
                {
                    if (!lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("cr.Id IdChequeRecebido")].ToString())))
                    {
                        var item = new ChequeRecebido
                        {
                            Id = Convert.ToInt32(p[colunas.IndexOf("cr.Id IdChequeRecebido")].ToString()),
                            Numero = Convert.ToInt64(p[colunas.IndexOf("cr.Numero NumeroChequeRecebido")]?.ToString() ?? "0"),
                            Agencia = p[colunas.IndexOf("cr.Agencia AgenciaChequeRecebido")]?.ToString() ?? string.Empty,
                            DigitoAgencia = p[colunas.IndexOf("cr.DigitoAgencia DigitoAgenciaChequeRecebido")]?.ToString() ?? string.Empty,
                            Conta = p[colunas.IndexOf("cr.Conta ContaChequeRecebido")]?.ToString() ?? string.Empty,
                            DigitoConta = p[colunas.IndexOf("cr.DigitoConta DigitoContaChequeRecebido")]?.ToString() ?? string.Empty,
                            CartorioProtestado = p[colunas.IndexOf("cr.CartorioProtestado CartorioProtestadoChequeRecebido")]?.ToString() ?? string.Empty,
                            Cpf = p[colunas.IndexOf("cr.CPFCNPJ CpfCnpjChequeRecebido")]?.ToString() ?? string.Empty,
                            Emitente = p[colunas.IndexOf("cr.Descricao EmitenteChequeRecebido")]?.ToString() ?? string.Empty,
                            Valor = Convert.ToDecimal(p[colunas.IndexOf("cr.Valor ValorChequeRecebido")]?.ToString() ?? "0"),
                            DataDeposito = p[colunas.IndexOf("cr.DataDeposito DataDepositoChequeRecebido")]?.ToString() == null ? new DateTime?() : Convert.ToDateTime(p[colunas.IndexOf("cr.DataDeposito DataDepositoChequeRecebido")].ToString()),
                            DataProtesto = p[colunas.IndexOf("cr.DataProtesto DataProtestoChequeRecebido")]?.ToString() == null ? new DateTime?() : Convert.ToDateTime(p[colunas.IndexOf("cr.DataProtesto DataProtestoChequeRecebido")].ToString()),
                            StatusCheque = (StatusCheque)Convert.ToInt32(p[colunas.IndexOf("cr.StatusCheque StatusChequeChequeRecebido")].ToString()),
                            Banco = new Banco
                            {
                                Id = Convert.ToInt32(p[colunas.IndexOf("b.Id IdBanco")].ToString()),
                                Descricao = p[colunas.IndexOf("b.Descricao DescricaoBanco")]?.ToString() ?? string.Empty,
                                CodigoBanco = p[colunas.IndexOf("b.CodigoBanco CodigoBancoBanco")]?.ToString() ?? string.Empty
                            },
                            Cliente = new Cliente
                            {
                                Id = p[colunas.IndexOf("c.Id IdCliente")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("c.Id IdCliente")].ToString()),
                                RazaoSocial = p[colunas.IndexOf("c.RazaoSocial RazaoSocialChequeRecebido")]?.ToString() ?? string.Empty,
                                NomeFantasia = p[colunas.IndexOf("c.NomeFantasia NomeFantasiaCliente")]?.ToString() ?? string.Empty,
                                Pessoa = new Pessoa
                                {
                                    Id = p[colunas.IndexOf("p.Id IdPessoa")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("p.Id IdPessoa")].ToString()),
                                    Nome = p[colunas.IndexOf("p.Nome NomePessoa")]?.ToString() ?? string.Empty,

                                    Documentos = new List<PessoaDocumento>
                            {
                                new PessoaDocumento
                                {
                                    Tipo = TipoDocumento.Cpf,
                                    Documento = new Documento(TipoDocumento.Cpf,
                                                                p[colunas.IndexOf("d1.Numero CpfPessoa")]?.ToString() ?? string.Empty,
                                                                DateTime.MinValue,
                                                                p[colunas.IndexOf("d1.Id IdCpf")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("d1.Id IdCpf")].ToString()),
                                                                null, null, null, false)
                                },
                                new PessoaDocumento
                                {
                                    Tipo = TipoDocumento.Cnpj,
                                    Documento = new Documento(TipoDocumento.Cnpj,
                                                                p[colunas.IndexOf("d2.Numero CnpjPessoa")]?.ToString() ?? string.Empty,
                                                                DateTime.MinValue,
                                                                p[colunas.IndexOf("d2.Id IdCnpj")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("d2.Id IdCnpj")].ToString()),
                                                                null, null, null, false)
                                }
                            }
                                }
                            },
                            ListaLancamentoCobranca = new List<ChequeRecebidoLancamentoCobranca>()
                        };

                        lista.Add(item);
                    }
                }
            });

            Parallel.ForEach(results, (object[] p) =>
            {
                lock(lista)
                {
                    if (p[colunas.IndexOf("l.Id IdLancamento")] != null && lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("cr.Id IdChequeRecebido")].ToString())))
                    {
                        lista.FirstOrDefault(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("cr.Id IdChequeRecebido")].ToString()))
                        .ListaLancamentoCobranca.Add(new ChequeRecebidoLancamentoCobranca
                        {
                            LancamentoCobranca = new LancamentoCobranca
                            {
                                Id = Convert.ToInt32(p[colunas.IndexOf("l.Id IdLancamento")].ToString()),
                                DataGeracao = p[colunas.IndexOf("l.DataGeracao DataGeracaoLancamento")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("l.DataGeracao DataGeracaoLancamento")].ToString()),
                                DataBaixa = p[colunas.IndexOf("l.DataBaixa")]?.ToString() == null ? new DateTime?() : Convert.ToDateTime(p[colunas.IndexOf("l.DataBaixa")].ToString()),
                                DataVencimento = p[colunas.IndexOf("l.DataVencimento DataVencimentoLancamento")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("l.DataVencimento DataVencimentoLancamento")].ToString()),
                                DataCompetencia = p[colunas.IndexOf("l.DataCompetencia DataCompetenciaLancamento")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("l.DataCompetencia DataCompetenciaLancamento")].ToString()),
                                StatusLancamentoCobranca = (StatusLancamentoCobranca)Convert.ToInt32(p[colunas.IndexOf("l.StatusLancamentoCobranca")].ToString()),
                                PossueCnab = p[colunas.IndexOf("l.PossueCnab")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("l.PossueCnab")]?.ToString()) : false,
                                TipoServico = p[colunas.IndexOf("l.TipoServico TipoServicoLancamento")]?.ToString() == null ? TipoServico.Mensalista : (TipoServico)Convert.ToInt32(p[colunas.IndexOf("l.TipoServico TipoServicoLancamento")].ToString()),
                                ValorContrato = p[colunas.IndexOf("l.ValorContrato ValorContratoLancamento")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("l.ValorContrato ValorContratoLancamento")].ToString()),
                                TipoValorMulta = p[colunas.IndexOf("l.TipoValorMulta")]?.ToString() == null ? TipoValor.Monetario : (TipoValor)Convert.ToInt32(p[colunas.IndexOf("l.TipoValorMulta")].ToString()),
                                ValorMulta = p[colunas.IndexOf("l.ValorMulta ValorMultaLancamento")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("l.ValorMulta ValorMultaLancamento")].ToString()),
                                TipoValorJuros = p[colunas.IndexOf("l.TipoValorJuros")]?.ToString() == null ? TipoValor.Monetario : (TipoValor)Convert.ToInt32(p[colunas.IndexOf("l.TipoValorJuros")].ToString()),
                                ValorJuros = p[colunas.IndexOf("l.ValorJuros ValorJurosLancamento")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("l.ValorJuros ValorJurosLancamento")].ToString()),

                                ContaFinanceira = new ContaFinanceira
                                {
                                    Id = p[colunas.IndexOf("cf.Id IdContaFinanceira")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("cf.Id IdContaFinanceira")].ToString()),
                                    Descricao = p[colunas.IndexOf("cf.Descricao DescricaoContaFinanceira")]?.ToString() ?? string.Empty,
                                    Conta = p[colunas.IndexOf("cf.Conta ContaContaFinanceira")]?.ToString() ?? string.Empty,
                                },
                                Unidade = new Unidade
                                {
                                    Id = p[colunas.IndexOf("u.Id IdUnidade")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("u.Id IdUnidade")].ToString()),
                                    Codigo = p[colunas.IndexOf("u.Codigo CodigoUnidade")]?.ToString() ?? string.Empty,
                                    Nome = p[colunas.IndexOf("u.Nome NomeUnidade")]?.ToString() ?? string.Empty,
                                }
                            }
                        });
                    }
                }
            });

            return lista;
        }
    }
}
