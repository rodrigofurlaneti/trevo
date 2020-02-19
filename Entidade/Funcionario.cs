using Core.Exceptions;
using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class Funcionario : BaseEntity, IAudit
    {
        public virtual string Codigo { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual Funcionario Supervisor { get; set; }
        public virtual string Salario { get; set; }
        public virtual byte[] Imagem { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual StatusFuncionario Status { get; set; }
        public virtual TipoEscalaFuncionario TipoEscala { get; set; }
        public virtual DateTime? DataAdmissao { get; set; }
        public virtual ItemFuncionario ItemFuncionario { get; set; }

        public virtual string PessoaNome => Pessoa?.Nome;

        public virtual BeneficioFuncionario BeneficioFuncionario { get; set; }
        public virtual OcorrenciaFuncionario OcorrenciaFuncionario { get; set; }
        public virtual IList<ControleFerias> ControlesFerias { get; set; }
        public virtual IList<FuncionarioIntervaloDozeTrintaSeis> ListaIntervaloDozeTrintaSeis { get; set; }
        public virtual IList<FuncionarioIntervaloCompensacao> ListaIntervaloCompensacao { get; set; }
        public virtual IList<FuncionarioIntervaloNoturno> ListaIntervaloNoturno { get; set; }

        public virtual void ValidarTipoEscala()
        {
            switch (TipoEscala)
            {
                case TipoEscalaFuncionario.DozeTrintaSeis:
                    if (ListaIntervaloDozeTrintaSeis == null || ListaIntervaloDozeTrintaSeis.Count == 0)
                        throw new BusinessRuleException("Adicione pelo menos um intervalo a escala de 12/36");
                    break;
                case TipoEscalaFuncionario.Compensacao:
                    if (ListaIntervaloCompensacao == null || ListaIntervaloCompensacao.Count == 0)
                        throw new BusinessRuleException("Adicione pelo menos um intervalo a escala de Compensação");
                    break;
                case TipoEscalaFuncionario.Noturno:
                    if (ListaIntervaloNoturno == null || ListaIntervaloNoturno.Count == 0)
                        throw new BusinessRuleException("Adicione pelo menos um intervalo a escala Noturna");
                    break;
            }
        }
    }
}