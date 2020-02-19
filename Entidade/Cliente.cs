using Entidade.Base;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Linq;

namespace Entidade
{
    public class Cliente : BaseEntity, IAudit
    {
        public virtual int? IdSoftpark { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual IList<ClienteVeiculo> Veiculos { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual TipoPessoa TipoPessoa { get; set; }
        public virtual IList<ClienteUnidade> Unidades { get; set; }
        public virtual SeloCliente SeloCliente { get; set; }
        private List<int> ListaIds { get; set; }
        public virtual List<OcorrenciaCliente> Ocorrencias { get; set; }
        public virtual List<int> ListaUnidade
        {
            get
            {

                var retorno = new List<int>();

                if (Unidades != null)
                    retorno.AddRange(Unidades.Select(ClienteUnidade => ClienteUnidade.Unidade.Id));

                return ListaIds != null && ListaIds.Any()
                        ? ListaIds
                        : retorno;
            }
            set { ListaIds = value; }
        }

        public virtual bool ExigeNotaFiscal { get; set; }

        public virtual bool NotaFiscalSemDesconto { get; set; }

        public virtual string NomeConvenio { get; set; }
        public virtual string Observacao { get; set; }

        public virtual string NomeExibicao
        {
            get
            {
                return string.IsNullOrEmpty(NomeFantasia)
                        ? string.IsNullOrEmpty(RazaoSocial)
                            ? string.IsNullOrEmpty(Pessoa?.Nome)
                                ? string.Empty
                                : Pessoa?.Nome
                            : RazaoSocial
                        : NomeFantasia;
            }
        }

        public virtual ContaCorrenteCliente ContaCorrenteCliente { get; set; }
    }
}