using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Dominio.IRepositorio.Base;
using Entidade;
using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface INecessidadeCompraServico : IBaseServico<NecessidadeCompra>
    {
        void Salvar(NecessidadeCompra necessidadeCompra, Usuario usuario);
        void SalvarNecessidadeCompraNotificacao(NecessidadeCompra necessidadeCompra, Notificacao notificacao);
        void SalvarCotacaoNotificacao(Cotacao cotacao, Notificacao notificacao);
        void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao);
        void SalvarCotacao(int necessidadeCompraId, List<CotacaoMaterialFornecedor> listaCotacaoMaterialFornecedores, Usuario usuario);
        void AtualizarStatusCotacao(int notificacaoId, Usuario usuario, AcaoNotificacao acao);
    }

    public class NecessidadeCompraServico : BaseServico<NecessidadeCompra, INecessidadeCompraRepositorio>, INecessidadeCompraServico
    {
        private readonly INecessidadeCompraRepositorio _necessidadeCompraRepositorio;
        private readonly INecessidadeCompraMaterialFornecedorRepositorio _materialFornecedorRepositorio;
        private readonly INecessidadeCompraNotificacaoRepositorio _necessidadeCompraNotificacaoRepositorio;
        private readonly ICotacaoNotificacaoRepositorio _cotacaoNotificacaoRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public NecessidadeCompraServico(
                INecessidadeCompraRepositorio necessidadeCompraRepositorio,
                INecessidadeCompraMaterialFornecedorRepositorio materialFornecedorRepositorio,
                INecessidadeCompraNotificacaoRepositorio necessidadeCompraNotificacaoRepositorio,
                ICotacaoNotificacaoRepositorio cotacaoNotificacaoRepositorio,
                ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                INotificacaoRepositorio notificacaoRepositorio
            )
        {
            _necessidadeCompraRepositorio = necessidadeCompraRepositorio;
            _materialFornecedorRepositorio = materialFornecedorRepositorio;
            _necessidadeCompraNotificacaoRepositorio = necessidadeCompraNotificacaoRepositorio;
            _cotacaoNotificacaoRepositorio = cotacaoNotificacaoRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        public void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao)
        {
            var necessidadeCompraNotificacao = _necessidadeCompraNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == notificacaoId);

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    necessidadeCompraNotificacao.Aprovar(usuario);
                    necessidadeCompraNotificacao.NecessidadeCompra.StatusNecessidadeCompra = StatusNecessidadeCompra.AguardandoCotacao;
                    break;
                case AcaoNotificacao.Reprovado:
                    necessidadeCompraNotificacao.Reprovar(usuario);
                    necessidadeCompraNotificacao.NecessidadeCompra.StatusNecessidadeCompra = StatusNecessidadeCompra.Reprovado;
                    break;
                default:
                    break;
            }

            _necessidadeCompraNotificacaoRepositorio.Save(necessidadeCompraNotificacao);
        }

        public void AtualizarStatusCotacao(int notificacaoId, Usuario usuario, AcaoNotificacao acao)
        {
            var cotacaoNotificacao = _cotacaoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == notificacaoId);
            var necessidadeCompra = _necessidadeCompraRepositorio.FirstBy(x => x.Cotacao.Id == cotacaoNotificacao.Cotacao.Id);

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    cotacaoNotificacao.Aprovar(usuario);
                    cotacaoNotificacao.Cotacao.Status = StatusCotacao.Aprovado;
                    necessidadeCompra.StatusNecessidadeCompra = StatusNecessidadeCompra.AguardandoPedido;
                    break;
                case AcaoNotificacao.Reprovado:
                    cotacaoNotificacao.Reprovar(usuario);
                    cotacaoNotificacao.Cotacao.Status = StatusCotacao.Reprovado;
                    necessidadeCompra.StatusNecessidadeCompra = StatusNecessidadeCompra.CotacaoReprovada;
                    break;
                default:
                    break;
            }

            _cotacaoNotificacaoRepositorio.Save(cotacaoNotificacao);
        }

        public override void ExcluirPorId(int id)
        {
            var notificacao = _necessidadeCompraNotificacaoRepositorio.FirstBy(x => x.NecessidadeCompra.Id == id);
            _necessidadeCompraNotificacaoRepositorio.Delete(notificacao);

            _necessidadeCompraRepositorio.DeleteById(id);
        }

        public void Salvar(NecessidadeCompra necessidadeCompra, Usuario usuario)
        {
            necessidadeCompra.AdicionarNecessidadeCompraAosMaterialFornecedores();
            _necessidadeCompraRepositorio.Save(necessidadeCompra);

            var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(necessidadeCompra, Entidades.NecessidadeCompra, usuario, necessidadeCompra.DataNotificacaoValidade);
            SalvarNecessidadeCompraNotificacao(necessidadeCompra, notificacao);
            DeletarMateriaisFornecedoresSemNecessidadeCompra();
        }

        public void SalvarCotacao(int necessidadeCompraId, List<CotacaoMaterialFornecedor> listaCotacaoMaterialFornecedores, Usuario usuario)
        {
            var necessidadeCompra = _necessidadeCompraRepositorio.GetById(necessidadeCompraId);

            necessidadeCompra.StatusNecessidadeCompra = StatusNecessidadeCompra.AguardandoAprovacaoCotacao;
            if(necessidadeCompra.Cotacao != null)
            {
                necessidadeCompra.Cotacao.MaterialFornecedores = listaCotacaoMaterialFornecedores;
                necessidadeCompra.Cotacao.Status = StatusCotacao.AguardandoAprovacao;
            }
            else
            {
                necessidadeCompra.Cotacao = new Cotacao
                {
                    DataInsercao = DateTime.Now,
                    MaterialFornecedores = listaCotacaoMaterialFornecedores,
                };
            }

            necessidadeCompra.Cotacao.AdicionarCotacaoAosMaterialFornecedores();
            _necessidadeCompraRepositorio.Save(necessidadeCompra);  

            var descricao = $"Cotação ID: {necessidadeCompra.Cotacao.Id}- Presente na Necessidade de compra com ID - {necessidadeCompra.Id}";
            var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(necessidadeCompra.Cotacao, Entidades.Cotacao, usuario, null, descricao, "NecessidadeCompra/Cotacao");
            SalvarCotacaoNotificacao(necessidadeCompra.Cotacao, notificacao);
        }

        public void SalvarCotacaoNotificacao(Cotacao cotacao, Notificacao notificacao)
        {
            var cotacaoNotificacao = new CotacaoNotificacao
            {
                Cotacao = cotacao,
                Notificacao = notificacao
            };

            _cotacaoNotificacaoRepositorio.Save(cotacaoNotificacao);
        }

        public void SalvarNecessidadeCompraNotificacao(NecessidadeCompra necessidadeCompra, Notificacao notificacao)
        {
            var necessidadeCompraNotificacao = new NecessidadeCompraNotificacao
            {
                NecessidadeCompra = necessidadeCompra,
                Notificacao = notificacao
            };

            _necessidadeCompraNotificacaoRepositorio.Save(necessidadeCompraNotificacao);
        }

        private void DeletarMateriaisFornecedoresSemNecessidadeCompra()
        {
            var materiaisFornecedores = _materialFornecedorRepositorio.ListBy(x => x.NecessidadeCompra == null);

            foreach (var item in materiaisFornecedores)
            {
                _materialFornecedorRepositorio.Delete(item);
            }
        }
    }
}