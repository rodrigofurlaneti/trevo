using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface ITipoLocacaoServico : IBaseServico<TipoLocacao>
    {
        List<TipoLocacao> BuscarParametrizados();
        List<TipoLocacao> BuscarParametrizadosPelaUnidadeId(int unidadeId);
    }

    public class TipoLocacaoServico : BaseServico<TipoLocacao, ITipoLocacaoRepositorio>, ITipoLocacaoServico
    {
        private readonly IParametrizacaoLocacaoRepositorio _parametrizacaoLocacaoRepositorio;

        public TipoLocacaoServico(IParametrizacaoLocacaoRepositorio parametrizacaoLocacaoRepositorio)
        {
            _parametrizacaoLocacaoRepositorio = parametrizacaoLocacaoRepositorio;
        }

        public List<TipoLocacao> BuscarParametrizados()
        {
            return _parametrizacaoLocacaoRepositorio.List().Select(x => x.TipoLocacao).DistinctBy(x => x.Id).ToList();
        }

        public List<TipoLocacao> BuscarParametrizadosPelaUnidadeId(int unidadeId)
        {
            return _parametrizacaoLocacaoRepositorio.ListBy(x => x.Unidade.Id == unidadeId).Select(x => x.TipoLocacao).DistinctBy(x => x.Id).ToList();
        }

        public override void Salvar(TipoLocacao tipoLocacao)
        {
            tipoLocacao.Validar();

            var tipoLocacaoParaSalvar = BuscarPorId(tipoLocacao.Id) ?? tipoLocacao;
            tipoLocacaoParaSalvar.Id = tipoLocacao.Id;
            tipoLocacaoParaSalvar.Descricao = tipoLocacao.Descricao;
            tipoLocacaoParaSalvar.DataInsercao = tipoLocacao.DataInsercao;

            base.Salvar(tipoLocacaoParaSalvar);
        }
    }
}