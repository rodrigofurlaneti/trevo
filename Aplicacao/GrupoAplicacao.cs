using System;
using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface IGrupoAplicacao : IBaseAplicacao<Grupo>
    {
        IList<Usuario> BuscarUsuarios();
    }

    public class GrupoAplicacao : BaseAplicacao<Grupo, IGrupoServico>, IGrupoAplicacao
    {
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        public GrupoAplicacao(IUsuarioAplicacao usuarioAplicacao)
        {
            _usuarioAplicacao = usuarioAplicacao;
        }

        public IList<Usuario> BuscarUsuarios()
        {
            return _usuarioAplicacao.Buscar() ?? new List<Usuario>();
        }

        public new void Salvar(Grupo entity)
        {
            var grupoRetorno = BuscarPorId(entity.Id) ?? entity;

            grupoRetorno.Id = entity.Id;
            grupoRetorno.Nome = entity.Nome;

            if (string.IsNullOrEmpty(grupoRetorno.Nome)) throw new Exception("Campo preenchidos incorretamente!");

            Servico.Salvar(grupoRetorno);
        }

    }
}