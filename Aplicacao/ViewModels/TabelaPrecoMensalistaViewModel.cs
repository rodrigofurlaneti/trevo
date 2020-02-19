using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoMensalistaViewModel
    {
        public int Id { get; set; }
        public StatusSolicitacao Status { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public int DiasCalculo { get; set; }
        public string UnidadesLista { get; set; }

        public IList<TabelaPrecoMensalistaUnidadeViewModel> TabelaPrecoUnidade { get; set; }

        public IList<TabelaPrecoMensalistaNotificacaoViewModel> Notificacoes { get; set; }

        public TabelaPrecoMensalistaViewModel()
        {

        }

        public TabelaPrecoMensalistaViewModel(TabelaPrecoMensalista TabelaPrecoMensalista)
        {
            Id = TabelaPrecoMensalista.Id;
            Nome = TabelaPrecoMensalista.Nome;
            Valor = TabelaPrecoMensalista.Valor.ToString();
            DiasCalculo = TabelaPrecoMensalista.DiasCalculo;
            Status = TabelaPrecoMensalista.Status;

            TabelaPrecoUnidade = TabelaPrecoMensalista.TabelaPrecoUnidade.Select(x => new TabelaPrecoMensalistaUnidadeViewModel(x)).ToList();
            Notificacoes = TabelaPrecoMensalista?.Notificacoes?.Select(x => new TabelaPrecoMensalistaNotificacaoViewModel()).ToList();


            TabelaPrecoMensalista.TabelaPrecoUnidade.ToList().ForEach(x => {
                UnidadesLista = UnidadesLista + x.Unidade.Nome + ", ";
            });

            if(!string.IsNullOrEmpty(UnidadesLista) && UnidadesLista.Contains(','))
            {
                UnidadesLista = UnidadesLista.Remove(UnidadesLista.Length - 2, 1);
            }
        }

        public TabelaPrecoMensalista ToEntity()
        {
            var entidade = new TabelaPrecoMensalista
            {
                Id = Id,
                Nome = Nome,
                Valor = string.IsNullOrEmpty(Valor) ? 0 : Convert.ToDecimal(Valor.Replace(".", "")),
                Status = Status,
                DiasCalculo = DiasCalculo,
                TabelaPrecoUnidade = this.TabelaPrecoUnidade == null ? null : this.TabelaPrecoUnidade?.Select(x => x?.ToEntity())?.ToList() ?? new List<TabelaPrecoMensalistaUnidade>()
            };

            return entidade;
        }
    }
}


