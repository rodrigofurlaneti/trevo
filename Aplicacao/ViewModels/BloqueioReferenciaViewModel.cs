using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class BloqueioReferenciaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public DateTime DataMesAnoReferencia { get; set; }
        public bool Ativo { get; set; }

        public BloqueioReferenciaViewModel()
        {
        }

        public BloqueioReferenciaViewModel(BloqueioReferencia obj)
        {
            Id = obj.Id;
            DataInsercao = obj.DataInsercao;
            DataMesAnoReferencia = obj.DataMesAnoReferencia;
            Ativo = obj.Ativo;
        }

        public BloqueioReferencia ToEntity() => new BloqueioReferencia
        {
            Id = Id,
            DataInsercao = DateTime.Now,
            DataMesAnoReferencia = DataMesAnoReferencia,
            Ativo = Ativo
        };

    }
}