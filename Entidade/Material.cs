using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Exceptions;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Material : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual TipoMaterial TipoMaterial { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string Altura { get; set; }
        public virtual string Largura { get; set; }
        public virtual string Profundidade { get; set; }
        public virtual string Comprimento { get; set; }
        public virtual string EAN { get; set; }
        public virtual byte[] Imagem { get; set; }
        public virtual bool EhUmAtivo { get; set; }
        public virtual int EstoqueMaximo { get; set; }
        public virtual int EstoqueMinimo { get; set; }
        public virtual int QuantidadeTotalEstoque { get; set; }
        public virtual IList<MaterialFornecedor> MaterialFornecedores { get; set; }
        public virtual IList<MaterialNotificacao> MaterialNotificacaos { get; set; }
        public virtual bool EstoqueEstaBaixo => QuantidadeTotalEstoque <= EstoqueMinimo;
        public virtual bool TemFornecedorPersonalizado => MaterialFornecedores != null && MaterialFornecedores.Any(x => x.EhPersonalizado);
        public virtual string EmailDoFornecedorPersonalizado => MaterialFornecedores.FirstOrDefault(x => x.EhPersonalizado).Fornecedor.ContatoEmail;
        public virtual string QuantidadeParaPedidoAutomatico => MaterialFornecedores.FirstOrDefault(x => x.EhPersonalizado).QuantidadeParaPedidoAutomatico.ToString();

        public virtual void DarEntrada(int quantidadeEntrada, bool validar = true)
        {
            if(validar)
                ValidarSePodeDarEntrada(quantidadeEntrada);

            QuantidadeTotalEstoque += quantidadeEntrada;
        }

        public virtual void DarSaida(int quantidadeSaida, bool validar = true)
        {
            if(validar)
                ValidarSePodeDarSaida(quantidadeSaida);

            QuantidadeTotalEstoque -= quantidadeSaida;
        }

        public virtual void DefinirQuantidadeTotalEmEstoque(int estoqueMaterialQtAnterior, int estoqueMaterialQtNova)
        {
            QuantidadeTotalEstoque -= estoqueMaterialQtAnterior;
            QuantidadeTotalEstoque += estoqueMaterialQtNova;
        }

        public virtual void AdicionarMaterialAoMaterialFornecedor()
        {
            foreach (var item in MaterialFornecedores)
            {
                item.Material = this;
            }
        }

        private void ValidarSePodeDarEntrada(int quantidadeEntrada)
        {
            if ((quantidadeEntrada + QuantidadeTotalEstoque) > EstoqueMaximo)
                throw new BusinessRuleException($"Não é possível dar entrada em {quantidadeEntrada} porque vai exceder a quantidade máxima de {EstoqueMaximo}.");
        }

        private void ValidarSePodeDarSaida(int quantidadeSaida)
        {
            if (quantidadeSaida > QuantidadeTotalEstoque)
                throw new BusinessRuleException($"Não é possível dar saída em {quantidadeSaida} porque a quantidade total do material é de apenas {QuantidadeTotalEstoque}");
        }
    }
}