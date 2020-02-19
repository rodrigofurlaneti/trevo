using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidade
{
    public class ContasAPagar : BaseEntity, IAudit
    {
        public ContasAPagar()
        {
            DataVencimento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            DataPagamento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        [Required]
        public virtual ContaFinanceira ContaFinanceira { get; set; }

        public virtual TipoContaPagamento TipoPagamento { get; set; }

        [Required]
        public virtual TipoDocumentoConta TipoDocumentoConta { get; set; }

        public virtual string NumeroDocumento { get; set; }

        [NotMapped]
        public virtual Unidade Unidade { get; set; }

        [NotMapped]
        public virtual ContaContabil ContaContabil { get; set; }

        [Required]
        public virtual DateTime DataVencimento { get; set; }
        public virtual DateTime? DataCompetencia { get; set; }
        public virtual DateTime DataPagamento { get; set; }

        [Required]
        public virtual Departamento Departamento { get; set; }

        [Required]
        public virtual Fornecedor Fornecedor { get; set; }

        [Required]
        public virtual decimal ValorTotal { get; set; }

        [Required]
        public virtual FormaPagamento FormaPagamento { get; set; }

        [Required]
        public virtual int NumeroParcela { get; set; }

        public virtual string Observacoes { get; set; }

        [Required]
        public virtual bool PodePagarEmEspecie { get; set; }

        [Required]
        public virtual bool ValorSolicitado { get; set; }

        public virtual bool Ignorado { get; set; }

        [Required]
        public virtual StatusContasAPagar StatusConta { get; set; }

        [Required]
        public virtual bool Ativo { get; set; }

        public virtual string CodigoAgrupadorParcela { get; set; }

        public virtual string NumeroRecibo { get; set; }

        public virtual IList<ContaPagarNotificacao> ContaPagarNotificacoes { get; set; }

        public virtual bool PossueCnab { get; set; }

        public virtual TipoJurosContaPagar TipoJuros { get; set; }

        public virtual decimal ValorJuros { get; set; }

        public virtual TipoMultaContaPagar TipoMulta { get; set; }

        public virtual decimal ValorMulta { get; set; }

        public virtual IList<ContasAPagarItem> ContaPagarItens { get; set; }

        public static implicit operator ContasAPagar(int v)
        {
            throw new NotImplementedException();
        }

        [NotMapped]
        public virtual string FornecedorFormatado => Fornecedor == null
            ? ""
            : !string.IsNullOrEmpty(Fornecedor.Nome)
                ? Fornecedor.Nome
                : !string.IsNullOrEmpty(Fornecedor.NomeFantasia)
                    ? Fornecedor.NomeFantasia
                    : "";

        public virtual string Contribuinte { get; set; }
        public virtual string CodigoDeBarras { get; set; }
    }
}