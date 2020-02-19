using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IHorarioUnidadeNotificacaoAplicacao : IBaseAplicacao<HorarioUnidadeNotificacao>
    {
        void Aprovar(HorarioUnidadeNotificacao model);
        void Reprovar(HorarioUnidadeNotificacao model);
    }

    public class HorarioUnidadeNotificacaoAplicacao : BaseAplicacao<HorarioUnidadeNotificacao, IHorarioUnidadeNotificacaoServico>, IHorarioUnidadeNotificacaoAplicacao
    {
        private readonly IParametroNotificacaoAplicacao _parametroNotificacaoAplicacao;
        private readonly IHorarioUnidadeAplicacao _HorarioUnidadeAplicacao;

        public HorarioUnidadeNotificacaoAplicacao(IParametroNotificacaoAplicacao parametroNotificacaoAplicacao, IHorarioUnidadeAplicacao HorarioUnidadeAplicacao)
        {
            _parametroNotificacaoAplicacao = parametroNotificacaoAplicacao;
            _HorarioUnidadeAplicacao = HorarioUnidadeAplicacao;
        }
        
        public void Aprovar(HorarioUnidadeNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Aprovado;
            Salvar(model);
        }

        public void Reprovar(HorarioUnidadeNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Reprovado;
            Salvar(model);
        }
    }
}
