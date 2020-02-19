using Dominio.IRepositorio.Base;
using Entidade;
using System;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface ISeloRepositorio : IRepository<Selo>
    {
        IList<DadosSelosVO> BuscarSelosPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
        IList<DadosSelosVO> BuscarSelosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
    }
}