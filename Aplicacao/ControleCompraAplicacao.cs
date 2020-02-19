using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IControleCompraAplicacao : IBaseAplicacao<ControleCompra>
    {
        ControleCompraViewModel PrimeiroPor(int orcamentoCotacaoId, int pecaServicoId);
        void Salvar(ControleCompraViewModel controleCompraViewModel);
        List<OrcamentoSinistroCotacaoItemViewModel> BuscarCotacoes(DateTime? data, StatusCompraServico? statusCompraServico);
    }

    public class ControleCompraAplicacao : BaseAplicacao<ControleCompra, IControleCompraServico>, IControleCompraAplicacao
    {
        private readonly IControleCompraServico _controleCompraServico;
        private readonly IOrcamentoSinistroServico _orcamentoSinistroServico;
        private readonly IPecaServicoServico _pecaServicoServico;

        public ControleCompraAplicacao(
            IControleCompraServico controleCompraServico
            , IOrcamentoSinistroServico orcamentoSinistroServico
            , IPecaServicoServico pecaServicoServico
            )
        {
            _controleCompraServico = controleCompraServico;
            _orcamentoSinistroServico = orcamentoSinistroServico;
            _pecaServicoServico = pecaServicoServico;
        }

        public List<OrcamentoSinistroCotacaoItemViewModel> BuscarCotacoes(DateTime? data, StatusCompraServico? statusCompraServico)
        {
            var listaOrcamento = new List<OrcamentoSinistro>();

            if (data.HasValue && statusCompraServico.HasValue)
            {
                listaOrcamento = _orcamentoSinistroServico.BuscarPor(x => x.OrcamentoSinistroCotacao != null &&
                                                                      x.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens
                                                                        .Any(item => item.DataServico.Date <= data.Value.Date &&
                                                                            item.StatusCompraServico == statusCompraServico.Value)).ToList();
            }
            else if (data.HasValue)
            {
                listaOrcamento = _orcamentoSinistroServico.BuscarPor(x => x.OrcamentoSinistroCotacao != null &&
                                                                      x.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens
                                                                        .Any(item => item.DataServico.Date <= data.Value.Date)).ToList();
            }
            else if (statusCompraServico.HasValue)
            {
                listaOrcamento = _orcamentoSinistroServico.BuscarPor(x => x.OrcamentoSinistroCotacao != null &&
                                                                      x.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens
                                                                        .Any(item => item.StatusCompraServico == statusCompraServico.Value)).ToList();
            }
            else
            {
                listaOrcamento = _orcamentoSinistroServico.BuscarPor(x => x.OrcamentoSinistroCotacao != null &&
                                                                      x.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens.Any()).ToList();
            }

            var cotacoes = listaOrcamento.Select(x => x.OrcamentoSinistroCotacao).ToList();

            var itens = new List<OrcamentoSinistroCotacaoItem>();
            foreach (var cotacao in cotacoes)
            {
                foreach (var item in cotacao.OrcamentoSinistroCotacaoItens)
                {
                    item.OrcamentoSinistroCotacao = cotacao;
                    itens.Add(item);
                }
            }

            itens = itens.Where(item => (!data.HasValue || item.DataServico.Date <= data.Value.Date) &&
                                        (statusCompraServico == null || item.StatusCompraServico == statusCompraServico.Value)).ToList();

            return Mapper.Map<List<OrcamentoSinistroCotacaoItemViewModel>>(itens);
        }

        public ControleCompraViewModel PrimeiroPor(int orcamentoCotacaoId, int pecaServicoId)
        {
            var controleCompra = _controleCompraServico.PrimeiroPor(x => x.OrcamentoSinistroCotacao.Id == orcamentoCotacaoId && x.PecaServico.Id == pecaServicoId);
            var orcamento = _orcamentoSinistroServico.PrimeiroPor(x => x.OrcamentoSinistroCotacao != null && x.OrcamentoSinistroCotacao.Id == orcamentoCotacaoId);
            var item = orcamento.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens.FirstOrDefault(x => x.PecaServico.Id == pecaServicoId);

            var controleCompraViewModel = new ControleCompraViewModel
            {
                Id = controleCompra?.Id ?? 0,
                DataInsercao = DateTime.Now,
                OrcamentoSinistroCotacao = Mapper.Map<OrcamentoSinistroCotacaoViewModel>(orcamento.OrcamentoSinistroCotacao),
                DataServico = item.DataServico,
                PecaServico = Mapper.Map<PecaServicoViewModel>(item.PecaServico),
                StatusCompraServico = item.StatusCompraServico,
                Observacao = controleCompra?.Observacao ?? string.Empty
            };

            return controleCompraViewModel;
        }

        public void Salvar(ControleCompraViewModel controleCompraViewModel)
        {
            var pecaServico = _pecaServicoServico.BuscarPorId(controleCompraViewModel.PecaServico.Id);
            var orcamentoSinistro = _orcamentoSinistroServico.PrimeiroPor(x => x.OrcamentoSinistroCotacao != null && 
                                                                             x.OrcamentoSinistroCotacao.Id == controleCompraViewModel.OrcamentoSinistroCotacao.Id);

            var controleCompra = Mapper.Map<ControleCompra>(controleCompraViewModel);

            controleCompra.PecaServico = pecaServico;
            controleCompra.OrcamentoSinistroCotacao = orcamentoSinistro.OrcamentoSinistroCotacao;

            _controleCompraServico.DeletarNotificacoes(controleCompra);
            _controleCompraServico.Salvar(controleCompra, controleCompraViewModel.NovaData.Value, controleCompraViewModel.StatusCompraServico);
        }
    }
}