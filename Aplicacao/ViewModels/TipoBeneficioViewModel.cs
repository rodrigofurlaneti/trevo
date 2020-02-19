using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class TipoBeneficioViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public TipoBeneficioViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}
