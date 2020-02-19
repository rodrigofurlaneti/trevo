using System;
using System.Collections.Generic;
using Core.Extensions;
using Entidade.Base;

namespace Entidade
{
    public class FeriasCliente : BaseEntity, IAudit
    {
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime DataFim { get; set; }

        public virtual bool InutilizarTodasVagas { get; set; }
        public virtual int TotalVagas { get; set; }
        public virtual ContratoMensalista ContratoMensalista { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual decimal ValorFeriasCalculada { get; set; }
        public virtual Usuario UsuarioCadastro { get; set; }

        public virtual IList<FeriasClienteDetalhe> ListaFeriasClienteDetalhe { get; set; }

        //Não Mapear
        public virtual DateTime DataCompetencia
        {
            get
            {
                return new DateTime(DataFim.AddMonths(1).Year, DataFim.AddMonths(1).Month, 1);
            }
        }

        public virtual List<DateTime> ListaDataCompetenciaPeriodo
        {
            get
            {
                var meses = DataInicio.GetMonthDifference(DataFim);
                if (meses == 0)
                    return new List<DateTime> { new DateTime(DataInicio.AddMonths(1).Year, DataInicio.AddMonths(1).Month, 1) };

                var lista = new List<DateTime>();
                for (int i = 0; i <= meses; i++)
                {
                    lista.Add(new DateTime(DataInicio.AddMonths(i + 1).Year, DataInicio.AddMonths(i + 1).Month, 1));
                }
                return lista;
            }
        }

        public virtual decimal ValorFeriasCalculadaAnterior { get; set; }
    }
}