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

    public class ContaContabilRepositorio : NHibRepository<ContaContabil>, IContaContabilRepositorio
    {
        public ContaContabilRepositorio(NHibContext context)
            : base(context)
        {
        }

        public IList<ContaContabil> BuscarDadosSimples()
        {
            var sql = new StringBuilder();

            sql.Append("SELECT c.Id, c.Descricao FROM ContaContabil c ");

            var query = Session.CreateSQLQuery(sql.ToString());
            
            return ConverterResultado(query.List())?.ToList() ?? new List<ContaContabil>();
        }

        public IList<ContaContabil> ConverterResultado(IList results)
        {
            var lista = new List<ContaContabil>();
            foreach (object[] p in results)
            {
                var item = new ContaContabil
                {
                    Id = p[0]?.ToString() == null ? 0 : Convert.ToInt32(p[0]?.ToString()),
                    Descricao = p[1]?.ToString() ?? string.Empty
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}