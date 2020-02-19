using Entidade;
using System;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class PessoaDocumentoViewModel
    {
        public DocumentoViewModel Documento{get; set;}

        public PessoaDocumentoViewModel()
        {
            Documento = new DocumentoViewModel();
        }

        public PessoaDocumentoViewModel(PessoaDocumento entity)
        {
            Documento = new DocumentoViewModel(entity.Documento);
        }

        public PessoaDocumento ToEntity() => new PessoaDocumento
        {
            DataInsercao = DateTime.Now,
            Documento = this.Documento.ToEntity()
        };
    }
}