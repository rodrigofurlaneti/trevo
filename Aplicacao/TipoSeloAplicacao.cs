using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface ITipoSeloAplicacao : IBaseAplicacao<TipoSelo>
    {
        IList<TipoSelo> BuscarPorConvenioUnidade(int idConvenio, int idUnidade);
        IList<TipoSelo> ListaTipoSelo(int? idConvenio = null, int? idUnidade = null);
    }

    public class TipoSeloAplicacao : BaseAplicacao<TipoSelo, ITipoSeloServico>, ITipoSeloAplicacao
    {
        private readonly ITipoSeloServico _tipoSeloServico;

        public TipoSeloAplicacao(ITipoSeloServico tipoSeloServico)
        {
            _tipoSeloServico = tipoSeloServico;
        }

        public IList<TipoSelo> ListaTipoSelo(int? idConvenio = null, int? idUnidade = null)
        {
            return _tipoSeloServico.ListaTipoSelo(idConvenio, idUnidade);
        }

        public IList<TipoSelo> BuscarPorConvenioUnidade(int idConvenio, int idUnidade)
        {
            return _tipoSeloServico.BuscarPorConvenioUnidade(idConvenio, idUnidade);
        }
    }
}