using Entidade.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class ParametrizacaoLocacao : BaseEntity
    {
        public virtual TipoLocacao TipoLocacao { get; set; }
        public virtual Unidade Unidade { get; set; }
    }
}