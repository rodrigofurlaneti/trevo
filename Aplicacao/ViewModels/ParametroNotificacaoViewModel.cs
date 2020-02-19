using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Aplicacao.ViewModels
{
    public class ParametroNotificacaoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public TipoNotificacaoViewModel TipoNotificacao { get; set; }
        public List<ParametroNotificacaoUsuario> Aprovadores { get; set; }

        public IList<int> IdAprovadores { get; set; }
        public List<UsuarioViewModel> Usuarios { get; set; }

        public ParametroNotificacaoViewModel()
        {
            DataInsercao = DateTime.Now;
            TipoNotificacao = new TipoNotificacaoViewModel();
            Aprovadores = new List<ParametroNotificacaoUsuario>();
            IdAprovadores = new List<int>();
            Usuarios = new List<UsuarioViewModel>();
        }

        public ParametroNotificacaoViewModel(ParametroNotificacao entity)
        {
            Id = entity.Id;
            TipoNotificacao = new TipoNotificacaoViewModel(entity?.TipoNotificacao);
            Aprovadores = entity?.Aprovadores?.Select(x => new ParametroNotificacaoUsuario { Usuario = new Usuario { Id = x?.Usuario?.Id ?? 0 } }).ToList();
            IdAprovadores = entity?.Aprovadores?.Select(x => x?.Usuario?.Id ?? 0).ToList();
            Usuarios = new List<UsuarioViewModel>();
        }

        public virtual ParametroNotificacao ToEntity() => new ParametroNotificacao
        {
            Id = this.Id,
            DataInsercao = DateTime.Now,
            TipoNotificacao = AutoMapper.Mapper.Map<TipoNotificacaoViewModel, TipoNotificacao>(this.TipoNotificacao),
            Aprovadores = this.Aprovadores?.Select(x => new ParametroNotificacaoUsuario {Usuario  = new Usuario { Id = x.Usuario.Id }, ParametroNotificacao = new ParametroNotificacao { Id = Id } }).ToList()
        };
    }
}


