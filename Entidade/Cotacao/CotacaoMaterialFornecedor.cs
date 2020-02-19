using System;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class CotacaoMaterialFornecedor : BaseEntity
    {
        public virtual Cotacao Cotacao { get; set; }
        public virtual Material Material { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual decimal ValorTotal { get; set; }
    }
}