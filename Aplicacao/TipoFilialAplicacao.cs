using System;
using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface ITipoFilialAplicacao : IBaseAplicacao<TipoFilial>
    {
        IList<Usuario> BuscarUsuarios();
    }
    public class TipoFilialAplicacao : BaseAplicacao<TipoFilial, ITipoFilialServico>, ITipoFilialAplicacao
    {
        private readonly IUsuarioAplicacao _usuarioAplicacao;

        public TipoFilialAplicacao(IUsuarioAplicacao usuarioAplicacao)
        {
            _usuarioAplicacao = usuarioAplicacao;
        }
        
        public IList<Usuario> BuscarUsuarios()
        {
            throw new System.NotImplementedException();
        }

        public new void Salvar(TipoFilial entity)
        {
            var tipoFilialRetorno = BuscarPorId(entity.Id) ?? entity;

            tipoFilialRetorno.Id = entity.Id;
            tipoFilialRetorno.Nome = entity.Nome;

            if(string.IsNullOrEmpty(tipoFilialRetorno.Nome)) throw new Exception("Campo preenchidos incorretamente!");

            Servico.Salvar(tipoFilialRetorno);
        }
    }
}
