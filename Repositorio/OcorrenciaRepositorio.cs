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
    public class OcorrenciaRepositorio : NHibRepository<OcorrenciaCliente>, IOcorrenciaRepositorio
    {
        public OcorrenciaRepositorio(NHibContext context)
            : base(context)
        {

        }

        public IList<OcorrenciaCliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int quantidadeRegistros)
        {
            ICriteria criteria = NHibContext.InnerSession.CreateCriteria(typeof(OcorrenciaCliente))
                                    .CreateAlias("Pessoa", "Pessoa")
                                    .AddOrder(Order.Asc("NomeFantasia"))
                                    .AddOrder(Order.Asc("Pessoa.Nome"))
                                    .SetFirstResult(registroInicial)
                                    .SetMaxResults(quantidadeRegistros);

            IList<OcorrenciaCliente> result = criteria.List<OcorrenciaCliente>();

            return result ?? new List<OcorrenciaCliente>(0);
        }

        public IList<OcorrenciaCliente> BuscarDadosGrid(string protocolo, string nome, string status, out int quantidadeRegistros, int pagina = 1, int take = 50)
        {
            var sql = new StringBuilder();

            var colunas = new List<string>
            {
                  "c.Id IdCliente", "c.RazaoSocial", "c.NomeFantasia", "p.Id IdPessoa", "p.Nome NomePessoa"
                , "oc.NumeroProtocolo", "oc.StatusOcorrencia","oc.Id IdOcorrencia"
            };

            sql.Append($"SELECT DISTINCT {string.Join(", ", colunas.ToArray())} ");
            sql.Append(" FROM OcorrenciaCliente oc (NOLOCK) ");
            sql.Append(" LEFT JOIN Cliente c (NOLOCK) on oc.Cliente_Id = c.Id ");
            sql.Append(" INNER JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
           

            sql.Append(" WHERE 1=1 ");

            if (!string.IsNullOrEmpty(protocolo))
            {
                sql.Append($" AND oc.NumeroProtocolo like '{protocolo}%' ");
            }

            if (!string.IsNullOrEmpty(nome))
            {
                sql.Append($" AND (c.NomeFantasia like '{nome}%' OR c.RazaoSocial like '{nome}%' OR p.Nome like '{nome}%') ");
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql.Append($" AND oc.StatusOcorrencia = '{status}' ");
            }

            sql.Append(" ORDER BY p.Nome ");

            var query = Session.CreateSQLQuery(sql.ToString()).SetCacheable(true);

            quantidadeRegistros = query.Future<object[]>().Count();

            return ConverteResultadoDadosGrid(query.Future<object[]>().Skip(pagina <= 1 ? 0 : (pagina - 1) * take).Take(take).ToList(), colunas)?.ToList() ?? new List<OcorrenciaCliente>();
        }

        public IList<OcorrenciaCliente> ConverteResultadoDadosGrid(IList results, List<string> colunas)
        {
            var lista = new List<OcorrenciaCliente>();
            foreach (object[] p in results)
            {
                if (lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("oc.Id IdOcorrencia")].ToString())))
                    continue;

                Cliente cliente = new Cliente();
                cliente.RazaoSocial = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() ?? string.Empty;
                cliente.NomeFantasia = p[colunas.IndexOf("c.NomeFantasia")]?.ToString() ?? string.Empty;
                cliente.TipoPessoa = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() != null || p[colunas.IndexOf("c.NomeFantasia")]?.ToString() != null ? TipoPessoa.Juridica : TipoPessoa.Fisica;
                cliente.Pessoa = new Pessoa
                {
                    Id = p[colunas.IndexOf("p.Id IdPessoa")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("p.Id IdPessoa")].ToString()),
                    Nome = p[colunas.IndexOf("p.Nome NomePessoa")]?.ToString() ?? string.Empty
                };

                var item = new OcorrenciaCliente
                {
                    Id = p[colunas.IndexOf("oc.Id IdOcorrencia")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("oc.Id IdOcorrencia")].ToString()),
                    Cliente = cliente,
                    NumeroProtocolo = p[colunas.IndexOf("oc.NumeroProtocolo")]?.ToString() ?? string.Empty,
                    StatusOcorrencia = p[colunas.IndexOf("oc.StatusOcorrencia")]?.ToString() == null ? StatusOcorrencia.Novo : (StatusOcorrencia)p[colunas.IndexOf("oc.StatusOcorrencia")]
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}
