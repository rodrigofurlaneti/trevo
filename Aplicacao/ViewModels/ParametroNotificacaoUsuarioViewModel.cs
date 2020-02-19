using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class ParametroNotificacaoUsuarioViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ParametroNotificacaoViewModel ParametroNotificacao { get; set; }
        public UsuarioViewModel Usuario { get; set; }

        public ParametroNotificacaoUsuarioViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ParametroNotificacaoUsuarioViewModel(ParametroNotificacaoUsuario entity)
        {
            Id = entity?.Id ?? 0;
            DataInsercao = entity?.DataInsercao ?? DateTime.Now;
            ParametroNotificacao = new ParametroNotificacaoViewModel(entity?.ParametroNotificacao);
            Usuario = new UsuarioViewModel(entity?.Usuario);
        }
    }
}