using System;
using Core.Attributes;

namespace Entidade.Base
{
    public class BaseEntity : IEntity, IAudit
    {
        public BaseEntity()
        {
            DataInsercao = DateTime.Now;
        }
        
        public virtual int Id { get; set; }
        
        public virtual DateTime DataInsercao { get; set; }
    }
}