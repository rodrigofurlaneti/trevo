using Entidade.Base;
using Entidade.Uteis;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Entidade
{
    public class Unidade : BaseEntity, IAudit
    {
        public Unidade()
        {
            //CheckListEstruturaUnidade = new List<CheckListEstruturaUnidade>();
        }

        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual int NumeroVaga { get; set; }
        public virtual int DiaVencimento { get; set; }
        public virtual Funcionario Responsavel { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual TiposUnidades TiposUnidades { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual MaquinaCartao MaquinaCartao { get; set; }
        public virtual IList<TipoPagamento> TiposPagamento { get; set; }
        public virtual IList<EstruturaUnidade> EstruturasUnidade { get; set; }
        public virtual IList<UnidadeCheckListAtividade> UnidadeCheckListAtividades { get; set; }
        public virtual IList<UnidadeFuncionario> UnidadeFuncionarios { get; set; }

        public virtual string CNPJ { get; set; }
        public virtual string CCM { get; set; }
        public virtual string HorarioInicial { get; set; }
        public virtual string HorarioFinal { get; set; }
        public virtual bool Ativa { get; set; }

        public virtual IList<CheckListEstruturaUnidade> CheckListEstruturaUnidade { get; set; }

        [JsonIgnore]
        public virtual CheckListAtividade CheckListAtividade { get; set; }

        public virtual IList<UnidadeCheckListAtividadeTipoAtividade> UnidadeCheckListTipoAtividades { get; set; }
    }
}