using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{
    public interface ITipoSeloServico : IBaseServico<TipoSelo>
    {
        IList<TipoSelo> BuscarPorConvenioUnidade(int idConvenio, int idUnidade);
        IList<TipoSelo> ListaTipoSelo(int? idConvenio = null, int? idUnidade = null);
    }

    public class TipoSeloServico : BaseServico<TipoSelo, ITipoSeloRepositorio>, ITipoSeloServico
    {
        private readonly ITipoSeloRepositorio _tipoSeloRepositorio;

        public TipoSeloServico(ITipoSeloRepositorio tipoSeloRepositorio)
        {
            _tipoSeloRepositorio = tipoSeloRepositorio;
        }

        public IList<TipoSelo> ListaTipoSelo(int? idConvenio = null, int? idUnidade = null)
        {
            if (idConvenio.HasValue && idUnidade.HasValue)
                return BuscarPorConvenioUnidade(idConvenio.Value, idUnidade.Value);

            return _tipoSeloRepositorio.List();
        }

        public IList<TipoSelo> BuscarPorConvenioUnidade(int idConvenio, int idUnidade)
        {
            if (idConvenio == 0 || idUnidade == 0)
                return new List<TipoSelo>();

            return _tipoSeloRepositorio.BuscarPorConvenioUnidade(idConvenio, idUnidade);
        }
    }
}