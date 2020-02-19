using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface IHorarioUnidadeServico : IBaseServico<HorarioUnidade>
    {
        void Salvar(HorarioUnidade horarioUnidade, int usuarioId);
    }
    public class HorarioUnidadeServico : BaseServico<HorarioUnidade, IHorarioUnidadeRepositorio>, IHorarioUnidadeServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly IHorarioUnidadeNotificacaoServico _horarioUnidadeNotificacaoServico;

        public HorarioUnidadeServico(IUsuarioRepositorio usuarioRepositorio,
                                      ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                      IHorarioUnidadeNotificacaoServico horarioUnidadeNotificacaoServico)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _horarioUnidadeNotificacaoServico = horarioUnidadeNotificacaoServico;
        }

        private void Validar(HorarioUnidade horarioUnidade)
        {
            if (Repositorio.FirstBy(x => x.Unidade.Id == horarioUnidade.Unidade.Id) != null)
                throw new BusinessRuleException("Já existe Horário Cadastrado para essa Unidade!");

        }

        public void Salvar(HorarioUnidade horarioUnidade, int usuarioId)
        {
            horarioUnidade.Status = StatusHorario.Aguardando;

            if (horarioUnidade.Id == 0)
               Repositorio.Save(horarioUnidade);

            _horarioUnidadeNotificacaoServico.Criar(horarioUnidade, usuarioId);

            Repositorio.Save(horarioUnidade);
        }
    }
}
