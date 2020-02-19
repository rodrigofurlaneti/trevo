using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ParametroNegociacaoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public PerfilViewModel Perfil { get; set; }
        //public UsuarioViewModel Usuario { get; set; }
        public virtual IList<LimiteDescontoViewModel> LimitesDesconto { get; set; }

        public ParametroNegociacaoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ParametroNegociacaoViewModel(ParametroNegociacao parametroNegociacao)
        {
            Id = parametroNegociacao.Id;
            DataInsercao = parametroNegociacao.DataInsercao;
            Unidade = new UnidadeViewModel(parametroNegociacao?.Unidade ?? new Unidade());
            Perfil = new PerfilViewModel(parametroNegociacao?.Perfil ?? new Perfil());
            //Usuario = new UsuarioViewModel(parametroNegociacao?.Usuario ?? new Usuario());
            LimitesDesconto = parametroNegociacao.LimitesDesconto?.Select(x => new LimiteDescontoViewModel(x)).ToList() ?? new List<LimiteDescontoViewModel>();
        }

        public ParametroNegociacao ToEntity()
        {
            return new ParametroNegociacao
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Unidade = Unidade?.ToEntity(),
                Perfil = Perfil?.ToEntity(),
                LimitesDesconto = LimitesDesconto?.Select(x => x.ToEntity()).ToList() ?? new List<LimiteDesconto>()
            };
        }

        public ParametroNegociacaoViewModel ToViewModel(ParametroNegociacao parametroNegociacao)
        {
            return new ParametroNegociacaoViewModel
            {
                Id = parametroNegociacao.Id,
                DataInsercao = parametroNegociacao.DataInsercao,
                Unidade = new UnidadeViewModel(parametroNegociacao?.Unidade ?? new Unidade()),
                Perfil = new PerfilViewModel(parametroNegociacao?.Perfil ?? new Perfil()),
                //Usuario = new UsuarioViewModel(parametroNegociacao?.Usuario ?? new Usuario()),
                LimitesDesconto = parametroNegociacao.LimitesDesconto?.Select(x => new LimiteDescontoViewModel(x)).ToList() ?? new List<LimiteDescontoViewModel>()
            };
            
        }
    }
}
