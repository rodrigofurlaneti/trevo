using Aplicacao.Base;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface ISeloAplicacao : IBaseAplicacao<Selo>
    {
        IList<DadosSelosVO> BuscarSelosPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
        IList<DadosSelosVO> BuscarSelosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
    }
    public class SeloAplicacao : BaseAplicacao<Selo, ISeloServico>, ISeloAplicacao
    {
        private readonly ISeloServico _seloServico;

        public SeloAplicacao(ISeloServico seloServico)
        {
            _seloServico = seloServico;
        }

        public IList<DadosSelosVO> BuscarSelosPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            return _seloServico.BuscarSelosPagosRelatorio(dataInicio, dataFim, unidade);
        }
        public IList<DadosSelosVO> BuscarSelosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade)
        {
            return _seloServico.BuscarSelosEmAbertoRelatorio(dataInicio, dataFim, unidade);
        }
    }
}
