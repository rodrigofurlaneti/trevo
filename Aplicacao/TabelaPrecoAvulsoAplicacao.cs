using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface ITabelaPrecoAvulsoAplicacao : IBaseAplicacao<TabelaPrecoAvulso>
    {
        void Salvar(TabelaPrecoAvulsoViewModel viewModel, int idUsuario);
        List<TabelaPrecoAvulsoGridViewModel> CarregarGrid();
        TabelaPrecoAvulsoViewModel CarregarTelaParaEdicao(int id);
        List<TabelaPrecoAvulsoUnidadeViewModel> CarregarGridUnidade(int id);
        List<TabelaPrecoAvulsoHoraValorViewModel> CarregarGridHoraValor(int id);
        TabelaPrecoAvulsoPeriodoViewModel CarregarGridPeriodo(int id);
        void Excluir(int id);
        void Excluir(int id, int idUnidade, int idUsuario);
    }

    public class TabelaPrecoAvulsoAplicacao : BaseAplicacao<TabelaPrecoAvulso, ITabelaPrecoAvulsoServico>, ITabelaPrecoAvulsoAplicacao
    {
        private readonly ITabelaPrecoAvulsoServico _tabelaPrecoAvulsoServico;
        private readonly ITabelaPrecoSoftparkAplicacao _tabelaPrecoSoftparkAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public TabelaPrecoAvulsoAplicacao(
            ITabelaPrecoAvulsoServico tabelaPrecoAvulsoServico,
            ITabelaPrecoSoftparkAplicacao tabelaPrecoSoftparkAplicacao,
            IUnidadeAplicacao unidadeAplicacao)
        {
            _tabelaPrecoAvulsoServico = tabelaPrecoAvulsoServico;
            _tabelaPrecoSoftparkAplicacao = tabelaPrecoSoftparkAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        public void Excluir(int id)
        {
            _tabelaPrecoAvulsoServico.Excluir(id);

            _tabelaPrecoSoftparkAplicacao.ExcluirPorId(id);
        }

        public void Excluir(int id, int idUnidade, int idUsuario)
        {
            _tabelaPrecoAvulsoServico.Excluir(id, idUnidade, idUsuario);

            var tabelaPrecoAvulso = _tabelaPrecoAvulsoServico.BuscarPorId(id);
            var tabelaPreco = new TabelaPrecoSoftparkViewModel(tabelaPrecoAvulso);
            _tabelaPrecoSoftparkAplicacao.Salvar(tabelaPreco);
        }

        public void Salvar(TabelaPrecoAvulsoViewModel viewModel, int idUsuario)
        {
            var entidade = viewModel.ToEntity();
            
            entidade.ListaHoraValor = viewModel.ListaHoraValor.
                Select(x => new TabelaPrecoAvulsoHoraValor
                {
                    TabelaPrecoAvulso = entidade,
                    Hora = x.Hora,
                    Valor = Convert.ToDecimal(x.Valor)
                })
                .ToList();

            entidade.ListaUnidade = viewModel.ListaUnidade
                .Select(x => new TabelaPrecoAvulsoUnidade
                {
                    TabelaPrecoAvulso = entidade,
                    HoraFim = x.HoraFim,
                    HoraInicio = x.HoraInicio,
                    ValorDiaria = decimal.Parse(x.ValorDiaria),
                    Unidade = new Unidade { Id = x.Unidade.Id }
                })
                .ToList();
            
            entidade.ListaPeriodo = viewModel.Periodo.ListaPeriodoSelecionado
                .Select(x => new TabelaPrecoAvulsoPeriodo
                {
                    TabelaPrecoAvulso = entidade,
                    Periodo = x
                })
                .ToList();

            entidade.Usuario = new Usuario { Id = idUsuario };

            _tabelaPrecoAvulsoServico.Salvar(entidade, idUsuario);

            var tabelaPrecoAvulso = _tabelaPrecoAvulsoServico.BuscarPorId(entidade.Id);
            foreach (var unidade in tabelaPrecoAvulso.ListaUnidade.Select(x => x.Unidade))
            {
                tabelaPrecoAvulso.ListaUnidade.Select(x => x.Unidade = _unidadeAplicacao.BuscarPorId(unidade.Id));
            }
            var tabelaPreco = new TabelaPrecoSoftparkViewModel(tabelaPrecoAvulso);
            _tabelaPrecoSoftparkAplicacao.Salvar(tabelaPreco);
        }

        public TabelaPrecoAvulsoViewModel CarregarTelaParaEdicao(int id)
        {
            var entidade = _tabelaPrecoAvulsoServico.BuscarPorId(id);
            var viewModel = new TabelaPrecoAvulsoViewModel(entidade);

            return viewModel;
        }

        public List<TabelaPrecoAvulsoGridViewModel> CarregarGrid()
        {
            var lista = new List<TabelaPrecoAvulsoGridViewModel>();
            var listaTabelaPrecoAvulso = _tabelaPrecoAvulsoServico.Buscar();
            foreach (var item in listaTabelaPrecoAvulso)
                foreach (var unidade in item.ListaUnidade)
                    lista.Add(new TabelaPrecoAvulsoGridViewModel(item, unidade.Unidade));
            
            return lista;
        }

        public List<TabelaPrecoAvulsoUnidadeViewModel> CarregarGridUnidade(int id)
        {
            var lista = new List<TabelaPrecoAvulsoUnidadeViewModel>();
            if (id != 0)
            {
                lista = _tabelaPrecoAvulsoServico.CarregarUnidadesDaTabela(id)
                    ?.Select(x => new TabelaPrecoAvulsoUnidadeViewModel(_unidadeAplicacao.BuscarPorId(x.Unidade.Id), x.HoraInicio, x.HoraFim, x.ValorDiaria.ToString("N2")))
                    ?.ToList();
            }
            
            return lista;
        }

        public List<TabelaPrecoAvulsoHoraValorViewModel> CarregarGridHoraValor(int id)
        {
            var lista = new List<TabelaPrecoAvulsoHoraValorViewModel>();
            if (id != 0)
            {
                lista = _tabelaPrecoAvulsoServico.CarregarHoraValorDaTabela(id)
                    ?.Select(x => new TabelaPrecoAvulsoHoraValorViewModel(x))
                    ?.ToList();
            }

            return lista;
        }

        public TabelaPrecoAvulsoPeriodoViewModel CarregarGridPeriodo(int id)
        {
            var viewModel = new TabelaPrecoAvulsoPeriodoViewModel();
            if (id != 0)
            {
                var periodos = _tabelaPrecoAvulsoServico.CarregarPeriodosDaTabela(id);
                if (periodos != null && periodos.Any())
                    viewModel.ListaPeriodoSelecionado = periodos.Select(x => x.Periodo).ToList();
            }

            return viewModel;
        }
    }
}