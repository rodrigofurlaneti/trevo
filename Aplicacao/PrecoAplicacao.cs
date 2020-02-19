using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;


namespace Aplicacao
{
    public interface IPrecoAplicacao : IBaseAplicacao<Preco>
    {
        void Salvar(Preco preco, int idUsuario);
    }
    public class PrecoAplicacao : BaseAplicacao<Preco,IPrecoServico>, IPrecoAplicacao
    {
        private readonly IPrecoServico _precoServico;

        public PrecoAplicacao(IPrecoServico precoServico)
        {
            _precoServico = precoServico;
        }

        public void Salvar(Preco preco, int idUsuario)
        {
            _precoServico.Salvar(preco, idUsuario);
        }
    }
}
