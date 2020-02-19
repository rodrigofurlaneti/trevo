using Aplicacao.Base;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IEmissaoSeloAplicacao : IBaseAplicacao<EmissaoSelo>
    {
        IList<EmissaoSelo> ListarEmissaoSeloFiltro(EmissaoSeloViewModel filtro);
        IList<EmissaoSelo> ListarEmissaoSeloFiltroSimples(EmissaoSeloViewModel filtro);
        EmissaoSelo GerarEmissaoSelos(EmissaoSelo filtro, int idPedido, DateTime? dataValidade);

        IList<ChaveValorViewModel> ListaCliente();
        IList<ChaveValorViewModel> ListaConvenio();
        IList<ChaveValorViewModel> ListaConvenio(int idUnidade);
        IList<ChaveValorViewModel> ListaUnidade(int? idConvenio = null);
        IList<ChaveValorViewModel> ListaUnidadesPorCliente(int idCliente);
        IList<ChaveValorViewModel> ListaTipoSelo(int? idConvenio = null, int? idUnidade = null);

        void SalvarEmissaoSeloGerada(EmissaoSelo emissaoSelo);
        bool PossuiValidade(int idTipoSelo);
        void Editar(EmissaoSeloViewModel emissaoSeloViewModel);

        void AlteraValidade(EmissaoSelo emissaoselo);
        void CancelarLote(EmissaoSelo emissaoSelo);

        TabelaPrecoAvulsoViewModel GetTabelaPrecoAvulsoPadrao(int idUnidade);
    }

    public class EmissaoSeloAplicacao : BaseAplicacao<EmissaoSelo, IEmissaoSeloServico>, IEmissaoSeloAplicacao
    {
        private readonly IEmissaoSeloServico _emissaoSeloServico;
        private readonly IClienteServico _clienteServico;
        private readonly IConvenioServico _convenioServico;
        private readonly IUnidadeServico _unidadeServico;
        private readonly ITipoSeloServico _tipoSeloServico;
        private readonly IPedidoSeloServico _pedidoSeloServico;
        private readonly ITabelaPrecoAvulsoAplicacao _tabelaPrecoAvulsoAplicacao;
        private readonly ISeloSoftparkAplicacao _seloSoftparkAplicacao;

        public EmissaoSeloAplicacao(
            IEmissaoSeloServico emissaoSeloServico,
            IClienteServico clienteServico,
            IConvenioServico convenioServico,
            IUnidadeServico unidadeServico,
            ITipoSeloServico tipoSeloServico,
            IPedidoSeloServico pedidoSeloServico,
            ITabelaPrecoAvulsoAplicacao tabelaPrecoAvulsoAplicacao,
            ISeloSoftparkAplicacao seloSoftparkAplicacao)
        {
            _emissaoSeloServico = emissaoSeloServico;
            _clienteServico = clienteServico;
            _convenioServico = convenioServico;
            _unidadeServico = unidadeServico;
            _tipoSeloServico = tipoSeloServico;
            _pedidoSeloServico = pedidoSeloServico;
            _tabelaPrecoAvulsoAplicacao = tabelaPrecoAvulsoAplicacao;
            _seloSoftparkAplicacao = seloSoftparkAplicacao;
        }

        public new EmissaoSelo SalvarComRetorno(EmissaoSelo emissaoselo)
        {
            if (!ObjetoValido(emissaoselo))
                throw new BusinessRuleException("Objeto Invalido!");

            var Id = _emissaoSeloServico.SalvarComRetorno(emissaoselo);

            return BuscarPorId(Id);
        }

        public new void Salvar(EmissaoSelo emissaoselo)
        {
            if (!ObjetoValido(emissaoselo))
                throw new BusinessRuleException("Objeto Invalido!");

            _emissaoSeloServico.Salvar(emissaoselo);
        }

        public void AlteraValidade(EmissaoSelo emissaoselo)
        {
            _emissaoSeloServico.Salvar(emissaoselo);
        }

        public bool ObjetoValido(EmissaoSelo entity)
        {
            if (entity?.PedidoSelo?.Unidade == null || entity.PedidoSelo.Unidade.Id == 0)
                throw new BusinessRuleException("Informe uma Unidade!");

            if (entity?.PedidoSelo?.Cliente == null || entity.PedidoSelo.Cliente.Id == 0)
                throw new BusinessRuleException("Informe um Cliente!");

            return true;
        }

        public IList<EmissaoSelo> ListarEmissaoSeloFiltro(EmissaoSeloViewModel filtro)
        {
            return _emissaoSeloServico.ListarEmissaoSeloFiltro(filtro?.PedidoSelo?.Unidade?.Id ?? 0, filtro?.PedidoSelo?.Cliente?.Id ?? 0, filtro?.PedidoSelo?.TipoSelo?.Id ?? 0, filtro?.Id ?? 0);
        }

        public IList<EmissaoSelo> ListarEmissaoSeloFiltroSimples(EmissaoSeloViewModel filtro)
        {
            return _emissaoSeloServico.ListarEmissaoSeloFiltroSimples(filtro?.StatusSelo ?? new Entidade.Uteis.StatusSelo?(), filtro?.PedidoSelo?.Unidade?.Id ?? 0, filtro?.PedidoSelo?.Cliente?.Id ?? 0, filtro?.PedidoSelo?.TipoSelo?.Id ?? 0, filtro?.Id ?? 0);
        }

        public EmissaoSelo GerarEmissaoSelos(EmissaoSelo filtro, int idPedido, DateTime? dataValidade)
        {
            return _emissaoSeloServico.GerarEmissaoSelos(filtro, idPedido, dataValidade);
        }

        public IList<ChaveValorViewModel> ListaCliente()
        {
            var lista = _clienteServico.Buscar()
                ?.Select(x => new ClienteViewModel(x))
                ?.Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = x.Descricao
                })
                ?.ToList();

            return lista ?? new List<ChaveValorViewModel>();
        }

        public IList<ChaveValorViewModel> ListaConvenio()
        {
            var lista = _convenioServico.Buscar()
                .Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = x.Descricao
                })
                .ToList();

            return lista ?? new List<ChaveValorViewModel>();
        }

        public IList<ChaveValorViewModel> ListaConvenio(int idUnidade)
        {
            var lista = _convenioServico.BuscarAtivosPorUnidade(idUnidade).Select(x => new ChaveValorViewModel
            {
                Id = x.Id,
                Descricao = x.Descricao
            }).ToList();

            return lista ?? new List<ChaveValorViewModel>();
        }

        public IList<ChaveValorViewModel> ListaUnidade(int? idConvenio = null)
        {
            var lista = _unidadeServico.ListaUnidade(idConvenio)
                .Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = x.Nome
                })
                .ToList();

            return lista ?? new List<ChaveValorViewModel>();
        }

        public IList<ChaveValorViewModel> ListaUnidadesPorCliente(int idCliente)
        {
            var lista = _clienteServico.BuscarPorId(idCliente)?.Unidades?.Select(x => x?.Unidade)?
                .Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = x.Nome
                }).ToList();

            return lista ?? new List<ChaveValorViewModel>();
        }

        public IList<ChaveValorViewModel> ListaTipoSelo(int? idConvenio = null, int? idUnidade = null)
        {
            var lista = _tipoSeloServico.ListaTipoSelo(idConvenio, idUnidade)
                .Select(x => new ChaveValorViewModel
                {
                    Id = x.Id,
                    Descricao = x.Nome
                })
                .ToList();

            return lista ?? new List<ChaveValorViewModel>();
        }

        public void SalvarEmissaoSeloGerada(EmissaoSelo emissaoSelo)
        {
            _emissaoSeloServico.SalvarEmissaoSeloGerada(emissaoSelo);

            emissaoSelo = _emissaoSeloServico.BuscarPorId(emissaoSelo.Id);

            var selos = emissaoSelo.Selo.Select(x => new SeloSoftparkViewModel(x)).ToList();
            _seloSoftparkAplicacao.AtualizarSelos(selos);
        }

        public void Editar(EmissaoSeloViewModel model)
        {
            var emissaoSelo = _emissaoSeloServico.BuscarPorId(model.Id);
            emissaoSelo.Responsavel = model.Responsavel;
            emissaoSelo.DataEntrega = model.DataEntrega;
            emissaoSelo.EntregaRealizada = model.EntregaRealizada;
            emissaoSelo.ClienteRemetente = model.ClienteRemetente;
            emissaoSelo.NomeImpressaoSelo = model.NomeImpressaoSelo;

            _emissaoSeloServico.Salvar(emissaoSelo);
        }

        public bool PossuiValidade(int idTipoSelo)
        {
            return _tipoSeloServico.BuscarPorId(idTipoSelo).ComValidade;
        }

        public void CancelarLote(EmissaoSelo emissaoSelo)
        {
            Servico.Salvar(emissaoSelo);

            _seloSoftparkAplicacao.CancelarLoteSelo(emissaoSelo.Id, emissaoSelo.NumeroLote);
        }

        public TabelaPrecoAvulsoViewModel GetTabelaPrecoAvulsoPadrao(int idUnidade)
        {
            var entidade = _tabelaPrecoAvulsoAplicacao.BuscarPor(x => x.Padrao == true)?.ToList()?.LastOrDefault(x => x.ListaUnidade != null && x.ListaUnidade.Any(y => y.Unidade.Id == idUnidade)) ?? null;
            return entidade != null ? new TabelaPrecoAvulsoViewModel(entidade) : null;
        }
    }
}