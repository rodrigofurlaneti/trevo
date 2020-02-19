using Entidade.Base;

namespace Entidade
{
    public class Modelo : BaseEntity
    {
        public virtual string Descricao { get; set; }     
        public virtual Marca Marca { get; set; }
    }
}