using Entidade.Base;
using Entidade.Uteis;
using System.Collections.Generic;

namespace Entidade
{
    public class Preco : BaseEntity
    {
        public Preco()
        {
            Funcionamentos = new List<Funcionamento>();
            //Mensalistas = new List<Mensalista>();
            //Alugueis = new List<Aluguel>();
            PrecoStatus = StatusPreco.Pendente;
            Ativo = true;
        }

        public virtual string Nome { get; set; }
        public virtual int TempoTolerancia { get; set; }
        public virtual string NomeUsuario { get; set; }
        public virtual StatusPreco PrecoStatus { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual IList<Funcionamento> Funcionamentos { get; set; }
        //public virtual IList<PrecoNotificacao> Notificacoes { get; set; }
    }
}