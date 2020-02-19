using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class UnidadeCondominoViewModel
    {
        public int Id { get; set; }
        public Unidade Unidade { get; set; }
        public int NumeroVagas { get; set; }

        public DateTime DataInsercao { get; set; }

        public IList<ClienteCondomino> ClienteCondomino { get; set; }

        public UnidadeCondominoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public UnidadeCondominoViewModel(UnidadeCondomino UnidadeCondomino)
        {
            Id = UnidadeCondomino.Id;
            Unidade = UnidadeCondomino.Unidade;
            NumeroVagas = UnidadeCondomino.NumeroVagas;
            DataInsercao = UnidadeCondomino.DataInsercao;
            ClienteCondomino = UnidadeCondomino.ClienteCondomino;
        }

        public UnidadeCondomino ToEntity()
        {
            var entidade = new UnidadeCondomino
            {
                Id = Id,
                Unidade = Unidade,
                NumeroVagas = NumeroVagas,
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
                ClienteCondomino = ClienteCondomino
            };

            return entidade;
        }
    }
}
