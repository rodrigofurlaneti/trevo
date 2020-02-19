using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IOcorrenciaAplicacao : IBaseAplicacao<OcorrenciaCliente>
    {
        List<OcorrenciaCliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int registrosPorPagina);
        IList<OcorrenciaCliente> BuscarDadosGrid(string protocolo, string nome, string status, out int quantidadeRegistros, int pagina = 1, int take = 50);
        void SalvarDadosOcorrenciaComNotificacao(OcorrenciaCliente ocorrenciaCliente, int usuarioId);
    }

    public class OcorrenciaAplicacao : BaseAplicacao<OcorrenciaCliente, IOcorrenciaServico>, IOcorrenciaAplicacao
    {
        private readonly IOcorrenciaServico _ocorrenciaServico;
        public OcorrenciaAplicacao(IOcorrenciaServico ocorrenciaServico)
        {
            _ocorrenciaServico = ocorrenciaServico;
        }

        public List<OcorrenciaCliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int registrosPorPagina)
        {
            return _ocorrenciaServico.BuscarPorIntervaloOrdenadoPorNome(registroInicial, registrosPorPagina).ToList();
        }

        public IList<OcorrenciaCliente> BuscarDadosGrid(string protocolo, string nome, string status, out int quantidadeRegistros, int pagina = 1, int take = 50)
        {
            return _ocorrenciaServico.BuscarDadosGrid(protocolo, nome, status, out quantidadeRegistros, pagina, take);
        }

        public void SalvarDadosOcorrenciaComNotificacao(OcorrenciaCliente ocorrenciaCliente, int usuarioId)
        {
            Salvar(ocorrenciaCliente);
            _ocorrenciaServico.Notificar(ocorrenciaCliente, usuarioId);            
        }
    }
}
