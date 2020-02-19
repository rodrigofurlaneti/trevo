using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System.Linq;

namespace Aplicacao
{
    public interface ITabelaPrecoMensalistaAplicacao : IBaseAplicacao<TabelaPrecoMensalista>
    {
        void Salvar(TabelaPrecoMensalista TabelaPrecoMensalista, int idUsuario);
    }
    public class TabelaPrecoMensalistaAplicacao : BaseAplicacao<TabelaPrecoMensalista, ITabelaPrecoMensalistaServico>, ITabelaPrecoMensalistaAplicacao
    {
        private readonly ITabelaPrecoMensalistaServico _tabelaPrecoMensalistaServico;
        private readonly ITabelaPrecoMensalSoftparkAplicacao _tabelaPrecoMensalistaSoftparkAplicacao;
        private readonly IUnidadeServico _unidadeServico;

        public TabelaPrecoMensalistaAplicacao(
            ITabelaPrecoMensalistaServico tabelaPrecoMensalistaServico,
            ITabelaPrecoMensalSoftparkAplicacao tabelaPrecoMensalistaSoftparkAplicacao,
            IUnidadeServico unidadeServico
            )
        {
            _tabelaPrecoMensalistaServico = tabelaPrecoMensalistaServico;
            _tabelaPrecoMensalistaSoftparkAplicacao = tabelaPrecoMensalistaSoftparkAplicacao;
            _unidadeServico = unidadeServico;
        }

        public void Salvar(TabelaPrecoMensalista tabelaPrecoMensalista, int idUsuario)
        {
            _tabelaPrecoMensalistaServico.Salvar(tabelaPrecoMensalista, idUsuario);

            tabelaPrecoMensalista = _tabelaPrecoMensalistaServico.BuscarPorId(tabelaPrecoMensalista.Id);

            foreach(var tabelaUnidade in tabelaPrecoMensalista.TabelaPrecoUnidade)
            {
                tabelaUnidade.Unidade = _unidadeServico.BuscarPorId(tabelaUnidade.Unidade.Id);
            }

            var tabelaPrecoMensal = new TabelaPrecoMensalSoftparkViewModel(tabelaPrecoMensalista);
            _tabelaPrecoMensalistaSoftparkAplicacao.Salvar(tabelaPrecoMensal);
        }

        public override void ExcluirPorId(int id)
        {
            base.ExcluirPorId(id);
            _tabelaPrecoMensalistaSoftparkAplicacao.ExcluirPorId(id);
        }
    }
}
