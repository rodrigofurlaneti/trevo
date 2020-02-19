using AutoMapper;
using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class EstoqueManualItemViewModel
    {
        public int Id { get; set; }

        public int CodigoMaterial { get; set; }
        public string NumeroMaterial { get; set; }
        public bool Inventariado { get; set; }

        public EstoqueViewModel Estoque { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public MaterialViewModel Material { get; set; }

        public EstoqueManualItemViewModel()
        {

        }

        public EstoqueManualItemViewModel(EstoqueManualItem EstoqueManualItem)
        {
            if (EstoqueManualItem != null)
            {
                Id = EstoqueManualItem.Id;

                CodigoMaterial = EstoqueManualItem.CodigoMaterial;
                NumeroMaterial = EstoqueManualItem.NumeroMaterial;
                Estoque = EstoqueManualItem.Estoque != null ? new EstoqueViewModel(EstoqueManualItem.Estoque) : null;
                Unidade = EstoqueManualItem.Unidade != null ? new UnidadeViewModel(EstoqueManualItem.Unidade) : null; ;
                Material = Mapper.Map<MaterialViewModel>(EstoqueManualItem.Material);
            }
        }

        public EstoqueManualItem ToEntity()
        {
            var entidade = new EstoqueManualItem
            {
                Id = Id,
                CodigoMaterial = CodigoMaterial,
                NumeroMaterial = NumeroMaterial,
                Estoque = Estoque?.ToEntity(),
                Unidade = Unidade?.ToEntity(),
                Material = Mapper.Map<Material>(Material)
            };

            return entidade;
        }
    }
}