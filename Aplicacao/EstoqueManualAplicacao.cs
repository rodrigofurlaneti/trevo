using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;
using System.Linq;
using Aplicacao.ViewModels;
using Entidade.Uteis;
using AutoMapper;

namespace Aplicacao
{
    public interface IEstoqueManualAplicacao : IBaseAplicacao<EstoqueManual>
    {
        void Salvar(EstoqueManual estoqueManual, int usuarioId, int materialId);
        List<EstoqueManualItemViewModel> GerarItens(int quantidade, int estoqueId, int materialId, int? unidadeId);
    }

    public class EstoqueManualAplicacao : BaseAplicacao<EstoqueManual, IEstoqueManualServico>, IEstoqueManualAplicacao
    {
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IEstoqueAplicacao _estoqueAplicacao;
        private readonly IMaterialAplicacao _materialAplicacao;

        private readonly IEstoqueManualServico _estoqueManualServico;
        private readonly IUsuarioServico _usuarioServico;


        public EstoqueManualAplicacao(IUnidadeAplicacao unidadeAplicacao,
                                      IEstoqueAplicacao estoqueAplicacao,
                                      IMaterialAplicacao materialAplicacao,
                                      IEstoqueManualServico estoqueManualServico,
                                      IUsuarioServico usuarioServico)
        {
            _unidadeAplicacao = unidadeAplicacao;
            _estoqueAplicacao = estoqueAplicacao;
            _materialAplicacao = materialAplicacao;
            _estoqueManualServico = estoqueManualServico;
            _usuarioServico = usuarioServico;
        }

        public List<EstoqueManualItemViewModel> GerarItens(int quantidade, int estoqueId, int materialId, int? unidadeId)
        {
            var itensGerados = _estoqueManualServico.GerarItens(quantidade, estoqueId, materialId, unidadeId);
            return Mapper.Map<List<EstoqueManualItemViewModel>>(itensGerados);
        }

        public void Salvar(EstoqueManual estoqueManual, int usuarioId, int materialId)
        {
            var usuario = _usuarioServico.BuscarPorId(usuarioId);
            _estoqueManualServico.Salvar(estoqueManual, usuario, materialId);
        }
    }
}