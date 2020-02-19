using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface IVagaCortesiaServico : IBaseServico<VagaCortesia>
    {
        void Salvar(VagaCortesia VagaCortesia);
    }
    public class VagaCortesiaServico : BaseServico<VagaCortesia, IVagaCortesiaRepositorio>, IVagaCortesiaServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;

        public VagaCortesiaServico(IUsuarioRepositorio usuarioRepositorio,
                                      ITipoNotificacaoRepositorio tipoNotificacaoRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
        }

        public void Salvar(VagaCortesia VagaCortesia, int idUsuario)
        {
            Repositorio.Save(VagaCortesia);
        }
    }
}
