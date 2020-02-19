using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface IConvenioAplicacao : IBaseAplicacao<Convenio>
    {
        IList<Convenio> BuscarPorAtivoComUnidade();
        IList<Convenio> BuscarAtivosPorUnidade(int idUnidade);
    }

    public class ConvenioAplicacao : BaseAplicacao<Convenio, IConvenioServico>, IConvenioAplicacao
    {
        private readonly IConvenioServico _convenioServico;

        public ConvenioAplicacao(IConvenioServico convenioServico)
        {
            _convenioServico = convenioServico;
        }

        public IList<Convenio> BuscarPorAtivoComUnidade()
        {
            return _convenioServico.BuscarPorAtivoComUnidade();
        }

        public IList<Convenio> BuscarAtivosPorUnidade(int idUnidade)
        {
            return _convenioServico.BuscarAtivosPorUnidade(idUnidade);
        }
    }
}
