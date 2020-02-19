using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface ITabelaPrecoAvulsoServico : IBaseServico<TabelaPrecoAvulso>
    {
        List<TabelaPrecoAvulsoPeriodo> CarregarPeriodosDaTabela(int id);
        List<TabelaPrecoAvulsoHoraValor> CarregarHoraValorDaTabela(int id);
        List<TabelaPrecoAvulsoUnidade> CarregarUnidadesDaTabela(int id);
        void Salvar(TabelaPrecoAvulso tabelaPrecoAvulso, int idUsuario);
        void Excluir(int id);
        void Excluir(int id, int idUnidade, int idUsuario);
    }

    public class TabelaPrecoAvulsoServico : BaseServico<TabelaPrecoAvulso, ITabelaPrecoAvulsoRepositorio>, ITabelaPrecoAvulsoServico
    {
        private readonly ITabelaPrecoAvulsoRepositorio _tabelaPrecoAvulsoRepositorio;
        private readonly ITabelaPrecoAvulsoNotificacaoServico _tabelaPrecoAvulsoNotificacaoServico;
        private readonly ITabelaPrecoMensalistaUnidadeRepositorio _tabelaPrecoMensalistaUnidadeRepositorio;

        public TabelaPrecoAvulsoServico(
            ITabelaPrecoAvulsoRepositorio tabelaPrecoAvulsoRepositorio,
            ITabelaPrecoAvulsoNotificacaoServico tabelaPrecoAvulsoNotificacaoServico)
        {
            _tabelaPrecoAvulsoRepositorio = tabelaPrecoAvulsoRepositorio;
            _tabelaPrecoAvulsoNotificacaoServico = tabelaPrecoAvulsoNotificacaoServico;
        }

        public List<TabelaPrecoAvulsoHoraValor> CarregarHoraValorDaTabela(int id)
        {
            return _tabelaPrecoAvulsoRepositorio.CarregarHoraValorDaTabela(id);
        }

        public List<TabelaPrecoAvulsoPeriodo> CarregarPeriodosDaTabela(int id)
        {
            return _tabelaPrecoAvulsoRepositorio.CarregarPeriodosDaTabela(id);
        }

        public List<TabelaPrecoAvulsoUnidade> CarregarUnidadesDaTabela(int id)
        {
            return _tabelaPrecoAvulsoRepositorio.CarregarUnidadesDaTabela(id);
        }

        public void Excluir(int id)
        {
            _tabelaPrecoAvulsoRepositorio.DeleteById(id);
        }

        public void Excluir(int id, int idUnidade, int idUsuario)
        {
            var entidade = _tabelaPrecoAvulsoRepositorio.GetById(id);
            entidade.Usuario = new Usuario { Id = idUsuario };

            entidade.ListaUnidade = entidade.ListaUnidade
                .Where(x => x.Unidade.Id != idUnidade)
                .ToList();

            _tabelaPrecoAvulsoRepositorio.Save(entidade);
        }

        public void Salvar(TabelaPrecoAvulso tabelaPrecoAvulso, int idUsuario)
        {
            if (tabelaPrecoAvulso.Status == StatusSolicitacao.Aguardando)
                throw new BusinessRuleException("A tabela de preço avulso, está aguardando aprovação, alteraçãos não serão salvas");

            tabelaPrecoAvulso.Status = StatusSolicitacao.Aguardando;

            if (tabelaPrecoAvulso.Id == 0)
                _tabelaPrecoAvulsoRepositorio.Save(tabelaPrecoAvulso);

            AdicionaNotificacoes(tabelaPrecoAvulso);
            _tabelaPrecoAvulsoNotificacaoServico.Criar(tabelaPrecoAvulso, idUsuario);

            _tabelaPrecoAvulsoRepositorio.Save(tabelaPrecoAvulso);
        }

        private void AdicionaPeriodo(TabelaPrecoAvulso tabelaPrecoAvulso)
        {
            var lista = tabelaPrecoAvulso.ListaPeriodo
                   .Select(x => new TabelaPrecoAvulsoPeriodo
                   {
                       DataInsercao = DateTime.Now,
                       TabelaPrecoAvulso = tabelaPrecoAvulso,
                       Periodo = x.Periodo
                   })
                   .ToList();

            tabelaPrecoAvulso.ListaPeriodo.Clear();

            foreach (var item in lista)
                tabelaPrecoAvulso.ListaPeriodo.Add(item);
        }

        private void AdicionaUnidade(TabelaPrecoAvulso tabelaPrecoAvulso)
        {
            var lista = tabelaPrecoAvulso.ListaUnidade
                   .Select(x => new TabelaPrecoAvulsoUnidade
                   {
                       DataInsercao = DateTime.Now,
                       TabelaPrecoAvulso = tabelaPrecoAvulso,
                       HoraInicio = x.HoraInicio,
                       HoraFim = x.HoraFim,
                       ValorDiaria = x.ValorDiaria,
                       Unidade = x.Unidade
                   })
                   .ToList();

            tabelaPrecoAvulso.ListaUnidade.Clear();

            foreach (var item in lista)
                tabelaPrecoAvulso.ListaUnidade.Add(item);
        }

        private void AdicionaHoraValor(TabelaPrecoAvulso tabelaPrecoAvulso)
        {
            var lista = tabelaPrecoAvulso.ListaHoraValor
                   .Select(x => new TabelaPrecoAvulsoHoraValor
                   {
                       DataInsercao = DateTime.Now,
                       TabelaPrecoAvulso = tabelaPrecoAvulso,
                       Hora = x.Hora,
                       Valor = x.Valor
                   })
                   .ToList();

            tabelaPrecoAvulso.ListaHoraValor.Clear();

            foreach (var item in lista)
                tabelaPrecoAvulso.ListaHoraValor.Add(item);
        }
        
        private void AdicionaNotificacoes(TabelaPrecoAvulso tabelaPrecoAvulso)
        {
            if (tabelaPrecoAvulso.Notificacoes == null)
                tabelaPrecoAvulso.Notificacoes = new List<TabelaPrecoAvulsoNotificacao>();

            var entidadeOriginal = _tabelaPrecoAvulsoRepositorio.GetById(tabelaPrecoAvulso.Id);
            if (entidadeOriginal.Notificacoes != null && entidadeOriginal.Notificacoes.Any())
            {
                tabelaPrecoAvulso.Notificacoes = entidadeOriginal.Notificacoes;
            }
        }
    }
}