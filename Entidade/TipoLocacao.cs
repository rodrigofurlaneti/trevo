using Core.Exceptions;
using Entidade.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class TipoLocacao : BaseEntity
    {
        public virtual string Descricao { get; set; }

        public virtual bool Validar()
        {
            if (string.IsNullOrEmpty(Descricao))
                throw new BusinessRuleException("Informe o Tipo de Locação!");

            if (Descricao.Length > 30)
                throw new BusinessRuleException("Tipo de Locação não pode ultrapassar 30 caracteres!");

            return true;
        }
    }
}