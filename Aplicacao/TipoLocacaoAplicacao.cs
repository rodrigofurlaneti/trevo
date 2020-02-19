using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;
using System.Linq;
using Aplicacao.ViewModels;
using Entidade.Uteis;
using AutoMapper;
using System.Web.Mvc;

namespace Aplicacao
{
    public interface ITipoLocacaoAplicacao : IBaseAplicacao<TipoLocacao>
    {
        List<TipoLocacaoViewModel> ListarOrdenado();
        List<TipoLocacaoViewModel> BuscarParametrizados();
        List<TipoLocacaoViewModel> BuscarParametrizadosPelaUnidadeId(int unidadeId);
    }

    public class TipoLocacaoAplicacao : BaseAplicacao<TipoLocacao, ITipoLocacaoServico>, ITipoLocacaoAplicacao
    {
        private readonly ITipoLocacaoServico _tipoLocacaoServico;

        public TipoLocacaoAplicacao(ITipoLocacaoServico tipoLocacaoServico)
        {
            _tipoLocacaoServico = tipoLocacaoServico;
        }

        public List<TipoLocacaoViewModel> BuscarParametrizados()
        {
            var tipoLocacoes = _tipoLocacaoServico.BuscarParametrizados();
            return Mapper.Map<List<TipoLocacaoViewModel>>(tipoLocacoes);
        }

        public List<TipoLocacaoViewModel> BuscarParametrizadosPelaUnidadeId(int unidadeId)
        {
            var tipoLocacoes = _tipoLocacaoServico.BuscarParametrizadosPelaUnidadeId(unidadeId);
            return Mapper.Map<List<TipoLocacaoViewModel>>(tipoLocacoes);
        }

        public List<TipoLocacaoViewModel> ListarOrdenado()
        {
            return Mapper.Map<List<TipoLocacaoViewModel>>(Servico.Buscar().OrderBy(x => x.Descricao).ToList());
        }
    }
}