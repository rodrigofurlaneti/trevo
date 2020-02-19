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
    public interface IPedidoCompraServico : IBaseServico<PedidoCompra>
    {
        void Salvar(PedidoCompra pedidoCompra, Usuario usuario);
        List<Cotacao> BuscarCotacoes();
        int BuscarIdPelaNotificaoId(int notificacaoId);
        void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao);
    }

    public class PedidoCompraServico : BaseServico<PedidoCompra, IPedidoCompraRepositorio>, IPedidoCompraServico
    {
        private readonly ICotacaoRepositorio _cotacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IPedidoCompraNotificacaoRepositorio _pedidoCompraNotificacaoRepositorio;
        private readonly IPedidoCompraCotacaoMaterialFornecedorRepositorio _pedidoCompraCotacaoMaterialFornecedorRepositorio;
        private readonly IContaPagarRepositorio _contaPagarRepositorio;
        private readonly INecessidadeCompraRepositorio _necessidadeCompraRepositorio;

        public PedidoCompraServico(
            ICotacaoRepositorio cotacaoRepositorio,
            INotificacaoRepositorio notificacaoRepositorio,
            IPedidoCompraNotificacaoRepositorio pedidoCompraNotificacaoRepositorio,
            IPedidoCompraCotacaoMaterialFornecedorRepositorio pedidoCompraCotacaoMaterialFornecedorRepositorio,
            IContaPagarRepositorio contaPagarRepositorio,
            INecessidadeCompraRepositorio necessidadeCompraRepositorio
            )
        {
            _cotacaoRepositorio = cotacaoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
            _pedidoCompraNotificacaoRepositorio = pedidoCompraNotificacaoRepositorio;
            _pedidoCompraCotacaoMaterialFornecedorRepositorio = pedidoCompraCotacaoMaterialFornecedorRepositorio;
            _contaPagarRepositorio = contaPagarRepositorio;
            _necessidadeCompraRepositorio = necessidadeCompraRepositorio;
        }

        public void Salvar(PedidoCompra pedidoCompra, Usuario usuario)
        {
            if(pedidoCompra.Id == 0)
            {
                pedidoCompra.Status = StatusPedidoCompra.AguardandoAprovacaoPedido;
                foreach (var item in pedidoCompra.PedidoCompraMaterialFornecedores)
                {
                    item.Id = 0;
                }
            }
            base.Salvar(pedidoCompra);
            var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(pedidoCompra, Entidades.PedidoCompra, usuario, null);
            SalvarPedidoCompraNotificacao(pedidoCompra, notificacao);
        }

        public override void ExcluirPorId(int id)
        {
            var pedidoCompra = Repositorio.GetById(id);
            var pedidoCompraNotificacao = _pedidoCompraNotificacaoRepositorio.FirstBy(x => x.PedidoCompra.Id == pedidoCompra.Id);
            _pedidoCompraNotificacaoRepositorio.Delete(pedidoCompraNotificacao);
            Repositorio.Delete(pedidoCompra);
        }

        public List<Cotacao> BuscarCotacoes()
        {
            return _cotacaoRepositorio.ListBy(x => x.Status == StatusCotacao.Aprovado).ToList();
        }

        public int BuscarIdPelaNotificaoId(int notificacaoId)
        {
            return _pedidoCompraNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == notificacaoId).PedidoCompra.Id;
        }

        public void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao)
        {
            var pedidoCompraNotificacao = _pedidoCompraNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == notificacaoId);
            var pedidoCompra = pedidoCompraNotificacao.PedidoCompra;
            var cotacao = pedidoCompra.PedidoCompraMaterialFornecedores.FirstOrDefault().CotacaoMaterialFornecedor.Cotacao;
            var necessidadeCompra = _necessidadeCompraRepositorio.FirstBy(x => x.Cotacao.Id == cotacao.Id);

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    pedidoCompraNotificacao.Aprovar(usuario);
                    pedidoCompra.Status = StatusPedidoCompra.AguardandoEntregaPedido;
                    necessidadeCompra.StatusNecessidadeCompra = StatusNecessidadeCompra.AguardandoEntregaPedido;
                    this.EnviarEmailComPedido(pedidoCompra);
                    var contasAPagar = this.SalvarContasAPagarComRetorno(pedidoCompra);
                    this.SalvarContasAPagarNotificacoes(contasAPagar, usuario, pedidoCompra.Id);
                    break;
                case AcaoNotificacao.Reprovado:
                    pedidoCompraNotificacao.Reprovar(usuario);
                    pedidoCompra.Status = StatusPedidoCompra.PedidoReprovado;
                    break;
                default:
                    break;
            }

            _pedidoCompraNotificacaoRepositorio.Save(pedidoCompraNotificacao);
        }

        private List<ContasAPagar> SalvarContasAPagarComRetorno(PedidoCompra pedidoCompra)
        {
            var contasAPagar = new List<ContasAPagar>();

            var pedidoCompraMaterialFornecedores = pedidoCompra.PedidoCompraMaterialFornecedores.Where(x => x.Selecionado).ToList();
            var cotacoesMaterialFornecedores = pedidoCompraMaterialFornecedores.Select(x => x.CotacaoMaterialFornecedor).ToList();
            var fornecedores = cotacoesMaterialFornecedores.Select(x => x.Fornecedor).Distinct().ToList();

            foreach (var fornecedor in fornecedores)
            {
                var cotacoes = cotacoesMaterialFornecedores.Where(x => x.Fornecedor.Id == fornecedor.Id).ToList();

                var valorTotal = cotacoes.Sum(x => x.ValorTotal);

                contasAPagar.Add(new ContasAPagar
                {
                    Unidade = pedidoCompra.Unidade,
                    Fornecedor = fornecedor,
                    ValorTotal = valorTotal,
                    FormaPagamento = (FormaPagamento)Enum.Parse(typeof(FormaPagamento), pedidoCompra.FormaPagamento.ToString()),
                    CodigoAgrupadorParcela = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TipoPagamento = TipoContaPagamento.Etc
                });
            }

            _contaPagarRepositorio.Save(contasAPagar);

            return contasAPagar;
        }

        private void SalvarContasAPagarNotificacoes(List<ContasAPagar> contasAPagar, Usuario usuario, int pedidoCompraId)
        {
            foreach (var conta in contasAPagar)
            {
                var descrição = $"Complete as informações da Conta a pagar de ID: {conta.Id} - Referente ao pedido de compra de ID: {pedidoCompraId}" +
                                $" para o Fornecedor {conta.Fornecedor.Nome}.";
                var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(conta, Entidades.ContasAPagar, usuario, null, descrição, "contapagar/finalizarcadastro", TipoAcaoNotificacao.Aviso);

                conta.ContaPagarNotificacoes = new List<ContaPagarNotificacao>();
                conta.ContaPagarNotificacoes.Add(new ContaPagarNotificacao
                {
                    Notificacao = notificacao
                });
            }

            _contaPagarRepositorio.Save(contasAPagar);
        }

        private void SalvarPedidoCompraNotificacao(PedidoCompra pedidoCompra, Notificacao notificacao)
        {
            var pedidoCompraNotificacao = new PedidoCompraNotificacao
            {
                PedidoCompra = pedidoCompra,
                Notificacao = notificacao
            };

            _pedidoCompraNotificacaoRepositorio.Save(pedidoCompraNotificacao);
        }

        private void EnviarEmailComPedido(PedidoCompra pedidoCompra)
        {
            var pedidoCompraMaterialFornecedores = pedidoCompra.PedidoCompraMaterialFornecedores.Where(x => x.Selecionado).ToList();
            var cotacoesMaterialFornecedores = pedidoCompraMaterialFornecedores.Select(x => x.CotacaoMaterialFornecedor).ToList();
            var fornecedores = cotacoesMaterialFornecedores.Select(x => x.Fornecedor).Distinct().ToList();
            var emailFrom = ConfigurationManager.AppSettings["EMAIL_FROM"];
            var dataAtual = DateTime.Now;

            try
            {
                foreach (var fornecedor in fornecedores)
                {
                    var email = fornecedor.ContatoEmail;
                    var cotacoes = cotacoesMaterialFornecedores.Where(x => x.Fornecedor.Id == fornecedor.Id).ToList();

                    var body = $"Conforme o pedido {pedidoCompra.Id} - gostaria de solicitar: <br />";

                    foreach (var cotacao in cotacoes)
                    {
                        body += $"{cotacao.Quantidade} unidades do material {cotacao.Material.Nome} a {cotacao.Valor.ToString("N2")}. <br />";
                    }

                    body += $"<br />Qualquer dúvida ou alterações entrar em contato pelo telefone: 011 3106-1456";

                    Mail.SendMail(email, $"Pedido de Compra", body, emailFrom);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}