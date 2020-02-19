using Entidade.Base;
using Entidade.Uteis;
using System;

namespace Entidade
{
    public class OcorrenciaCliente : BaseEntity, IAudit
    {
        public virtual string NumeroProtocolo { get; set; }
        public virtual DateTime DataCompetencia { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual Veiculo Veiculo { get; set; }
        public virtual Funcionario FuncionarioAtribuido { get; set; }
        public virtual TipoNatureza Natureza { get; set; }
        public virtual TipoOrigem Origem { get; set; }
        public virtual TipoPrioridade Prioridade { get; set; }
        public virtual StatusOcorrencia StatusOcorrencia { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string Solucao { get; set; }
        public virtual DateTime DataOcorrencia { get; set; }
    }
}
