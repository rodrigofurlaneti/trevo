using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface IBloqueioReferenciaAplicacao : IBaseAplicacao<BloqueioReferencia>
    {
        bool PossuiPermissaoLiberacao(Usuario usuarioLogado);
        bool LiberarBloqueioReferencia(DateTime dataReferencia, Usuario usuarioLogado);
        KeyValuePair<int, StatusDesbloqueioLiberacao> ValidarLiberacao(int idNotificacao, int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, Usuario usuarioLogado, string nomeArquivo = null);
        KeyValuePair<int, StatusDesbloqueioLiberacao> GerarNovaRequisicao(int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, Usuario usuarioLogado);
    }

    public class BloqueioReferenciaAplicacao : BaseAplicacao<BloqueioReferencia, IBloqueioReferenciaServico>, IBloqueioReferenciaAplicacao
    {
        private readonly IBloqueioReferenciaServico _bloqueioReferenciaServico;

        public BloqueioReferenciaAplicacao(IBloqueioReferenciaServico bloqueioReferenciaServico)
        {
            _bloqueioReferenciaServico = bloqueioReferenciaServico;
        }

        public bool PossuiPermissaoLiberacao(Usuario usuarioLogado)
        {
            return _bloqueioReferenciaServico.PossuiPermissaoLiberacao(usuarioLogado);
        }

        public bool LiberarBloqueioReferencia(DateTime dataReferencia, Usuario usuarioLogado)
        {
            return _bloqueioReferenciaServico.LiberarBloqueioReferencia(dataReferencia, usuarioLogado);
        }

        public KeyValuePair<int, StatusDesbloqueioLiberacao> ValidarLiberacao(int idNotificacao, int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, Usuario usuarioLogado, string nomeArquivo = null)
        {
            return _bloqueioReferenciaServico.ValidarLiberacao(idNotificacao, idRegistro, entidadeRegistro, dataReferencia, usuarioLogado, nomeArquivo);
        }

        public KeyValuePair<int, StatusDesbloqueioLiberacao> GerarNovaRequisicao(int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, Usuario usuarioLogado)
        {
            return _bloqueioReferenciaServico.GerarNovaRequisicao(idRegistro, entidadeRegistro, dataReferencia, usuarioLogado);
        }
    }
}