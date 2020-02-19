using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{
    public interface IConvenioServico : IBaseServico<Convenio>
    {
        IList<Convenio> BuscarPorAtivoComUnidade();
        IList<Convenio> BuscarAtivosPorUnidade(int idUnidade);
    }

    public class ConvenioServico : BaseServico<Convenio, IConvenioRepositorio>, IConvenioServico
    {
        private readonly IConvenioRepositorio _convenioRepositorio;

        public ConvenioServico(IConvenioRepositorio convenioRepositorio)
        {
            _convenioRepositorio = convenioRepositorio;
        }

        public IList<Convenio> BuscarPorAtivoComUnidade()
        {
            return _convenioRepositorio.BuscarPorAtivoComUnidade();
        }

        public IList<Convenio> BuscarAtivosPorUnidade(int idUnidade)
        {
            return _convenioRepositorio.BuscarAtivosPorUnidade(idUnidade);
        }
    }
}