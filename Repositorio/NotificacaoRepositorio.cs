using System;
using System.Collections.Generic;
using System.Text;
using Core.Exceptions;
using Core.Extensions;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Base;
using Entidade.Uteis;
using Repositorio.Base;

namespace Repositorio
{
    public class NotificacaoRepositorio : NHibRepository<Notificacao>, INotificacaoRepositorio
    {
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        public NotificacaoRepositorio(NHibContext context, ITipoNotificacaoRepositorio tipoNotificacaoRepositorio)
            : base(context)
        {
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
        }

        public IList<Notificacao> ObterNotificacoes(int idUsuario, int? tipoNotificacao = null)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT DISTINCT n FROM Notificacao n, ParametroNotificacao pn ");
            sql.Append("  JOIN n.TipoNotificacao tp ");
            sql.Append("  LEFT JOIN n.NotificacaoUsuarioAprovadores nua ");
            sql.Append("  JOIN pn.Aprovadores a ");
            sql.Append($" WHERE (n.Status = {(int)StatusNotificacao.Aguardando} OR n.Status = {(int)StatusNotificacao.Visualizacao}) AND tp.Entidade = pn.TipoNotificacao.Entidade ");

            if (IsSql())
                sql.Append($"     AND (n.DataVencimentoNotificacao IS NULL OR :dataAtual <= Convert(DATE, n.DataVencimentoNotificacao))");

            else if (IsMySql())
                sql.Append($"     AND (n.DataVencimentoNotificacao IS NULL OR :dataAtual <= Convert(n.DataVencimentoNotificacao, DATE))");

            if (idUsuario > 0)
                sql.Append($" AND (a.Usuario.Id = {idUsuario} Or nua.UsuarioAprovador.Id = {idUsuario})");

            if (tipoNotificacao.HasValue && tipoNotificacao > 0)
                sql.Append($" AND tp.Entidade = {tipoNotificacao.Value}");

            var query = Session.CreateQuery(sql.ToString());

            query.SetParameter("dataAtual", DateTime.Now.Date);

            return query.List<Notificacao>() ?? new List<Notificacao>();
        }

        public Notificacao SalvarNotificacaoComRetorno(IEntity entidade, Entidades enumEntidades, Usuario usuario, DateTime? dataValidade,
            string descricao = "", string urlPersonalizada = "", TipoAcaoNotificacao tipoAcao = TipoAcaoNotificacao.AprovarReprovar,
            List<Usuario> UsuariosAprovadores = null)
        {
            var tipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == enumEntidades) ?? null;
            if (tipoNotificacao == null)
                throw new BusinessRuleException("Tipo de Notificação não identificada! Contate o Suporte.");

            descricao = string.IsNullOrEmpty(descricao) ? $"{enumEntidades.ToDescription()} ID: {entidade.Id.ToString()}" : descricao;

            var notificacaoUsuarioAprovadores = new List<NotificacaoUsuarioAprovador>();
            if (UsuariosAprovadores != null)
            {
                foreach (var usuarioAprovador in UsuariosAprovadores)
                {
                    notificacaoUsuarioAprovadores.Add(new NotificacaoUsuarioAprovador
                    {
                        UsuarioAprovador = usuarioAprovador
                    });
                }
            }

            var notificacao = new Notificacao
            {
                Usuario = usuario,
                TipoNotificacao = tipoNotificacao,
                Status = StatusNotificacao.Aguardando,
                Descricao = descricao,
                DataInsercao = DateTime.Now,
                AcaoNotificacao = tipoAcao,
                DataVencimentoNotificacao = dataValidade == null || dataValidade <= System.Data.SqlTypes.SqlDateTime.MinValue.Value
                                            ? DateTime.Now.AddDays(+4).Date
                                            : dataValidade,
                UrlPersonalizada = urlPersonalizada,
                NotificacaoUsuarioAprovadores = notificacaoUsuarioAprovadores
            };
            Save(notificacao);

            return GetById(notificacao.Id);
        }
    }
}