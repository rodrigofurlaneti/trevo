using Entidade.Base;

namespace Entidade
{
    public class PrecoParametroSelo : BaseEntity
    {
        public virtual Unidade Unidade { get; set; }
        //public virtual Preco Preco { get; set; }

        //public virtual int TipoPreco { get; set; }
        public virtual TipoSelo TipoPreco { get; set; }

        public virtual decimal DescontoTabelaPreco { get; set; }

        public virtual decimal DescontoMaximoValor { get; set; }
        public virtual Perfil Perfil { get; set; }

        public virtual decimal DescontoCustoTabelaPreco { get; set; }
    }
}