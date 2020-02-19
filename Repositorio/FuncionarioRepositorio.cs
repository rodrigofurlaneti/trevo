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
    public class FuncionarioRepositorio : NHibRepository<Funcionario>, IFuncionarioRepositorio
    {
        public FuncionarioRepositorio(NHibContext context)
            : base(context)
        {

        }

        public List<Funcionario> BuscarComDadosSimples()
        {
            var sql = new StringBuilder();

            var colunas = new List<string> {
                "f.Id IdFuncionario", "f.DataInsercao DataInsercaoFuncionario", "f.Codigo CodigoFuncionario", "p.Id IdPessoa", "p.Nome NomePessoa"
            };

            sql.Append($"SELECT {string.Join(",", colunas)} ");
            sql.Append("FROM Funcionario f (NOLOCK) ");
            sql.Append("LEFT JOIN Pessoa p (NOLOCK) on p.Id = f.Pessoa ");
            sql.Append($"GROUP BY {string.Join(",", colunas.Select(x => (x.Contains(" ") ? x.Split(' ').First() : x)).ToList())} ");
            sql.Append(" ORDER BY p.Nome ");

            var query = Session.CreateSQLQuery(sql.ToString());

            return ConverterResultadoPesquisaEmObjetoSimples(query.List(), colunas)?.ToList() ?? new List<Funcionario>();
        }

        private IList<Funcionario> ConverterResultadoPesquisaEmObjetoSimples(IList results, List<string> colunas)
        {
            var lista = new List<Funcionario>();

            foreach (object[] p in results)
            {
                if (lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("f.Id IdFuncionario")].ToString())))
                    continue;

                var item = new Funcionario
                {
                    Id = p[colunas.IndexOf("f.Id IdFuncionario")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("f.Id IdFuncionario")].ToString()),
                    DataInsercao = p[colunas.IndexOf("f.DataInsercao DataInsercaoFuncionario")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("f.DataInsercao DataInsercaoFuncionario")].ToString()),
                    Codigo = p[colunas.IndexOf("f.Codigo CodigoFuncionario")]?.ToString() ?? string.Empty,
                    Pessoa = new Pessoa
                    {
                        Id = p[colunas.IndexOf("p.Id IdPessoa")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("p.Id IdPessoa")].ToString()),
                        Nome = p[colunas.IndexOf("p.Nome NomePessoa")]?.ToString() ?? string.Empty
                    }
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}