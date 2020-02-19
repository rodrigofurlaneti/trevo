using Core.Attributes;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class LayoutCampo : BaseEntity
    {
        public virtual string Conteudo { get; set; }

        public virtual string Campo { get; set; }
        
        public virtual int PosicaoInicio { get; set; }

        public virtual int PosicaoFim { get; set; }

        public virtual int Tamanho { get; set; }
        
        public virtual TipoValidacao Formatacao { get; set; }

        public virtual string Preenchimento { get; set; }

        public virtual Direcao Direcao { get; set; }

        public virtual LayoutLinha LayoutLinha { get; set; }
    }
}