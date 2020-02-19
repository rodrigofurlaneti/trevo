using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Menu : BaseEntity
    {
        public virtual Menu MenuPai { get; set; }
        [Required(ErrorMessage = "Informe o Nome da Página!"), StringLength(100)]
        public virtual string Descricao { get; set; }
        [Required(ErrorMessage = "Informe a Posição!"), DefaultValue(1)]
        public virtual int Posicao { get; set; }
        public virtual string Url { get; set; }
        public virtual bool Ativo { get; set; }
    }
}