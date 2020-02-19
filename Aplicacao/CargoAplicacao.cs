using Aplicacao.Base;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface ICargoAplicacao : IBaseAplicacao<Cargo>
    {
        IList<Cargo> ListarCargo();
    }

    public class CargoAplicacao : BaseAplicacao<Cargo, ICargoServico>, ICargoAplicacao
    {
        public new void Salvar(Cargo entity)
        {
            var CargoRetorno = BuscarPorId(entity.Id) ?? entity;

            CargoRetorno.Id = entity.Id;
            CargoRetorno.Nome = entity.Nome;

            if (string.IsNullOrEmpty(CargoRetorno.Nome)) throw new Exception("Campo preenchidos incorretamente!");

            Servico.Salvar(CargoRetorno);
        }

        public IList<Cargo> ListarCargo()
        {
            return Buscar();
        }
    }
}