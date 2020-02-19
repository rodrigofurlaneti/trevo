using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class LojaContatoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ContatoViewModel Contato { get; set; }
        
        public LojaContatoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public LojaContatoViewModel(EmpresaContato lojaContato)
        {
            Id = lojaContato?.Id ?? 0;
            DataInsercao = lojaContato?.DataInsercao ?? DateTime.Now;
            Contato = new ContatoViewModel(lojaContato?.Contato);
        }
    }
}