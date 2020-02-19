using System;
using System.ComponentModel.DataAnnotations;
using Core.Exceptions;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class EstoqueMaterial : BaseEntity
    {
        public virtual Estoque Estoque { get; set; }
        public virtual Material Material { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual decimal Preco { get; set; }
        public virtual decimal PrimeiroPreco { get; set; }
        public virtual decimal ValorTotal { get; set; }

        protected EstoqueMaterial()
        {
        }

        public EstoqueMaterial(Estoque estoque, Material material)
        {
            Estoque = estoque;
            Material = material;
        }

        public virtual void DarEntrada(int quantidadeEntrada, decimal preco)
        {
            Quantidade += quantidadeEntrada;
            ValorTotal += quantidadeEntrada * preco;

            if (PrimeiroPreco == 0)
                PrimeiroPreco = preco;

            AtualizarPreco();
        }

        public virtual void DarSaida(int quantidadeSaida, bool validar = true)
        {
            if(validar)
                ValidarSePodeDarSaida(quantidadeSaida);

            Quantidade -= quantidadeSaida;
            ValorTotal -= quantidadeSaida * Preco;
        }

        public virtual void DefinirQuantidadeEmEstoque(int quantidade)
        {
            Quantidade = quantidade;
            ValorTotal = quantidade * Preco;
        }

        private void ValidarSePodeDarSaida(int quantidadeSaida)
        {
            if (quantidadeSaida > Quantidade)
                throw new BusinessRuleException($"Não é possível dar saída em {quantidadeSaida} porque a quantidade no estoque {Estoque.Nome} é de apenas {Quantidade}");
        }

        private void AtualizarPreco()
        {
            Preco = ValorTotal / Quantidade;
        }
    }
}