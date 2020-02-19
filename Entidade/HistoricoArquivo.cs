using System;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class HistoricoArquivo : BaseEntity
    {
        public HistoricoArquivo()
        {
            DataAtualizacao = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        public virtual DateTime DataAtualizacao { get; set; }
        public virtual StatusArquivoSpcCartorio StatusArquivoSpcCartorio { get; set; }
        public virtual string Destino { get; set; }
        public virtual string NomeArquivo { get; set; }
        public virtual TipoArquivoSpcCartorio TipoArquivo { get; set; }

    }
}