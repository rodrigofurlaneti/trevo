using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface IParametroEquipeServico : IBaseServico<ParametroEquipe>
    {
        void Salvar(ParametroEquipe parametroEquipe, int idUsuario);
    }
    public class ParametroEquipeServico : BaseServico<ParametroEquipe, IParametroEquipeRepositorio>, IParametroEquipeServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly IParametroEquipeNotificacaoServico _parametroEquipeNotificacaoServico;

        public ParametroEquipeServico(IUsuarioRepositorio usuarioRepositorio,
                                      ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                      IParametroEquipeNotificacaoServico parametroEquipeNotificacaoServico)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _parametroEquipeNotificacaoServico = parametroEquipeNotificacaoServico;
        }

        public void Salvar(ParametroEquipe parametroEquipe, int idUsuario)
        {
            parametroEquipe.Status = StatusSolicitacao.Aguardando;

            if (parametroEquipe.Id == 0)
                Repositorio.Save(parametroEquipe);

            _parametroEquipeNotificacaoServico.Criar(Repositorio.GetById(parametroEquipe.Id), idUsuario);
            Repositorio.Save(parametroEquipe);
        }
    }
}
