using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IOISAplicacao : IBaseAplicacao<OIS>
    {
        void Salvar(OIS ois, int usuarioId);
    }

    public class OISAplicacao : BaseAplicacao<OIS, IOISServico>, IOISAplicacao
    {
        private readonly IOISServico _oisServico;
        private readonly IUsuarioServico _usuarioServico;

        public OISAplicacao(IOISServico oisServico, IUsuarioServico usuarioServico)
        {
            _oisServico = oisServico;
            _usuarioServico = usuarioServico;
        }

        public void Salvar(OIS ois, int usuarioId)
        {
            ois.Usuario = _usuarioServico.BuscarPorId(usuarioId);
            _oisServico.Salvar(ois);
        }
    }
}