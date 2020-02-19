using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aplicacao.ViewModels
{
    public class IntegracaoViewModel
    {
        public int Id { get; set; }
        public TipoLeiauteImportacao Leiaute { get; set; }
        public int Lote { get; set; }
        public Assessoria Assessoria { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataHoraArquivo { get; set; }
        public Usuario UsuarioImportacao { get; set; }
        public DateTime DataInsercao { get; set; }
        public bool Status { get; set; }

        public IntegracaoViewModel()
        {
            this.DataInsercao = DateTime.Now;
            this.UsuarioImportacao = new Usuario();
        }

        public IntegracaoViewModel(Integracao integracao)
        {
            this.Id = integracao?.Id ?? 0;
            this.Leiaute = integracao?.Leiaute ?? TipoLeiauteImportacao.ArquivoOcorrencia;
            this.Lote = integracao?.Lote ?? 0;
            this.Assessoria = integracao?.Assessoria ?? new Assessoria();
            this.NomeArquivo = integracao?.NomeArquivo;
            this.DataHoraArquivo = integracao?.DataHoraArquivo ?? DateTime.Now ;
            this.UsuarioImportacao = integracao?.UsuarioImportacao ?? new Usuario();
            this.DataInsercao = integracao?.DataInsercao ?? DateTime.Now;
            this.Status = integracao?.Status ?? true;
        }

        public Integracao ToEntity() => new Integracao()
        {
            Id = this.Id,
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Leiaute = this.Leiaute,
            Lote = this.Lote,
            Assessoria = this.Assessoria,
            NomeArquivo = this.NomeArquivo,
            DataHoraArquivo = this.DataHoraArquivo,
            UsuarioImportacao = this.UsuarioImportacao,
            Status = this.Status
        };
    }
}
