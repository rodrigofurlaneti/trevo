using Aplicacao.Base;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface IMarcaAplicacao : IBaseAplicacao<Marca>
    {
    }

    public class MarcaAplicacao : BaseAplicacao<Marca, IMarcaServico>, IMarcaAplicacao
    {
        public readonly IMarcaServico marcaServico;

        public new void Salvar(Marca entity)
        {
            var marcaRetorno = BuscarPorId(entity.Id) ?? entity;

            marcaRetorno.Id = entity.Id;
            marcaRetorno.Nome = entity.Nome;

            if (string.IsNullOrEmpty(marcaRetorno.Nome)) throw new Exception("Campo preenchidos incorretamente!");

            Servico.Salvar(marcaRetorno);
        }
    }
}