using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class OcorrenciaFuncionarioDetalheViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string Justificativa { get; set; }
        public TipoOcorrenciaViewModel TipoOcorrencia { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public UsuarioViewModel UsuarioResponsavel { get; set; }

        public OcorrenciaFuncionarioDetalheViewModel(
            int id,
            DateTime dataOcorrencia, 
            string justificativa, 
            TipoOcorrenciaViewModel tipoOcorrencia,
            UnidadeViewModel unidade,
            UsuarioViewModel usuarioResponsavel
            )
        {
            this.Id = id;
            this.DataInsercao = DateTime.Now;
            this.DataOcorrencia = dataOcorrencia;
            this.Justificativa = justificativa;
            this.TipoOcorrencia = tipoOcorrencia;
            this.Unidade = unidade;
            this.UsuarioResponsavel = usuarioResponsavel;
        }
    }
}
