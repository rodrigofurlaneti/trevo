using System.Collections.Generic;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface INecessidadeCompraAplicacao : IBaseAplicacao<NecessidadeCompra>
    {
        void Salvar(NecessidadeCompraViewModel necessidadeCompraViewModel, int usuarioId);
        void SalvarCotacao(int necessidadeCompraId, List<CotacaoMaterialFornecedorViewModel> listaCotacaoMaterialFornecedoresViewModel, int usuarioId);
    }

    public class NecessidadeCompraAplicacao : BaseAplicacao<NecessidadeCompra, INecessidadeCompraServico>, INecessidadeCompraAplicacao
    {
        private readonly INecessidadeCompraServico _necessidadeCompraServico;
        private readonly IUsuarioServico _usuarioServico;

        public NecessidadeCompraAplicacao(
            INecessidadeCompraServico necessidadeCompraServico,
            IUsuarioServico usuarioServico
            )
        {
            _necessidadeCompraServico = necessidadeCompraServico;
            _usuarioServico = usuarioServico;
        }

        public void Salvar(NecessidadeCompraViewModel necessidadeCompraViewModel, int usuarioId)
        {
            var usuario = _usuarioServico.BuscarPorId(usuarioId);
            var necessidadeCompra = Mapper.Map<NecessidadeCompra>(necessidadeCompraViewModel);
            _necessidadeCompraServico.Salvar(necessidadeCompra, usuario);
        }

        public void SalvarCotacao(int necessidadeCompraId, List<CotacaoMaterialFornecedorViewModel> listaCotacaoMaterialFornecedoresViewModel, int usuarioId)
        {
            var usuario = _usuarioServico.BuscarPorId(usuarioId);
            var listaCotacaoMaterialFornecedores = Mapper.Map<List<CotacaoMaterialFornecedor>>(listaCotacaoMaterialFornecedoresViewModel);

            _necessidadeCompraServico.SalvarCotacao(necessidadeCompraId, listaCotacaoMaterialFornecedores, usuario);
        }
    }
}