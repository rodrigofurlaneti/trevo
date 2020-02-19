using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class OIS : BaseEntity
    {
        public virtual DateTime DataAtualizacao { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual IList<OISContato> OISContatos { get; set; }
        public virtual Marca Marca { get; set; }
        public virtual Modelo Modelo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual TipoVeiculo TipoVeiculo { get; set; }
        public virtual string Placa { get; set; }
        public virtual string Cor { get; set; }
        public virtual string Ano { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual IList<OISCategoria> OISCategorias { get; set; }
        public virtual IList<OISFuncionario> OISFuncionarios { get; set; }
        public virtual StatusSinistro StatusSinistro { get; set; }
        public virtual string Observacao { get; set; }
        public virtual IList<OISImagem> OISImagens { get; set; }

        public virtual IList<OISNotificacao> OISNotificacoes { get; set; }
    }
}