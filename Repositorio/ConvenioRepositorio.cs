using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio
{
    public class ConvenioRepositorio : NHibRepository<Convenio>, IConvenioRepositorio
    {
        public ConvenioRepositorio(NHibContext context)
           : base(context)
        {
        }

        public IList<Convenio> BuscarPorAtivoComUnidade()
        {
            return Session.GetListBy<Convenio>(x => x.Status == true && x.ConvenioUnidades.Any()).ToList();
        }

        public IList<Convenio> BuscarAtivosPorUnidade(int idUnidade)
        {
            return Session.GetListBy<Convenio>(x => x.Status == true && x.ConvenioUnidades.Any() && x.ConvenioUnidades.Any(y => y.ConvenioUnidade.Unidade.Id == idUnidade)).ToList();
        }
    }
}