using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Dominio;
using Entidade;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface IPedidoCompraAplicacao : IBaseAplicacao<PedidoCompra>
    {
        List<CotacaoViewModel> BuscarCotacoes();
        void Salvar(PedidoCompraViewModel pedidoCompraViewModel, int usuarioId);
    }

    public class PedidoCompraAplicacao : BaseAplicacao<PedidoCompra, IPedidoCompraServico>, IPedidoCompraAplicacao
    {
        private readonly IPedidoCompraServico _pedidoCompraServico;
        private readonly IUsuarioServico _usuarioServico;

        public PedidoCompraAplicacao(IPedidoCompraServico pedidoCompraServico, IUsuarioServico usuarioServico)
        {
            _pedidoCompraServico = pedidoCompraServico;
            _usuarioServico = usuarioServico;
        }

        public List<CotacaoViewModel> BuscarCotacoes()
        {
            return Mapper.Map<List<CotacaoViewModel>>(_pedidoCompraServico.BuscarCotacoes());
        }

        public void Salvar(PedidoCompraViewModel pedidoCompraViewModel, int usuarioId)
        {
            var usuario = _usuarioServico.BuscarPorId(usuarioId);
            var pedidoCompra = Mapper.Map<PedidoCompra>(pedidoCompraViewModel);

            _pedidoCompraServico.Salvar(pedidoCompra, usuario);
        }
    }
}