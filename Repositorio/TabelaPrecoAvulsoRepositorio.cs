using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio
{
    public class TabelaPrecoAvulsoRepositorio : NHibRepository<TabelaPrecoAvulso>, ITabelaPrecoAvulsoRepositorio
    {
        public TabelaPrecoAvulsoRepositorio(NHibContext context) : base(context)
        {

        }

        public List<TabelaPrecoAvulsoPeriodo> CarregarPeriodosDaTabela(int id)
        {
            var lista = Session.GetListBy<TabelaPrecoAvulsoPeriodo>(x => x.TabelaPrecoAvulso.Id == id)
                ?.OrderBy(x => (int)x.Periodo)
                ?.ToList();

            return lista ?? new List<TabelaPrecoAvulsoPeriodo>();
        }

        public List<TabelaPrecoAvulsoHoraValor> CarregarHoraValorDaTabela(int id)
        {
            var lista = Session.GetListBy<TabelaPrecoAvulsoHoraValor>(x => x.TabelaPrecoAvulso.Id == id)
                ?.OrderBy(x => x.Hora)
                ?.ToList();

            return lista ?? new List<TabelaPrecoAvulsoHoraValor>();
        }

        public List<TabelaPrecoAvulsoUnidade> CarregarUnidadesDaTabela(int id)
        {
            var lista = Session.GetListBy<TabelaPrecoAvulsoUnidade>(x => x.TabelaPrecoAvulso.Id == id)
                ?.OrderBy(x => x.Unidade.Nome)
                ?.ToList();

            return lista ?? new List<TabelaPrecoAvulsoUnidade>();
        }
    }
}