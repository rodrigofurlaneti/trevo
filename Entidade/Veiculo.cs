using Entidade.Base;
using Entidade.Uteis;
using System;

namespace Entidade
{
    public class Veiculo : BaseEntity
    {
        public virtual Modelo Modelo { get; set; }

        public virtual string Cor { get; set; }

        public virtual int? Ano { get; set; }

        public virtual TipoVeiculo TipoVeiculo { get; set; }

        public virtual string Placa { get; set; }
        
        public virtual string VeiculoFull
        {
            get
            {
                return String.Format("{0} {1}, {2}", "Placa: " + Placa, "Modelo: " + Modelo.Descricao, "Marca: "+ Modelo.Marca.Nome);
            }
        }
    }
}