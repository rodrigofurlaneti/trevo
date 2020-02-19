using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface IPrecoServico : IBaseServico<Preco>
    {
        void Salvar(Preco preco, int idUsuario);
    }

    public class PrecoServico : BaseServico<Preco, IPrecoRepositorio>, IPrecoServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly IPrecoNotificacaoServico _precoNotificacaoServico;

        public PrecoServico(IUsuarioRepositorio usuarioRepositorio,
                                  ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                  IPrecoNotificacaoServico precoNotificacaoServico)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _precoNotificacaoServico = precoNotificacaoServico;
        }

        public void Salvar(Preco preco, int idUsuario)
        {
            if (preco.Id == 0) {
                preco.PrecoStatus = StatusPreco.Pendente;

                var idretorno = Repositorio.SaveAndReturn(preco);

                _precoNotificacaoServico.Criar(Repositorio.GetById(idretorno), idUsuario);
            }
            else
                Repositorio.Save(preco);
        }
    }
}
