using System;
using System.ComponentModel.DataAnnotations;
using Core.Exceptions;
using Core.Validators;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Documento : BaseEntity
    {
        protected Documento()
        {
        }

        public Documento(TipoDocumento tipo, string numero, DateTime dataInsercao, int id = 0,
            string orgaoExpedidor = "", Pessoa pessoa = null, DateTime? dataExpedicao = null, bool validar = true)
        {
            if (validar &&
                !Validar(tipo, numero))
                throw new BusinessRuleException("Documento Inválido");

            Tipo = tipo;
            Numero = numero;
            OrgaoExpedidor = orgaoExpedidor;
            Pessoa = pessoa;
            DataExpedicao = dataExpedicao;
        }

        [Required]
        public virtual TipoDocumento Tipo { get; protected set; }

        public virtual string Numero { get; protected set; }

        public virtual string OrgaoExpedidor { get; protected set; }

        public virtual DateTime? DataExpedicao { get; protected set; }

        public virtual Pessoa Pessoa { get; protected set; }

        private static bool Validar(TipoDocumento tipo, string numero)
        {
            switch (tipo)
            {
                case TipoDocumento.Cpf:
                    return Validators.IsCpf(numero);
                case TipoDocumento.Cnpj:
                    return Validators.IsCnpj(numero);
            }

            return true;
        }

        public static Documento NovoDocumento(Documento documentoAnterior, string numero)
        {
            var documentoNovo = new Documento(documentoAnterior.Tipo,
                numero, documentoAnterior.DataInsercao, documentoAnterior.Id, 
                documentoAnterior.OrgaoExpedidor, documentoAnterior.Pessoa, documentoAnterior.DataExpedicao);

            return documentoNovo;
        }
    }
}