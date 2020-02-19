using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class DevedorArquivoImportacaoViewModel
    {
        public int Id { get; set; }
        public int Devedor { get; set; }
        public DateTime DataInsercao { get; set; }
        public ArquivoImportacaoViewModel ArquivoImportacao { get; set; }

        public DevedorArquivoImportacaoViewModel(DevedorArquivoImportacao entity)
        {
            this.Id = entity?.Id ?? 0;
            this.Devedor = entity?.Devedor?.Id ?? 0;
            this.DataInsercao = entity?.DataInsercao ?? DateTime.Now;
            this.ArquivoImportacao = new ArquivoImportacaoViewModel(entity?.ArquivoImportacao);
        }

        public DevedorArquivoImportacao ToEntity() => new DevedorArquivoImportacao
        {
            Id = this.Id,
            DataInsercao = DateTime.Now,
            ArquivoImportacao = this.ArquivoImportacao.ToEntity(),
            Devedor = new Devedor { Id = this.Id }
        };
    }
}