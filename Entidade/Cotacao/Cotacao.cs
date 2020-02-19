using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Cotacao : BaseEntity
    {
        public virtual IList<CotacaoMaterialFornecedor> MaterialFornecedores { get; set; }
        public virtual StatusCotacao Status { get; set; }

        public virtual void AdicionarCotacaoAosMaterialFornecedores()
        {
            foreach (var item in MaterialFornecedores)
            {
                item.Cotacao = this;
            }
        }
    }
}