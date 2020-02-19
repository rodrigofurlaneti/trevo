using System;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class NecessidadeCompraMaterialFornecedor : BaseEntity
    {
        public virtual NecessidadeCompra NecessidadeCompra { get; set; }
        public virtual Material Material { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }

        public virtual int Quantidade { get; set; }
    }
}