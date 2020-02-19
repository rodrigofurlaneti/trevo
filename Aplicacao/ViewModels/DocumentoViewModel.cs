using Entidade;
using System;
using Entidade.Uteis;
using Core.Extensions;

namespace Aplicacao.ViewModels
{
    public class DocumentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public TipoDocumento Tipo { get; set; }
        public string Numero { get; set; }
        public string OrgaoExpedidor { get; set; }
        public DateTime? DataExpedicao { get; set; }

        public DocumentoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public DocumentoViewModel(Documento documento)
        {
            Id = documento?.Id ?? 0;
            DataInsercao = documento?.DataInsercao ?? DateTime.Now;
            Tipo = documento?.Tipo ?? TipoDocumento.Cpf;
            Numero = string.IsNullOrEmpty(documento?.Numero) ? string.Empty : documento?.Numero.ExtractLettersAndNumbers();
            OrgaoExpedidor = documento?.OrgaoExpedidor;
            DataExpedicao = documento?.DataExpedicao;
        }

        public Documento ToEntity() => new Documento
        (
            Tipo,
            Numero.ExtractLettersAndNumbers(),
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Id = Id,
            OrgaoExpedidor,
            null,
            DataExpedicao
        );
    }
}