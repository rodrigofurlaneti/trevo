using Entidade.Base;
using Entidade.Uteis;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class ControlePontoFeriasUnidadeApoio
    {
        public virtual DateTime Data { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual string HorarioEntrada { get; set; }
        public virtual string HorarioSaida { get; set; }
        public virtual TipoHoraExtra TipoHoraExtra { get; set; }
    }
}
