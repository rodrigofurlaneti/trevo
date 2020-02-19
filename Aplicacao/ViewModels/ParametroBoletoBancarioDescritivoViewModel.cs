using System;

namespace Aplicacao.ViewModels
{
    public class ParametroBoletoBancarioDescritivoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descritivo { get; set; }
        public UsuarioViewModel Usuario { get; set; }

        public ParametroBoletoBancarioDescritivoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ParametroBoletoBancarioDescritivoViewModel(string descritivo, UsuarioViewModel usuario)
        {
            Usuario = usuario;
            Descritivo = descritivo;
            DataInsercao = DateTime.Now;
        }
    }
}
