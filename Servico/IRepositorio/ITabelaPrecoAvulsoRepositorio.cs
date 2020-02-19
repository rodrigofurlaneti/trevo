using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface ITabelaPrecoAvulsoRepositorio : IRepository<TabelaPrecoAvulso>
    {
        List<TabelaPrecoAvulsoPeriodo> CarregarPeriodosDaTabela(int id);
        List<TabelaPrecoAvulsoHoraValor> CarregarHoraValorDaTabela(int id);
        List<TabelaPrecoAvulsoUnidade> CarregarUnidadesDaTabela(int id);
    }
}