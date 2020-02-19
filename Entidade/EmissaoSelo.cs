using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class EmissaoSelo : BaseEntity
    {
        public virtual DateTime? Validade { get; set; }
        public virtual StatusSelo StatusSelo { get; set; }
        public virtual bool EntregaRealizada { get; set; }
        public virtual DateTime? DataEntrega { get; set; }
        public virtual string Responsavel { get; set; }
        public virtual string ClienteRemetente { get; set; }
        public virtual IList<Selo> Selo { get; set; }
        public virtual PedidoSelo PedidoSelo { get; set; }
        public virtual Usuario UsuarioAlteracaoStatus { get; set; }
        public virtual string NumeroLote { get; set; }
        public virtual string NomeImpressaoSelo { get; set; }
    }
}