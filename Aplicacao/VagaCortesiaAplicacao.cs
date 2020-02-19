using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IVagaCortesiaAplicacao : IBaseAplicacao<VagaCortesia>
    {
        void Salvar(VagaCortesiaViewModel model);
    }
    public class VagaCortesiaAplicacao : BaseAplicacao<VagaCortesia, IVagaCortesiaServico>, IVagaCortesiaAplicacao
    {
        private readonly IVagaCortesiaServico _VagaCortesiaServico;
        private readonly IVagaCortesiaVigenciaServico _vagaCortesiaVigenciaServico;
        private readonly IClienteAplicacao _clienteAplicacao;

        public VagaCortesiaAplicacao(IVagaCortesiaServico VagaCortesiaServico,
                                     IClienteAplicacao clienteAplicacao,
                                     IVagaCortesiaVigenciaServico vagaCortesiaVigenciaServico)
        {
            _VagaCortesiaServico = VagaCortesiaServico;
            _clienteAplicacao = clienteAplicacao;
            _vagaCortesiaVigenciaServico = vagaCortesiaVigenciaServico;
        }

        public void Salvar(VagaCortesiaViewModel model)
        {

            var entity = BuscarPorId(model.Id) ?? model.ToEntity();

            entity.Cliente = _clienteAplicacao.BuscarPorId(model.Cliente.Id);

            var listaDocumentosRemover = new List<int>();
            listaDocumentosRemover = entity.VagaCortesiaVigencia?.Where(x => !model?.VagaCortesiaVigencia?.Any(p => p.Id == x.Id) ?? false)?.Select(x => x.Id)?.ToList() ?? new List<int>();

            entity.VagaCortesiaVigencia = model.VagaCortesiaVigencia.Select(x => x.ToEntity()).ToList() ?? new List<VagaCortesiaVigencia>();

            foreach (var item in entity.VagaCortesiaVigencia)
            {
                item.VagaCortesia = entity;
            }

            Salvar(entity);

            listaDocumentosRemover.ForEach(x => _vagaCortesiaVigenciaServico.ExcluirPorId(x));

        }
    }
}
