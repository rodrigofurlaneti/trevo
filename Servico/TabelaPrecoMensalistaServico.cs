using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface ITabelaPrecoMensalistaServico : IBaseServico<TabelaPrecoMensalista>
    {
        void Salvar(TabelaPrecoMensalista TabelaPrecoMensalista, int idUsuario);
    }
    public class TabelaPrecoMensalistaServico : BaseServico<TabelaPrecoMensalista, ITabelaPrecoMensalistaRepositorio>, ITabelaPrecoMensalistaServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly ITabelaPrecoMensalistaNotificacaoServico _TabelaPrecoMensalistaNotificacaoServico;

        public TabelaPrecoMensalistaServico(IUsuarioRepositorio usuarioRepositorio,
                                      ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                      ITabelaPrecoMensalistaNotificacaoServico TabelaPrecoMensalistaNotificacaoServico
                                      )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _TabelaPrecoMensalistaNotificacaoServico = TabelaPrecoMensalistaNotificacaoServico;
        }

        public void Salvar(TabelaPrecoMensalista TabelaPrecoMensalista, int idUsuario)
        {
            TabelaPrecoMensalista.Status = StatusSolicitacao.Aguardando;

            if (TabelaPrecoMensalista.Id == 0)
                Repositorio.Save(TabelaPrecoMensalista);

            _TabelaPrecoMensalistaNotificacaoServico.Criar(Repositorio.GetById(TabelaPrecoMensalista.Id), idUsuario);

            Repositorio.Save(TabelaPrecoMensalista);
        }
    }
}
