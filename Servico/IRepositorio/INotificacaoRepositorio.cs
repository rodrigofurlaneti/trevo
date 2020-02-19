using System;
using System.Collections.Generic;
using Dominio.IRepositorio.Base;
using Entidade;
using Entidade.Base;
using Entidade.Uteis;

namespace Dominio.IRepositorio
{
    public interface INotificacaoRepositorio : IRepository<Notificacao>
    {
        IList<Notificacao> ObterNotificacoes(int idUsuario, int? tipoNotificacao = null);
        Notificacao SalvarNotificacaoComRetorno(IEntity entidade, Entidades enumEntidades, Usuario usuario, DateTime? dataValidade,
            string descricao = "", string urlPersonalizada = "", TipoAcaoNotificacao tipoAcao = TipoAcaoNotificacao.AprovarReprovar,
            List<Usuario> UsuariosAprovadores = null);
    }
}