using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Dominio
{
    public interface IEstoqueManualServico : IBaseServico<EstoqueManual>
    {
        void Salvar(EstoqueManual estoqueManual, Usuario usuario, int materialId);
        List<EstoqueManualItem> GerarItens(int quantidade, int estoqueId, int materialId, int? unidadeId);
        void CriarNotificacaoSeEstoqueBaixo(Material material, Usuario usuario);
    }

    public class EstoqueManualServico : BaseServico<EstoqueManual, IEstoqueManualRepositorio>, IEstoqueManualServico
    {
        private readonly IEstoqueManualRepositorio _estoqueManualRepositorio;
        private readonly IEstoqueManualItemRepositorio _estoqueManualItemRepositorio;
        private readonly IEstoqueRepositorio _estoqueRepositorio;
        private readonly IEstoqueMaterialRepositorio _estoqueMaterialRepositorio;
        private readonly IUnidadeRepositorio _unidadeRepositorio;
        private readonly IMaterialHistoricoRepositorio _materialHistoricoRepositorio;
        private readonly IPedidoCompraRepositorio _pedidoCompraRepositorio;
        private readonly INecessidadeCompraRepositorio _necessidadeCompraRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        private readonly IMaterialServico _materialServico;

        public EstoqueManualServico(IEstoqueManualRepositorio estoqueManualRepositorio,
                                    IEstoqueManualItemRepositorio estoqueManualItemRepositorio,
                                    IEstoqueRepositorio estoqueRepositorio,
                                    IEstoqueMaterialRepositorio estoqueMaterialRepositorio,
                                    IUnidadeRepositorio unidadeRepositorio,
                                    IMaterialHistoricoRepositorio materialHistoricoRepositorio,
                                    IMaterialServico materialServico,
                                    IPedidoCompraRepositorio pedidoCompraRepositorio,
                                    INecessidadeCompraRepositorio necessidadeCompraRepositorio,
                                    INotificacaoRepositorio notificacaoRepositorio)
        {
            _estoqueManualRepositorio = estoqueManualRepositorio;
            _estoqueManualItemRepositorio = estoqueManualItemRepositorio;
            _estoqueRepositorio = estoqueRepositorio;
            _estoqueMaterialRepositorio = estoqueMaterialRepositorio;
            _unidadeRepositorio = unidadeRepositorio;
            _materialHistoricoRepositorio = materialHistoricoRepositorio;
            _materialServico = materialServico;
            _pedidoCompraRepositorio = pedidoCompraRepositorio;
            _necessidadeCompraRepositorio = necessidadeCompraRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        private void EnviarEmailComPedido(Material material)
        {
            var email = _materialServico.BuscarPorId(material.Id).EmailDoFornecedorPersonalizado;
            var emailFrom = ConfigurationManager.AppSettings["EMAIL_FROM"];
            var body = $"Conforme a solicitação gostaria de pedir {material.QuantidadeParaPedidoAutomatico} quantidades do material {material.Nome}. <br />" +
                       $"Qualquer dúvida entre em contato, pelo telefone: 011 3106-1456";
            var dataAtual = DateTime.Now;

            try
            {
                Mail.SendMail(email, $"Pedido de {material.Nome}", body, emailFrom);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DarEntrada(EstoqueManual estoqueManual, EstoqueMaterial estoqueMaterial)
        {
            estoqueManual.Material.DarEntrada(estoqueManual.Quantidade);
            estoqueMaterial.DarEntrada(estoqueManual.Quantidade, estoqueManual.Preco);
        }

        private void DarSaida(EstoqueManual estoqueManual, EstoqueMaterial estoqueMaterial)
        {
            estoqueManual.ListEstoqueManualItem = estoqueManual.ListEstoqueManualItem.Where(x => x.Unidade != null && x.Unidade.Id > 0).ToList();

            var quantidadeSaida = estoqueManual.Material.EhUmAtivo ?
                             estoqueManual.ListEstoqueManualItem.Count() :
                             estoqueManual.Quantidade;

            estoqueManual.Material.DarSaida(quantidadeSaida);
            estoqueMaterial.DarSaida(quantidadeSaida);
        }

        private void DarComoInventariado(EstoqueManual estoqueManual, EstoqueMaterial estoqueMaterial)
        {
            estoqueManual.ListEstoqueManualItem = estoqueManual.ListEstoqueManualItem.Where(x => x.Inventariado).ToList();

            var quantidade = estoqueManual.Material.EhUmAtivo ?
                             estoqueManual.ListEstoqueManualItem.Count() :
                             estoqueManual.Quantidade;

            if (estoqueManual.Material.EhUmAtivo)
            {
                estoqueManual.Material.DarSaida(quantidade);
                estoqueMaterial.DarSaida(quantidade);
            }
            else
            {
                estoqueManual.Material.DefinirQuantidadeTotalEmEstoque(estoqueMaterial.Quantidade, quantidade);
                estoqueMaterial.DefinirQuantidadeEmEstoque(quantidade);
            }
        }

        public void Salvar(EstoqueManual estoqueManual, Usuario usuario, int materialId)
        {
            estoqueManual.Material = _materialServico.BuscarPorId(estoqueManual.Material.Id);

            var estoqueMaterial = _estoqueMaterialRepositorio
                                  .FirstBy(x => x.Material.Id == estoqueManual.Material.Id && x.Estoque.Id == estoqueManual.Estoque.Id) ??
                                  new EstoqueMaterial(estoqueManual.Estoque, estoqueManual.Material);

            switch (estoqueManual.Acao)
            {
                case AcaoEstoqueManual.Entrada:
                    DarEntrada(estoqueManual, estoqueMaterial);
                    this.MudarStatusDoPedido(estoqueManual.PedidoCompra, materialId);
                    break;
                case AcaoEstoqueManual.Saida:
                    DarSaida(estoqueManual, estoqueMaterial);
                    CriarNotificacaoSeEstoqueBaixo(estoqueMaterial.Material, usuario);
                    break;
                case AcaoEstoqueManual.Inventario:
                    DarComoInventariado(estoqueManual, estoqueMaterial);
                    break;
            }

			var materialHistorico = new MaterialHistorico(estoqueManual){Usuario = usuario};
            _materialServico.SalvarMaterialHistorico(materialHistorico);
            _estoqueMaterialRepositorio.Save(estoqueMaterial);
            base.Salvar(estoqueManual);
        }

        public void CriarNotificacaoSeEstoqueBaixo(Material material, Usuario usuario)
        {
            if (material.EstoqueEstaBaixo)
            {
                if (material.TemFornecedorPersonalizado)
                {
                    EnviarEmailComPedido(material);
                    var descricao = $"Foi feito o pedido do material {material.Nome} para o fornecedor personalizado";
                    var url = "Material/Edit";
                    var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(material, Entidades.Material, usuario, null, descricao, url, TipoAcaoNotificacao.Aviso);
                    _materialServico.SalvarMaterialNotificacao(material, notificacao);
                }
                else
                {
                    var notificacao = _materialServico.SalvarNotificacaoComRetorno(material, usuario);
                    _materialServico.SalvarMaterialNotificacao(material, notificacao);
                }
            }
        }

        public List<EstoqueManualItem> GerarItens(int quantidade, int estoqueId, int materialId, int? unidadeId)
        {
            var estoqueManualItens = new List<EstoqueManualItem>();

            var estoque = _estoqueRepositorio.GetById(estoqueId);
            var unidade = unidadeId.HasValue ? _unidadeRepositorio.GetById(unidadeId.Value) : null;
            var material = _materialServico.BuscarPorId(materialId);
            var quantidadeExistente = _estoqueManualItemRepositorio.Count();

            for (int i = 1; i <= quantidade; i++)
            {
                var estoqueManualItem = new EstoqueManualItem
                {
                    CodigoMaterial = quantidadeExistente + i,
                    NumeroMaterial = material.Id.ToString(),
                    Estoque = estoque,
                    Unidade = unidade,
                    Material = material
                };

                estoqueManualItens.Add(estoqueManualItem);
            }

            return estoqueManualItens;
        }

        private void MudarStatusDoPedido(PedidoCompra pedidoCompra, int materialId)
        {
            if (pedidoCompra != null)
            {
                pedidoCompra = _pedidoCompraRepositorio.GetById(pedidoCompra.Id);

                var cotacao = pedidoCompra.PedidoCompraMaterialFornecedores.FirstOrDefault().CotacaoMaterialFornecedor.Cotacao;
                var necessidadeCompra = _necessidadeCompraRepositorio.FirstBy(x => x.Cotacao.Id == cotacao.Id);
                if (pedidoCompra.Status == StatusPedidoCompra.AguardandoEntregaPedido)
                {
                    var materialFornecedores = pedidoCompra.PedidoCompraMaterialFornecedores.Where(x => x.CotacaoMaterialFornecedor.Material.Id == materialId);

                    foreach (var materialFornecedor in materialFornecedores)
                    {
                        materialFornecedor.EntregaRealizada = true;
                    }

                    _necessidadeCompraRepositorio.Save(necessidadeCompra);
                    _pedidoCompraRepositorio.Save(pedidoCompra);


                    if (pedidoCompra.PedidoCompraMaterialFornecedores.All(x => x.EntregaRealizada))
                    {
                        pedidoCompra.Status = StatusPedidoCompra.EntregaRealizada;
                        necessidadeCompra.StatusNecessidadeCompra = StatusNecessidadeCompra.EntregaRealizada;
                    }
                }
            }
        }
    }
}