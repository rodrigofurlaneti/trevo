using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IHorarioUnidadeAplicacao : IBaseAplicacao<HorarioUnidade>
    {
        void Salvar(HorarioUnidade HorarioUnidade, int idUsuario);
    }
    public class HorarioUnidadeAplicacao : BaseAplicacao<HorarioUnidade, IHorarioUnidadeServico>, IHorarioUnidadeAplicacao
    {
        private readonly IHorarioUnidadeServico _HorarioUnidadeServico;

        public HorarioUnidadeAplicacao(IHorarioUnidadeServico HorarioUnidadeServico)
        {
            _HorarioUnidadeServico = HorarioUnidadeServico;
        }

        public void Salvar(HorarioUnidade HorarioUnidade, int idUsuario)
        {
            _HorarioUnidadeServico.Salvar(HorarioUnidade, idUsuario);
        }
    }
}
