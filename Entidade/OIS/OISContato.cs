using Entidade.Base;

namespace Entidade
{
    public class OISContato
    {
        public virtual OIS OIS { get; set; }
        public virtual Contato Contato { get; set; }
    }
}