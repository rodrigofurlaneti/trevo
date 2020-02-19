using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class MovimentacaoSeloSoftparkViewModel : BaseSoftparkViewModel
    {
        public int SeloId { get; set; }
        public SeloSoftparkViewModel Selo { get; set; }
        public int MovimentoId { get; set; }
        public MovimentacaoSoftparkViewModel Movimentacao { get; set; }

        public MovimentacaoSeloSoftparkViewModel()
        {
        }

        public MovimentacaoSeloSoftparkViewModel(MovimentacaoSelo movimentacaoSelo, MovimentacaoSoftparkViewModel movimentacao)
        {
            Id = movimentacaoSelo.IdSoftpark.HasValue && movimentacaoSelo.IdSoftpark.Value > 0 ? movimentacaoSelo.IdSoftpark.Value : movimentacaoSelo.Id;
            DataInsercao = movimentacaoSelo.DataInsercao;
            SeloId = movimentacaoSelo.Selo.Id;
            Selo = new SeloSoftparkViewModel(movimentacaoSelo.Selo);
            MovimentoId = movimentacaoSelo.Movimentacao.Id;
            Movimentacao = movimentacao;
        }

        public MovimentacaoSelo ToEntity()
        {
            return new MovimentacaoSelo
            {
                Id = 0,
                IdSoftpark = this.Id > 0 ? this.Id : default(int?),
                DataInsercao = this.DataInsercao,
                Selo = this.Selo?.ToEntity(),
                Movimentacao = this.Movimentacao?.ToEntity(),
                MovimentacaoIdSoftpark = this.Movimentacao != null && this.Movimentacao.Id > 0 ? this.Movimentacao.Id : default(int?)
            };
        }
    }
}
