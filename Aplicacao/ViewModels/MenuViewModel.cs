using Entidade;
using System.Collections.Generic;
using System;

namespace Aplicacao.ViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public MenuPaiViewModel MenuPai { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public int Posicao { get; set; }
        public string Url { get; set; }
        public bool Ativo { get; set; }

        public MenuViewModel() 
        { 

        }

        public MenuViewModel(Menu menu)
        {
            this.Id = menu?.Id ?? 0;
            this.DataInsercao = menu?.DataInsercao ?? DateTime.Now;
            this.MenuPai = menu?.MenuPai != null ? new MenuPaiViewModel(menu.MenuPai) : null;
            this.Ativo = menu?.Ativo ?? false;
            this.Descricao = menu?.Descricao;
            this.Posicao = menu?.Posicao ?? 0;
            this.Url = menu?.Url;
        }

        public List<MenuViewModel> MenuViewModelList(List<Menu> menus)
        {
            var listaMenus = new List<MenuViewModel>();
            
            foreach (var menu in menus)
                listaMenus.Add(new MenuViewModel(menu));
                
            return listaMenus;
        }

        public Menu ToEntity() => new Menu
        {
            Id = this.Id,
            MenuPai = this.MenuPai.ToEntity(),
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Ativo = this.Ativo,
            Descricao = this.Descricao,
            Posicao = this.Posicao,
            Url = this.Url
        };
    }

    public class MenuPaiViewModel
    {
        public int Id { get; set; }
        public MenuViewModel MenuPai { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInsercao { get; set; }
        public int Posicao { get; set; }
        public string Url { get; set; }
        public bool Ativo { get; set; }

        public MenuPaiViewModel() { }

        public MenuPaiViewModel(Menu menu)
        {
            this.Id = menu?.Id ?? 0;
            this.Ativo = menu?.Ativo ?? false;
            this.DataInsercao = menu?.DataInsercao ?? DateTime.Now;
            this.Descricao = menu?.Descricao;
            this.Posicao = menu?.Posicao ?? 0;
            this.Url = menu?.Url;
        }

        public Menu ToEntity() => new Menu
        {
            Id = this.Id,
            MenuPai = this.MenuPai.ToEntity(),
            Ativo = this.Ativo,
            Descricao = this.Descricao,
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Posicao = this.Posicao,
            Url = this.Url
        };
    }
}