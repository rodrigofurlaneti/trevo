using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class BeneficioFuncionarioDetalheViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public TipoBeneficioViewModel TipoBeneficio { get; set; }
        public string Valor { get; set; }

        public BeneficioFuncionarioDetalheViewModel(TipoBeneficioViewModel tipoBeneficio, string valor)
        {
            DataInsercao = DateTime.Now;
            this.TipoBeneficio = tipoBeneficio;
            this.Valor = valor;
        }
    }
}
