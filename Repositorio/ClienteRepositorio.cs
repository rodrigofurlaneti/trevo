using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Extensions;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using NHibernate;
using NHibernate.Criterion;
using Repositorio.Base;

namespace Repositorio
{
    public class ClienteRepositorio : NHibRepository<Cliente>, IClienteRepositorio
    {
        public ClienteRepositorio(NHibContext context)
            : base(context)
        {

        }

        public IList<Cliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int quantidadeRegistros)
        {
            ICriteria criteria = NHibContext.InnerSession.CreateCriteria(typeof(Cliente))
                                    .CreateAlias("Pessoa", "Pessoa")
                                    .AddOrder(Order.Asc("NomeFantasia"))
                                    .AddOrder(Order.Asc("Pessoa.Nome"))
                                    .SetFirstResult(registroInicial)
                                    .SetMaxResults(quantidadeRegistros);

            IList<Cliente> result = criteria.List<Cliente>();

            return result ?? new List<Cliente>(0);
        }

        public IList<Cliente> BuscarDadosGrid(string documento, string nome, string contrato, out int quantidadeRegistros, int pagina = 1, int take = 50)
        {
            var sql = new StringBuilder();

            var colunas = new List<string>
            {
                  "c.Id IdCliente", "c.RazaoSocial", "c.NomeFantasia", "p.Id IdPessoa", "p.Nome NomePessoa"
                , "docCPF.IdCPF", "docCPF.CPF", "docCNPJ.IdCNPJ", "docCNPJ.CNPJ"
            };

            sql.Append($"SELECT DISTINCT {string.Join(", ", colunas.ToArray())} ");
            sql.Append(" FROM Cliente c (NOLOCK) ");
            sql.Append(" INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");

            sql.Append(" LEFT JOIN ContratoMensalista cm (NOLOCK) on cm.Cliente_id = c.Id ");

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

            sql.Append(" WHERE 1=1 ");

            if (!string.IsNullOrEmpty(documento))
            {
                //CPF
                if (documento.ExtractNumbers().Length == 11)
                {
                    sql.Append($" AND (replace(replace(replace(docCPF.CPF, '.', ''), '-', ''), '/', '') = '{documento.ExtractNumbers()}' OR docCPF.CPF = '{documento}') ");
                }
                //CNPJ
                if (documento.ExtractNumbers().Length == 14)
                {
                    sql.Append($" AND (replace(replace(replace(docCNPJ.CNPJ, '.', ''), '-', ''), '/', '') = '{documento.ExtractNumbers()}' OR docCNPJ.CNPJ = '{documento}') ");
                }
            }

            if (!string.IsNullOrEmpty(nome))
            {
                sql.Append($" AND (c.NomeFantasia like '{nome}%' OR c.RazaoSocial like '{nome}%' OR p.Nome like '{nome}%') ");
            }

            if (!string.IsNullOrEmpty(contrato))
            {
                sql.Append($" AND cm.NumeroContrato like '{contrato}%' ");
            }

            sql.Append(" ORDER BY p.Nome, c.NomeFantasia, c.RazaoSocial ");

            var query = Session.CreateSQLQuery(sql.ToString()).SetCacheable(true);

            quantidadeRegistros = query.Future<object[]>().Count();

            return ConverteResultadoDadosGrid(query.Future<object[]>().Skip(pagina <= 1 ? 0 : (pagina-1) * take).Take(take).ToList(), colunas)?.ToList() ?? new List<Cliente>();
        }

        public IList<Cliente> ConverteResultadoDadosGrid(IList results, List<string> colunas)
        {
            var lista = new List<Cliente>();
            foreach (object[] p in results)
            {
                if (lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("c.Id IdCliente")].ToString())))
                    continue;

                var item = new Cliente
                {
                    Id = p[colunas.IndexOf("c.Id IdCliente")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("c.Id IdCliente")].ToString()),
                    RazaoSocial = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() ?? string.Empty,
                    NomeFantasia = p[colunas.IndexOf("c.NomeFantasia")]?.ToString() ?? string.Empty,
                    TipoPessoa = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() != null || p[colunas.IndexOf("c.NomeFantasia")]?.ToString() != null ? TipoPessoa.Juridica : TipoPessoa.Fisica,
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
                            }
                    }
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}