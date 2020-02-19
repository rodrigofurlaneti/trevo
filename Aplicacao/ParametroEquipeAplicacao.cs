using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IParametroEquipeAplicacao : IBaseAplicacao<ParametroEquipe>
    {
        void Salvar(ParametroEquipe ParametroEquipe, int idUsuario);
    }
    public class ParametroEquipeAplicacao : BaseAplicacao<ParametroEquipe, IParametroEquipeServico>, IParametroEquipeAplicacao
    {
        private readonly IParametroEquipeServico _ParametroEquipeServico;

        public ParametroEquipeAplicacao(IParametroEquipeServico ParametroEquipeServico)
        {
            _ParametroEquipeServico = ParametroEquipeServico;
        }

        public void Salvar(ParametroEquipe ParametroEquipe, int idUsuario)
        {
            _ParametroEquipeServico.Salvar(ParametroEquipe, idUsuario);
        }
    }
}
