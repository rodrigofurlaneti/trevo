using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface IRetiradaCofreServico : IBaseServico<RetiradaCofre>
    {
        void SolicitarRetirada(IList<ContasAPagar> listaContaPagar, string observacoes, Departamento departamento, Usuario usuario);
        RetiradaCofre BuscarPelaNotificacaoId(int notificacaoId);
        void AtualizarStatus(List<RetiradaCofre> retiradasCofre, AcaoRetiradaCofre acao, Usuario usuario);
    }

    public class RetiradaCofreServico : BaseServico<RetiradaCofre, IRetiradaCofreRepositorio>, IRetiradaCofreServico
    {
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IContaPagarRepositorio _contaPagarRepositorio;
        private readonly IDepartamentoRepositorio _departamentoRepositorio;

        public RetiradaCofreServico(
            INotificacaoRepositorio notificacaoRepositorio
            , IUsuarioRepositorio usuarioRepositorio
            , IContaPagarRepositorio contaPagarRepositorio
            , IDepartamentoRepositorio departamentoRepositorio)
        {
            _notificacaoRepositorio = notificacaoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _contaPagarRepositorio = contaPagarRepositorio;
            _departamentoRepositorio = departamentoRepositorio;
        }

        private void AdicionarNotificacoes(List<RetiradaCofre> retiradas, Usuario usuario, List<Usuario> usuariosAprovadores)
        {
            foreach (var retirada in retiradas)
            {
                var itemRetirada = Repositorio.GetById(retirada.Id);
                var conta = itemRetirada.ContasAPagar;
                var urlPersonalizada = $"retiradacofre/index?retiradaCofreId={itemRetirada.Id}";
                var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(itemRetirada, Entidades.RetiradaCofre, usuario, null, "", urlPersonalizada, TipoAcaoNotificacao.Aviso, usuariosAprovadores);

                if (itemRetirada.RetiradaCofreNotificacoes == null)
                    itemRetirada.RetiradaCofreNotificacoes = new List<RetiradaCofreNotificacao>();

                itemRetirada.RetiradaCofreNotificacoes.Add(new RetiradaCofreNotificacao
                {
                    Notificacao = notificacao
                });
                
                Repositorio.Save(itemRetirada);
            }
        }

        private void GerarContaPagarNotificacoes(List<ContasAPagar> contasPagar, Usuario usuario, List<Usuario> usuariosAprovadores, AcaoRetiradaCofre acao)
        {
            foreach (var conta in contasPagar)
            {
                var descricao = acao == AcaoRetiradaCofre.Aprovar ?
                                        $"A retirada de cofre para a conta de Id: {conta.Id} foi aprovada" :
                                        $"A retirada de cofre para a conta de Id: {conta.Id} foi negada";

                var urlPersonalizada = acao == AcaoRetiradaCofre.Aprovar ?
                                       $"contapagar/indexpagar?contaPagarId={conta.Id}" :
                                       $"pagamentoespecie/index?contaPagarId={conta.Id}";
                var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(conta, Entidades.ContasAPagar, usuario, null, descricao, urlPersonalizada, TipoAcaoNotificacao.Aviso, usuariosAprovadores);

                var itemConta = _contaPagarRepositorio.GetById(conta.Id);

                itemConta.ContaPagarNotificacoes = itemConta.ContaPagarNotificacoes ?? new List<ContaPagarNotificacao>();
                itemConta.ContaPagarNotificacoes.Add(new ContaPagarNotificacao
                {
                    Notificacao = notificacao
                });

                _contaPagarRepositorio.Save(itemConta);
            }
        }

        public void SolicitarRetirada(IList<ContasAPagar> listaContaPagar, string observacoes, Departamento departamento, Usuario usuario)
        {
            var retiradas = new List<RetiradaCofre>();

            foreach (var contaAPagar in listaContaPagar)
            {
                contaAPagar.StatusConta = StatusContasAPagar.RetiradaCofre;

                retiradas.Add(new RetiradaCofre
                {
                    ContasAPagar = contaAPagar,
                    Usuario = usuario,
                    DataInsercao = DateTime.Now,
                    StatusRetiradaCofre = StatusRetiradaCofre.Pendente,
                    Observacoes = observacoes
                });
            }

            Repositorio.Save(retiradas);

            departamento = _departamentoRepositorio.GetById(departamento.Id);

            var responsaveisIds = departamento?.DepartamentoResponsaveis?.Select(x => x.Funcionario.Id)?.ToList() ?? new List<int>();
            var usuariosAprovadores = responsaveisIds.Any() ? _usuarioRepositorio.ListBy(x => responsaveisIds.Contains(x.Funcionario.Id))?.ToList() ?? new List<Usuario>() : new List<Usuario>();

            AdicionarNotificacoes(retiradas, usuario, usuariosAprovadores);
        }

        public RetiradaCofre BuscarPelaNotificacaoId(int notificacaoId)
        {
            return Repositorio.FirstBy(x => x.RetiradaCofreNotificacoes.Any(rn => rn.Notificacao.Id == notificacaoId));
        }

        public void AtualizarStatus(List<RetiradaCofre> retiradasCofre, AcaoRetiradaCofre acao, Usuario usuario)
        {
            var retiradasParaSalvar = new List<RetiradaCofre>();
            var notificacoesParaDeletar = new List<Notificacao>();

            foreach (var item in retiradasCofre)
            {
                var retiradaCofre = Repositorio.GetById(item.Id);
                var conta = retiradaCofre.ContasAPagar;

                switch (acao)
                {
                    case AcaoRetiradaCofre.Aprovar:
                        conta.StatusConta = StatusContasAPagar.PendentePagamento;
                        retiradaCofre.StatusRetiradaCofre = StatusRetiradaCofre.Retirado;
                        break;
                    case AcaoRetiradaCofre.Negar:
                        conta.StatusConta = conta.DataVencimento.Date < DateTime.Now.Date ? StatusContasAPagar.Vencida : StatusContasAPagar.EmAberto;
                        retiradaCofre.StatusRetiradaCofre = StatusRetiradaCofre.Cancelado;
                        break;
                    default:
                        break;
                }

                _contaPagarRepositorio.Save(conta);
                
                var notificacoes = retiradaCofre.RetiradaCofreNotificacoes.Select(x => x.Notificacao).ToList();
                notificacoesParaDeletar.AddRange(notificacoes);
                retiradaCofre.RetiradaCofreNotificacoes.Clear();

                Repositorio.Save(retiradaCofre);

                retiradasParaSalvar.Add(Repositorio.GetById(retiradaCofre.Id));
            }

            var contas = retiradasParaSalvar?.Select(x => x.ContasAPagar)?.ToList() ?? new List<ContasAPagar>();
            var responsaveisIds = retiradasParaSalvar?.SelectMany(x => x.ContasAPagar?.Departamento?.DepartamentoResponsaveis)?.Select(x => x.Funcionario.Id)?.ToList() ?? new List<int>();
            var usuariosAprovadores = responsaveisIds.Any() ? _usuarioRepositorio.ListBy(x => responsaveisIds.Contains(x.Funcionario.Id))?.ToList() ?? new List<Usuario>() : new List<Usuario>();

            GerarContaPagarNotificacoes(contas, usuario, usuariosAprovadores, acao);

            //Repositorio.Save(retiradasParaSalvar);

            foreach (var notificacao in notificacoesParaDeletar)
            {
                _notificacaoRepositorio.Delete(notificacao);
            }
        }
    }
}