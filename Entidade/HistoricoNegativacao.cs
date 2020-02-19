using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Attributes;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class HistoricoNegativacao : BaseEntity
    {
        public HistoricoNegativacao()
        {
            Contrato = new Contrato();
            DataSolicitacao = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        public virtual DateTime DataSolicitacao { get; set; }

        public virtual HistoricoNegativacaoDestino Destino { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual HistoricoArquivo Arquivo { get; set; }

        public virtual DateTime? DataEfetivacao { get; set; }

        public virtual TipoOperacaoNegativacao TipoOperacao { get; set; }

        public virtual RetornoNegativacao Status { get; set; }
    }
}