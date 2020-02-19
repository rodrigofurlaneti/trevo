using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class TipoMensalistaViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInsercao { get; set; }

        public TipoMensalistaViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public TipoMensalistaViewModel(TipoMensalista tipoMensalista)
        {
            Id = tipoMensalista.Id;
            Descricao = tipoMensalista.Descricao;
            DataInsercao = tipoMensalista.DataInsercao;
        }

        public TipoMensalista ToEntity()
        {
            var tipoMensalista = new TipoMensalista()
            {
                Id = Id,
                Descricao = Descricao,
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao
            };
            return tipoMensalista;
        }
    }
}