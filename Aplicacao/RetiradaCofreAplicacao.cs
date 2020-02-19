using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IRetiradaCofreAplicacao : IBaseAplicacao<RetiradaCofre>
    {
        void SolicitarRetirada(List<PagamentoEspecieViewModel> pagamentoEspecie, string observacoes, int usuarioId);
        void AtualizarStatus(List<RetiradaCofreViewModel> retiradasCofreViewModel, AcaoRetiradaCofre acao, int usuarioId);
    }

    public class RetiradaCofreAplicacao : BaseAplicacao<RetiradaCofre, IRetiradaCofreServico>, IRetiradaCofreAplicacao
    {
        private readonly IRetiradaCofreServico _retiradaCofreServico;
        private readonly IContaPagarServico _contaPagarServico;
        private readonly IDepartamentoServico _departamentoServico;
        private readonly IUsuarioServico _usuarioServico;

        public RetiradaCofreAplicacao(
            IRetiradaCofreServico retiradaCofreServico
            , IContaPagarServico contaPagarServico
            , IDepartamentoServico departamentoServico
            , IUsuarioServico usuarioServico
            )
        {
            _retiradaCofreServico = retiradaCofreServico;
            _contaPagarServico = contaPagarServico;
            _departamentoServico = departamentoServico;
            _usuarioServico = usuarioServico;
        }

        public void AtualizarStatus(List<RetiradaCofreViewModel> retiradasCofreViewModel, AcaoRetiradaCofre acao, int usuarioId)
        {
            var usuario = _usuarioServico.BuscarPorId(usuarioId);
            var retiradasCofre = Mapper.Map<List<RetiradaCofre>>(retiradasCofreViewModel);
            _retiradaCofreServico.AtualizarStatus(retiradasCofre, acao, usuario);
        }

        public void SolicitarRetirada(List<PagamentoEspecieViewModel> pagamentoEspecie, string observacoes, int usuarioId)
        {
            var usuario = _usuarioServico.BuscarPorId(usuarioId);
            var departamentoIds = pagamentoEspecie.Select(x => x.DepartamentoId).DistinctBy(g => g);
            var departamentos = _departamentoServico.BuscarPor(x => departamentoIds.Contains(x.Id));

            foreach (var departamento in departamentos)
            {
                var contasIdsDoDepartamento = pagamentoEspecie.Where(x => x.DepartamentoId == departamento.Id).Select(x => x.ContasAPagarId);
                var contasAPagar = _contaPagarServico.BuscarPor(x => contasIdsDoDepartamento.Contains(x.Id));

                _retiradaCofreServico.SolicitarRetirada(contasAPagar, observacoes, departamento, usuario);
            }
        }
    }
}