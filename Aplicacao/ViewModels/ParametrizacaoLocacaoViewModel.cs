using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class ParametrizacaoLocacaoViewModel
    {
        public int Id { get; set; }
        public TipoLocacaoViewModel TipoLocacao { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public DateTime DataInsercao { get; set; }

        //public IList<UnidadeViewModel> ListaUnidades { get; set; }

        private List<int> ListaIds { get; set; }
        public List<int> ListaUnidades { get; set; }

        public int MyProperty { get; set; }
        public ParametrizacaoLocacaoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ParametrizacaoLocacaoViewModel(ParametrizacaoLocacao ParametrizacaoLocacao)
        {
            Id = ParametrizacaoLocacao.Id;
            TipoLocacao = new TipoLocacaoViewModel(ParametrizacaoLocacao.TipoLocacao);
            Unidade = new UnidadeViewModel(ParametrizacaoLocacao.Unidade);
            DataInsercao = ParametrizacaoLocacao.DataInsercao;
        }

        public ParametrizacaoLocacao ToEntity()
        {
            var entidade = new ParametrizacaoLocacao
            {
                Id = Id,
                TipoLocacao = TipoLocacao.ToEntity(),
                Unidade = Unidade.ToEntity(),
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            };

            return entidade;
        }
    }
}

