using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class UnidadeCheckListAtividadeTipoAtividadeViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public TipoAtividadeViewModel TipoAtividade { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public bool Selecionado { get; set; }

        public UnidadeCheckListAtividadeTipoAtividadeViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}