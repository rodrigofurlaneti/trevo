using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositorio
{
    public class UnidadeRepositorio : NHibRepository<Unidade>, IUnidadeRepositorio
    {
        private readonly IConvenioRepositorio _convenioRepositorio;

        public UnidadeRepositorio(NHibContext context,
            IConvenioRepositorio convenioRepositorio) 
            : base(context)
        {
            _convenioRepositorio = convenioRepositorio;
        }

        public IList<Unidade> BuscarPorConvenio(int idConvenio)
        {
            var convenio = _convenioRepositorio.GetById(idConvenio);
            if (convenio == null || !convenio.ConvenioUnidades.Any())
                return new List<Unidade>();

            var listaIdUnidade = convenio.ConvenioUnidades
                .Where(x => x?.ConvenioUnidade?.Unidade != null)
                .Select(x => x.ConvenioUnidade.Unidade.Id)
                .Distinct()
                .ToList();

            return Session
                .GetListBy<Unidade>(x => listaIdUnidade.Contains(x.Id))
                .ToList();
        }

        public List<Unidade> ListarOrdenadoSimplificado()
        {
            var sql = new StringBuilder();

            var colunas = new List<string> {
                "u.Id IdUnidade", "u.DataInsercao DataInsercaoUnidade", "u.Codigo CodigoUnidade", "u.Nome NomeUnidade", "u.NumeroVaga NumeroVagaUnidade"
            };

            sql.Append($"SELECT {string.Join(",", colunas)} ");
            sql.Append("FROM Unidade u (NOLOCK) ");
            sql.Append($"GROUP BY {string.Join(",", colunas.Select(x => (x.Contains(" ") ? x.Split(' ').First() : x)).ToList())} ");

            var query = Session.CreateSQLQuery(sql.ToString());

            return ConverterResultadoPesquisaEmObjetoSimples(query.List(), colunas)?.ToList() ?? new List<Unidade>();
        }

        private IList<Unidade> ConverterResultadoPesquisaEmObjetoSimples(IList results, List<string> colunas)
        {
            var lista = new List<Unidade>();

            foreach (object[] p in results)
            {
                if (lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("u.Id IdUnidade")].ToString())))
                    continue;

                var item = new Unidade
                {
                    Id = p[colunas.IndexOf("u.Id IdUnidade")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("u.Id IdUnidade")].ToString()),
                    DataInsercao = p[colunas.IndexOf("u.DataInsercao DataInsercaoUnidade")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("u.DataInsercao DataInsercaoUnidade")].ToString()),
                    Codigo = p[colunas.IndexOf("u.Codigo CodigoUnidade")]?.ToString() ?? string.Empty,
                    Nome = p[colunas.IndexOf("u.Nome NomeUnidade")]?.ToString() ?? string.Empty,
                    NumeroVaga = p[colunas.IndexOf("u.NumeroVaga NumeroVagaUnidade")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("u.NumeroVaga NumeroVagaUnidade")].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}