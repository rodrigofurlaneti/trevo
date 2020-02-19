using System;
using Entidade.Base;

namespace Entidade
{

    public class FeriasClienteDetalhe : IAudit
    {
        public virtual FeriasCliente FeriasCliente { get; set; }

        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime DataFim { get; set; }

        public virtual decimal ValorFeriasCalculada { get; set; }

        //Não Mapear Abaixo
        public virtual DateTime DataCompetencia
        {
            get
            {
                return new DateTime(DataFim.AddMonths(1).Year, DataFim.AddMonths(1).Month, 1);
            }
        }
        
        public virtual decimal ValorFeriasCalculadaAnterior { get; set; }
    }
}