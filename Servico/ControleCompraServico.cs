using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface IControleCompraServico : IBaseServico<ControleCompra>
    {
        void Salvar(ControleCompra controleCompra, DateTime novaData, StatusCompraServico statusCompraServico);
        void DeletarNotificacoes(ControleCompra controleCompra);
    }

    public class ControleCompraServico : BaseServico<ControleCompra, IControleCompraRepositorio>, IControleCompraServico
    {
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public ControleCompraServico(INotificacaoRepositorio notificacaoRepositorio)
        {
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        public void Salvar(ControleCompra controleCompra, DateTime novaData, StatusCompraServico statusCompraServico)
        {
            var item = controleCompra.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens.FirstOrDefault(x => x.PecaServico.Id == controleCompra.PecaServico.Id);
            if (item.OrcamentoSinistroCotacaoHistoricoDataItens == null)
                item.OrcamentoSinistroCotacaoHistoricoDataItens = new List<OrcamentoSinistroCotacaoHistoricoDataItem>();

            item.OrcamentoSinistroCotacaoHistoricoDataItens.Add(new OrcamentoSinistroCotacaoHistoricoDataItem
            {
                OrcamentoSinistroCotacaoItem = item,
                Data = item.DataServico
            });

            item.DataServico = novaData;
            item.StatusCompraServico = statusCompraServico;

            base.Salvar(controleCompra);
        }

        public void DeletarNotificacoes(ControleCompra controleCompra)
        {
            var orcamentoSinistroContacao = controleCompra.OrcamentoSinistroCotacao;
            var notificacoes = orcamentoSinistroContacao.OrcamentoSinistroCotacaoNotificacoes
                                                       .Select(x => x.Notificacao)
                                                       .Where(x => x.Descricao.Contains("A data limite"))
                                                       .ToList();

            orcamentoSinistroContacao.OrcamentoSinistroCotacaoNotificacoes = orcamentoSinistroContacao.OrcamentoSinistroCotacaoNotificacoes
                                                                                                      .Where(x => !x.Notificacao.Descricao.Contains("A data limite"))
                                                                                                      .ToList();

            Repositorio.Save(controleCompra);

            if (notificacoes != null)
            {
                foreach (var notificacao in notificacoes)
                {
                    _notificacaoRepositorio.Delete(notificacao);
                }
            }
        }
    }
}