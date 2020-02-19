using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IUnidadeAplicacao : IBaseAplicacao<Unidade>
    {
        IList<Unidade> ListaUnidade(int? idConvenio = null);
        List<UnidadeViewModel> ListarOrdenadas();
        List<Unidade> ListarOrdenadoSimplificado();
        IList<Unidade> BuscarPorConvenio(int idConvenio);
    }

    public class UnidadeAplicacao : BaseAplicacao<Unidade, IUnidadeServico>, IUnidadeAplicacao
    {
        public readonly IUnidadeServico _unidadeServico;
        public readonly IEstacionamentoSoftparkAplicacao _estacionamentoSoftparkAplicacao;

        public UnidadeAplicacao(IUnidadeServico unidadeServico, IEstacionamentoSoftparkAplicacao estacionamentoSoftparkAplicacao)
        {
            _unidadeServico = unidadeServico;
            _estacionamentoSoftparkAplicacao = estacionamentoSoftparkAplicacao;
        }

        public IList<Unidade> ListaUnidade(int? idConvenio = null)
        {
            if (idConvenio.HasValue && idConvenio.Value > 0)
                return BuscarPorConvenio(idConvenio.Value);

            return _unidadeServico.Buscar();
        }

        public IList<Unidade> BuscarPorConvenio(int idConvenio)
        {
            return _unidadeServico.BuscarPorConvenio(idConvenio);
        }

        public List<UnidadeViewModel> ListarOrdenadas()
        {
            var unidades = _unidadeServico.Buscar().OrderBy(x => x.Nome).ToList();
            return AutoMapper.Mapper.Map<List<UnidadeViewModel>>(unidades);
        }

        public List<Unidade> ListarOrdenadoSimplificado()
        {
            return _unidadeServico.ListarOrdenadoSimplificado()?.OrderBy(x => x.Nome)?.ToList();
        }

        public new void Salvar(Unidade unidade)
        {
            Servico.Salvar(unidade);

            unidade = _unidadeServico.BuscarPorId(unidade.Id);

            var estacionamento = new EstacionamentoSoftparkViewModel(unidade);
            _estacionamentoSoftparkAplicacao.Salvar(estacionamento);
        }

        public override void ExcluirPorId(int id)
        {
            base.ExcluirPorId(id);

            _estacionamentoSoftparkAplicacao.ExcluirPorId(id);
        }
    }
}