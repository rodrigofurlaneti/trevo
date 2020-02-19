using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using NHibernate;
using NHibernate.Criterion;
using Repositorio.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositorio
{
    public class ContratoMensalistaRepositorio : NHibRepository<ContratoMensalista>, IContratoMensalistaRepositorio
    {
        public ContratoMensalistaRepositorio(NHibContext context)
            : base(context)
        {

        }

        public void Salvar(ContratoMensalista contratoMensalista)
        {
            throw new System.NotImplementedException();
        }

        public IList<ContratoMensalista> BuscarPorIntervaloOrdenadoPeloNomeDoCliente(int registroInicial, int quantidadeRegistros)
        {
            ICriteria criteria = NHibContext.InnerSession.CreateCriteria(typeof(ContratoMensalista))
                                    .CreateAlias("Cliente", "Cliente")
                                    .CreateAlias("Cliente.Pessoa", "Pessoa")
                                    .AddOrder(Order.Asc("Cliente.NomeFantasia"))
                                    .AddOrder(Order.Asc("Pessoa.Nome"))
                                    .SetFirstResult(registroInicial)
                                    .SetMaxResults(quantidadeRegistros);

            var result = criteria.List<ContratoMensalista>();

            return result ?? new List<ContratoMensalista>(0);
        }

        public IList<ContratoMensalista> BuscarPorCliente(int idCliente)
        {
            var sql = new StringBuilder();

            var colunas = new List<string> {
                "cm.Id IdContratoMensalista", "cm.NumeroContrato", "cm.NumeroVagas", "cm.Valor", "cm.DataVencimento", "cm.Ativo", "cm.DataInicio", "cm.DataFim",
                "u.Id IdUnidade", "u.Codigo CodigoUnidade", "u.Nome NomeUnidade",
                "c.Id IdCliente", "c.NomeFantasia", "c.RazaoSocial", "p.Id IdPessoa", "p.Nome NomePessoa",
                "tp.Id IdTipoMensalista", "tp.Descricao DescricaoTipoMensalista"
            };

            sql.Append($"SELECT {string.Join(", ", colunas.ToArray())} ");
            sql.Append(" FROM ContratoMensalista cm (NOLOCK) ");
            sql.Append(" INNER JOIN Unidade u (NOLOCK) on u.Id = cm.Unidade_Id ");
            sql.Append(" INNER JOIN Cliente c (NOLOCK) on c.Id = cm.Cliente_Id ");
            sql.Append(" INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append(" INNER JOIN TipoMensalista tp (NOLOCK) on tp.Id = cm.TipoMensalista_Id ");
            sql.Append("WHERE 1=1 ");
            
            sql.Append($" AND c.Id = {idCliente} ");

            sql.Append($"GROUP BY {string.Join(",", colunas.Select(x => (x.Contains(" ") ? x.Split(' ').First() : x)).ToList())} ");

            var query = Session.CreateSQLQuery(sql.ToString());
            
            return ConverterResultadoPorCliente(query.List(), colunas)?.ToList() ?? new List<ContratoMensalista>();
        }
        
        private IList<ContratoMensalista> ConverterResultadoPorCliente(IList results, List<string> colunas)
        {
            var lista = new List<ContratoMensalista>();
            foreach (object[] p in results)
            {
                if (lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("cm.Id IdContratoMensalista")].ToString())))
                    continue;

                var item = new ContratoMensalista
                {
                    Id = Convert.ToInt32(p[colunas.IndexOf("cm.Id IdContratoMensalista")].ToString()),
                    NumeroContrato = p[colunas.IndexOf("cm.NumeroContrato")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("cm.NumeroContrato")].ToString()),
                    NumeroVagas = p[colunas.IndexOf("cm.NumeroVagas")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("cm.NumeroVagas")].ToString()),
                    Valor = p[colunas.IndexOf("cm.Valor")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("cm.Valor")].ToString()),
                    DataVencimento = p[colunas.IndexOf("cm.DataVencimento")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : Convert.ToDateTime(p[colunas.IndexOf("cm.DataVencimento")].ToString()),
                    DataInicio = p[colunas.IndexOf("cm.DataInicio")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : Convert.ToDateTime(p[colunas.IndexOf("cm.DataInicio")].ToString()),
                    DataFim = p[colunas.IndexOf("cm.DataFim")]?.ToString() == null ? new DateTime?() : Convert.ToDateTime(p[colunas.IndexOf("cm.DataFim")].ToString()),
                    Ativo = p[colunas.IndexOf("cm.Ativo")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("cm.Ativo")]?.ToString()) : false,
                    TipoMensalista = new TipoMensalista
                    {
                        Id = p[colunas.IndexOf("tp.Id IdTipoMensalista")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("tp.Id IdTipoMensalista")].ToString()),
                        Descricao = p[colunas.IndexOf("tp.Descricao DescricaoTipoMensalista")]?.ToString() ?? string.Empty,
                    },
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
                            Nome = p[colunas.IndexOf("p.Nome NomePessoa")]?.ToString() ?? string.Empty
                        }
                    }
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}