using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class DescontoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public bool NecessitaAprovacao { get; set; }
        public TipoDesconto TipoDesconto { get; set; }
        public IList<NegociacaoSeloDescontoNotificacao> Notificacoes { get; set; }

        public DescontoViewModel()
        {

        }

        public DescontoViewModel(Desconto entidade)
        {
            if (entidade != null)
            {
                Id = entidade.Id;
                DataInsercao = entidade.DataInsercao;
                Descricao = entidade.Descricao;
                Valor = entidade.Valor.ToString();
                NecessitaAprovacao = entidade.NecessitaAprovacao;
                TipoDesconto = entidade.TipoDesconto;
                Notificacoes = entidade.Notificacoes;
            }
        }

        public Desconto ToEntity()
        {
            return new Desconto
            {
                DataInsercao = DataInsercao,
                Descricao = Descricao,
                Id = Id,
                NecessitaAprovacao = NecessitaAprovacao,
                Notificacoes = Notificacoes,
                //Status
                TipoDesconto = TipoDesconto,
                Valor = Convert.ToDecimal(Valor)
            };
        }
    }
}