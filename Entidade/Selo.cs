using Entidade.Base;
using System;
namespace Entidade
{
    public class Selo : BaseEntity
    {
        public virtual int Sequencial { get; set; }
        public virtual DateTime? Validade { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual EmissaoSelo EmissaoSelo { get; set; }
    }
}