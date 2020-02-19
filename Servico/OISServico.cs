using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface IOISServico : IBaseServico<OIS>
    {
        void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao);
        int BuscarIdPelaNotificacaoId(int notificacaoId);
        void GerarNotificacaoSeEmNegociacaoMaisDeMes();
    }

    public class OISServico : BaseServico<OIS, IOISRepositorio>, IOISServico
    {
        private readonly IImagemServico _imagemServico;

        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public OISServico(
            IImagemServico imagemServico,
            IContatoRepositorio contatoRepositorio
            , INotificacaoRepositorio notificacaoRepositorio
            )
        {
            _imagemServico = imagemServico;
            _contatoRepositorio = contatoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        private void AtualizarNotificacoes(OIS ois)
        {
            var notificacoes = Repositorio.GetById(ois.Id)?.OISNotificacoes?.Select(x => x.Notificacao).ToList() ?? new List<Notificacao>();

            ois.OISNotificacoes = new List<OISNotificacao>();

            if (ois.StatusSinistro == StatusSinistro.Recebido)
            {
                var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(ois, Entidades.OIS, ois.Usuario, null, "", "aberturaois/edit", TipoAcaoNotificacao.Aviso);

                var oisNotificacao = new OISNotificacao
                {
                    OIS = ois,
                    Notificacao = notificacao
                };

                ois.OISNotificacoes.Add(oisNotificacao);
            }
            Repositorio.Save(ois);

            foreach (var notificacao in notificacoes)
            {
                _notificacaoRepositorio.Delete(notificacao);
            }
        }

        private void AdicionarContatos(OIS ois)
        {
            if (ois.OISContatos != null && ois.OISContatos.Any())
            {
                var oisTelefones = ois.OISContatos.Where(x => x.Contato.Tipo == TipoContato.Residencial || x.Contato.Tipo == TipoContato.Celular).Select(x => x.Contato).ToList();

                foreach (var item in oisTelefones)
                {
                    var contatoExistente = _contatoRepositorio.FirstBy(x => x.Numero == item.Numero);
                    item.Id = contatoExistente != null ? contatoExistente.Id : item.Id;
                }

                var oisEmail = ois.OISContatos.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Email)?.Contato;
                if (oisEmail != null)
                {
                    var emailExistente = _contatoRepositorio.FirstBy(x => x.Email == oisEmail.Email);
                    oisEmail.Id = emailExistente != null ? emailExistente.Id : oisEmail.Id;
                }

                var contatosParaSalvar = ois.OISContatos.Where(x => x.Contato.Id <= 0).Select(x => x.Contato).ToList();

                if (contatosParaSalvar != null && contatosParaSalvar.Any())
                    _contatoRepositorio.Save(contatosParaSalvar);
            }
        }

        public void GerarNotificacaoSeEmNegociacaoMaisDeMes()
        {
            var descricao = "Está em negociação a mais de 1 mês.";
            var dataAtual = DateTime.Now.AddDays(-30);

            var listaOis = Repositorio.ListBy(x => x.StatusSinistro == StatusSinistro.EmNegociacao && 
                                                   x.DataAtualizacao.Date <= dataAtual.Date &&
                                                   !x.OISNotificacoes.Any(on => on.Notificacao.Descricao == descricao)).ToList();

            foreach (var ois in listaOis)
            {
                ois.OISNotificacoes = new List<OISNotificacao>();

                var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(ois, Entidades.OIS, ois.Usuario, null, descricao, 
                                    "aberturaois/edit", TipoAcaoNotificacao.Aviso);

                var oisNotificacao = new OISNotificacao
                {
                    OIS = ois,
                    Notificacao = notificacao
                };

                ois.OISNotificacoes.Add(oisNotificacao);
            }

            Repositorio.Save(listaOis);
        }

        public override void Salvar(OIS ois)
        {
            AdicionarContatos(ois);
            ois.StatusSinistro = ois.Id <= 0 ? StatusSinistro.Recebido : ois.StatusSinistro;

            foreach (var item in ois.OISImagens)
            {
                item.ImagemUpload = _imagemServico.GerarThumbnailEmBytes(item.ImagemUpload, 280, 150);
            }

            Repositorio.Save(ois);
            AtualizarNotificacoes(ois);

            ois.DataAtualizacao = DateTime.Now;
            base.Salvar(ois);
        }

        public void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao)
        {
            var ois = Repositorio.FirstBy(x => x.OISNotificacoes.Any(on => on.Notificacao.Id == notificacaoId));
            var oisNotificacoes = ois.OISNotificacoes.FirstOrDefault(x => x.Notificacao.Id == notificacaoId);

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    oisNotificacoes.Aprovar(usuario);
                    break;
                case AcaoNotificacao.Reprovado:
                    oisNotificacoes.Reprovar(usuario);
                    break;
                default:
                    break;
            }

            Repositorio.Save(ois);
        }

        public int BuscarIdPelaNotificacaoId(int notificacaoId)
        {
            var ois = Repositorio.FirstBy(x => x.OISNotificacoes.Any(on => on.Notificacao.Id == notificacaoId));

            return ois.Id;
        }
    }
}