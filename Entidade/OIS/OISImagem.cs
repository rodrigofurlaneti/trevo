using Entidade.Base;

namespace Entidade
{
    public class OISImagem
    {
        public virtual OIS OIS { get; set; }
        public virtual byte[] ImagemUpload { get; set; }
    }
}