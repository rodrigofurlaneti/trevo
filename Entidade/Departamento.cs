using Entidade.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Departamento : BaseEntity
    {
        public virtual string Nome { get; set; }

        public virtual string Sigla { get; set; }

        public virtual IList<DepartamentoFuncionario> DepartamentoResponsaveis { get; set; }

        public virtual void AddResponsaveis(List<int> responsaveisIds)
        {
            DepartamentoResponsaveis = DepartamentoResponsaveis ?? new List<DepartamentoFuncionario>();
            foreach (var responsavelId in responsaveisIds)
            {
                DepartamentoResponsaveis.Add(new DepartamentoFuncionario
                {
                    Departamento = this,
                    Funcionario = new Funcionario { Id = responsavelId }
                });
            }
        }
    }
}
