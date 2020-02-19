using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface IBloqueioReferenciaServico : IBaseServico<BloqueioReferencia>
    {
        bool PossuiPermissaoLiberacao(Usuario usuarioLogado);
        bool LiberarBloqueioReferencia(DateTime dataReferencia, Usuario usuarioLogado);
        KeyValuePair<int, StatusDesbloqueioLiberacao> ValidarLiberacao(int idNotificacao, int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, Usuario usuarioLogado, string nomeArquivo = null);
        KeyValuePair<int, StatusDesbloqueioLiberacao> GerarNovaRequisicao(int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, Usuario usuarioLogado);
    }

    public class BloqueioReferenciaServico : BaseServico<BloqueioReferencia, IBloqueioReferenciaRepositorio>, IBloqueioReferenciaServico
    {
        private readonly IBloqueioReferenciaRepositorio _bloqueioReferenciaRepositorio;
        private readonly IUsuarioServico _usuarioServico;
        private readonly IPerfilServico _perfilServico;
        private readonly INotificacaoDesbloqueioReferenciaServico _notificacaoDesbloqueioReferenciaServico;

        public BloqueioReferenciaServico(IBloqueioReferenciaRepositorio bloqueioReferenciaRepositorio
                                            , IUsuarioServico usuarioServico
                                            , IPerfilServico perfilServico
                                        , INotificacaoDesbloqueioReferenciaServico notificacaoDesbloqueioReferenciaServico)
        {
            _bloqueioReferenciaRepositorio = bloqueioReferenciaRepositorio;
            _perfilServico = perfilServico;
            _usuarioServico = usuarioServico;
            _notificacaoDesbloqueioReferenciaServico = notificacaoDesbloqueioReferenciaServico;
        }

        public bool PossuiPermissaoLiberacao(Usuario usuarioLogado)
        {
            var usuarioBusca = _usuarioServico.BuscarPorId(usuarioLogado?.Id ?? 0);
            //Caso o usuário logado tenha a permissão de perfil, ele libera a referência
            if (usuarioBusca.Perfils.Any(x => x.Perfil.Permissoes.Any(y => y.Regra.Equals("liberacaoBloqueioReferencia"))))
                return true;
            return false;
        }

        public bool LiberarBloqueioReferencia(DateTime dataReferencia, Usuario usuarioLogado)
        {
            dataReferencia = new DateTime(dataReferencia.Year, dataReferencia.Month, 1);

            var registroBloqueio = PrimeiroPor(x => x.DataMesAnoReferencia == dataReferencia);
            //Não existindo ou existindo mas inativada, está liberado a referência
            if (registroBloqueio == null
                || (registroBloqueio != null && !registroBloqueio.Ativo))
                return true;

            var usuarioBusca = _usuarioServico.BuscarPorId(usuarioLogado?.Id ?? 0);
            //Caso o usuário logado tenha a permissão de perfil, ele libera a referência
            if (usuarioBusca.Perfils.Any(x => x.Perfil.Permissoes.Any(y => y.Regra.Equals("liberacaoBloqueioReferencia"))))
                return true;
            //Neste ponto entende-se que tem registro de bloqueio ativo e não possui permissão de liberação, então não é liberado
            return false;
        }

        public KeyValuePair<int, StatusDesbloqueioLiberacao> ValidarLiberacao(int idNotificacao, int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, Usuario usuarioLogado, string nomeArquivo = null)
        {
            var liberacao = LiberarBloqueioReferencia(dataReferencia, usuarioLogado);
            if (liberacao)
                return new KeyValuePair<int, StatusDesbloqueioLiberacao>(idNotificacao, StatusDesbloqueioLiberacao.Aprovado);

            var notificacao = idNotificacao > 0
                                ? _notificacaoDesbloqueioReferenciaServico.BuscarPor(x => x.Notificacao.Id == idNotificacao)?.LastOrDefault()
                                : _notificacaoDesbloqueioReferenciaServico
                                    .BuscarPor(x => x.IdRegistro == idRegistro
                                                && x.EntidadeRegistro == entidadeRegistro
                                                && x.DataReferencia == dataReferencia
                                                && x.Notificacao.Usuario.Id == usuarioLogado.Id
                                                && x.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Utilizado)?.LastOrDefault();

            if (notificacao == null || notificacao.Id == 0)
            {
                _notificacaoDesbloqueioReferenciaServico.Criar(idRegistro, entidadeRegistro, dataReferencia, usuarioLogado.Id, nomeArquivo);
                notificacao = _notificacaoDesbloqueioReferenciaServico
                                    .BuscarPor(x => x.IdRegistro == idRegistro
                                                && x.EntidadeRegistro == entidadeRegistro
                                                && x.DataReferencia == dataReferencia
                                                && x.Notificacao.Usuario.Id == usuarioLogado.Id
                                                && x.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Utilizado)?.LastOrDefault();
            }
            return new KeyValuePair<int, StatusDesbloqueioLiberacao>(notificacao.Notificacao.Id, notificacao.StatusDesbloqueioLiberacao);
        }

        public KeyValuePair<int, StatusDesbloqueioLiberacao> GerarNovaRequisicao(int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, Usuario usuarioLogado)
        {
            _notificacaoDesbloqueioReferenciaServico.Criar(idRegistro, entidadeRegistro, dataReferencia, usuarioLogado.Id);
            var notificacao = _notificacaoDesbloqueioReferenciaServico
                                    .BuscarPor(x => x.IdRegistro == idRegistro
                                                && x.EntidadeRegistro == entidadeRegistro
                                                && x.DataReferencia == dataReferencia
                                                && x.Notificacao.Usuario.Id == usuarioLogado.Id
                                                && x.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Utilizado)?.LastOrDefault();

            return new KeyValuePair<int, StatusDesbloqueioLiberacao>(notificacao.Notificacao.Id, notificacao.StatusDesbloqueioLiberacao);
        }
    }
}