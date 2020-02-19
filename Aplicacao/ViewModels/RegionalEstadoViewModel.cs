using System;

namespace Aplicacao.ViewModels
{
    public class RegionalEstadoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public RegionalViewModel Regional { get; set; }
        public EstadoViewModel Estado { get; set; }
    }
}
