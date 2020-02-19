using System.Collections.Generic;
using Aplicacao.Base;
using Core.Exceptions;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ITipoMensalistaAplicacao : IBaseAplicacao<TipoMensalista>
    { }

    public class TipoMensalistaAplicacao : BaseAplicacao<TipoMensalista, ITipoMensalistaServico>, ITipoMensalistaAplicacao
    {
        public new void Salvar(TipoMensalista entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");

            var tipoMensalista = BuscarPorId(entity.Id) ?? entity;
            tipoMensalista.Id = entity.Id;
            tipoMensalista.Descricao = entity.Descricao;
            tipoMensalista.DataInsercao = entity.DataInsercao;
            tipoMensalista.Ativo = entity.Ativo;

            Servico.Salvar(tipoMensalista);
        }

        public bool ObjetoValido(TipoMensalista entity)
        {
            if (string.IsNullOrEmpty(entity.Descricao))
                throw new BusinessRuleException("Informe o Tipo de Mensalista!");

            return true;
        }
    }
}
