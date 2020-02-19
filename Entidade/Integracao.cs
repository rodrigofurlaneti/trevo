using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Attributes;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Integracao : BaseEntity
    {
        public virtual TipoLeiauteImportacao Leiaute { get; set; }
        public virtual int Lote { get; set; }
        public virtual Assessoria Assessoria { get; set; }
        public virtual string NomeArquivo { get; set; }
        public virtual DateTime DataHoraArquivo { get; set; }
        public virtual Usuario UsuarioImportacao { get; set; }
        public virtual bool Status { get; set; }

        public Integracao()
        {
            DataHoraArquivo = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
        
    }
}