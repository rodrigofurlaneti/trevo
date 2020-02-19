using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface ISeloServico : IBaseServico<Selo>
    {
        IList<DadosSelosVO> BuscarSelosPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
        IList<DadosSelosVO> BuscarSelosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
    }

    public class SeloServico : BaseServico<Selo, ISeloRepositorio>, ISeloServico
    {
        private readonly ISeloRepositorio _seloRepositorio;

        public SeloServico(ISeloRepositorio seloRepositorio)
        {
            _seloRepositorio = seloRepositorio;
        }

        public IList<DadosSelosVO> BuscarSelosPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            return _seloRepositorio.BuscarSelosPagosRelatorio(dataInicio, dataFim, unidade);
        }
        public IList<DadosSelosVO> BuscarSelosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            return _seloRepositorio.BuscarSelosEmAbertoRelatorio(dataInicio, dataFim, unidade);
        }
    }
}