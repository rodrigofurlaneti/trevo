using System.Collections.Generic;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IOrcamentoSinistroAplicacao : IBaseAplicacao<OrcamentoSinistro>
    {
        void Salvar(OrcamentoSinistro orcamentoSinistro, int usuarioId);
        void SalvarCotacao(int orcamentoSinistroId, List<OrcamentoSinistroCotacaoItemViewModel> listaOrcamentoSinistroCotacaoItem, int usuarioId);
        void Cancelar(int id);
    }

    public class OrcamentoSinistroAplicacao : BaseAplicacao<OrcamentoSinistro, IOrcamentoSinistroServico>, IOrcamentoSinistroAplicacao
    {
        private readonly IOrcamentoSinistroServico _orcamentoSinistroServico;
        private readonly IUsuarioServico _usuarioServico;

        public OrcamentoSinistroAplicacao(
            IOrcamentoSinistroServico orcamentoSinistroServico
            , IUsuarioServico usuarioServico
            )
        {
            _orcamentoSinistroServico = orcamentoSinistroServico;
            _usuarioServico = usuarioServico;
        }

        public void Cancelar(int id)
        {
            _orcamentoSinistroServico.Cancelar(id);
        }

        public void Salvar(OrcamentoSinistro orcamentoSinistro, int usuarioId)
        {
            var usuario = _usuarioServico.BuscarPorId(usuarioId);
            _orcamentoSinistroServico.Salvar(orcamentoSinistro, usuario);
        }

        public void SalvarCotacao(int orcamentoSinistroId, List<OrcamentoSinistroCotacaoItemViewModel> listaOrcamentoSinistroCotacaoItem, int usuarioId)
        {
            var usuario = _usuarioServico.BuscarPorId(usuarioId);
            var orcamentoSinistro = _orcamentoSinistroServico.BuscarPorId(orcamentoSinistroId);
            var orcamentoSinistroCotacaoItens = Mapper.Map<List<OrcamentoSinistroCotacaoItem>>(listaOrcamentoSinistroCotacaoItem);
            _orcamentoSinistroServico.SalvarCotacao(orcamentoSinistro, orcamentoSinistroCotacaoItens, usuario);
        }
    }
}