using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class NecessidadeCompra : BaseEntity
    {
        public virtual DateTime DataNotificacaoValidade { get; set; }

        public virtual IList<NecessidadeCompraMaterialFornecedor> MaterialFornecedores { get; set; }

        public virtual StatusNecessidadeCompra StatusNecessidadeCompra { get; set; }

        public virtual Cotacao Cotacao { get; set; }

        public virtual void AdicionarNecessidadeCompraAosMaterialFornecedores()
        {
            foreach (var item in MaterialFornecedores)
            {
                item.NecessidadeCompra = this;
            }
        }
    }
}