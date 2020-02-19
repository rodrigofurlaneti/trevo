using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class VagaCortesiaViewModel
    {
        public int Id { get; set; }
        public ClienteViewModel Cliente { get; set; }

        public string UnidadesLista { get; set; }

        public IList<VagaCortesiaVigenciaViewModel> VagaCortesiaVigencia { get; set; }

        public VagaCortesiaViewModel()
        {

        }

        public VagaCortesiaViewModel(VagaCortesia VagaCortesia)
        {
            Id = VagaCortesia.Id;
            Cliente = VagaCortesia.Cliente != null ? new ClienteViewModel(VagaCortesia.Cliente) : new ClienteViewModel();

            VagaCortesiaVigencia = VagaCortesia.VagaCortesiaVigencia.Select(x => new VagaCortesiaVigenciaViewModel(x)).ToList();

            VagaCortesia.VagaCortesiaVigencia.ToList().ForEach(x => {
                UnidadesLista = UnidadesLista + x.Unidade.Nome + ", ";
            });

            if(!string.IsNullOrEmpty(UnidadesLista) && UnidadesLista.Contains(','))
            {
                UnidadesLista = UnidadesLista.Remove(UnidadesLista.Length - 2, 1);
            }
        }

        public VagaCortesia ToEntity()
        {
            var entidade = new VagaCortesia
            {
                Id = Id,
                Cliente = Cliente.ToEntity(),
                VagaCortesiaVigencia = VagaCortesiaVigencia?.Select(x => x.ToEntity())?.ToList() ?? new List<VagaCortesiaVigencia>(),
            };

            return entidade;
        }
    }
}


