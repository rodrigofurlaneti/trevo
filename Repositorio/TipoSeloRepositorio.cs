using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio
{
    public class TipoSeloRepositorio : NHibRepository<TipoSelo>, ITipoSeloRepositorio
    {
        private readonly IConvenioRepositorio _convenioRepositorio;

        public TipoSeloRepositorio(NHibContext context,
            IConvenioRepositorio convenioRepositorio) 
            : base(context)
        {
            _convenioRepositorio = convenioRepositorio;
        }

        public IList<TipoSelo> BuscarPorConvenioUnidade(int idConvenio, int idUnidade)
        {
            var convenio = _convenioRepositorio.GetById(idConvenio);
            if (convenio == null || !convenio.ConvenioUnidades.Any())
                return new List<TipoSelo>();

            var listaIdTipoSelo = convenio.ConvenioUnidades
                .Where(x => x.ConvenioUnidade.Unidade.Id == idUnidade)
                .Select(x => x.ConvenioUnidade.TipoSelo.Id)
                .ToList();

            if (listaIdTipoSelo == null || !listaIdTipoSelo.Any())
                return new List<TipoSelo>();

            return Session.GetListBy<TipoSelo>(x => listaIdTipoSelo.Contains(x.Id)).ToList();
        }
    }
}
