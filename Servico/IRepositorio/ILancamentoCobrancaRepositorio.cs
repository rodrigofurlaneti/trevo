using Dominio.IRepositorio.Base;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface ILancamentoCobrancaRepositorio : IRepository<LancamentoCobranca>
    {
        IList<LancamentoCobranca> ListarLancamentosCobranca(int? idContaFinanceira, TipoServico? tipoServico, 
            DateTime? dataVencimentoIni, DateTime? dataVencimentoFim, int? idCliente, string documento);
        IList<LancamentoCobranca> ListarLancamentosCobranca(int? idContaFinanceira, TipoServico? tipoServico, int? idUnidade, 
            StatusLancamentoCobranca? statusLancamentoCobranca, TipoFiltroGeracaoCNAB? tipoFiltroGeracaoCNAB, int? supervisor, int? cliente,
            string dataDe, string dataAte);
        IList<LancamentoCobranca> BuscarLancamentosCobranca(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, int numeroContrato, List<int> listaIds = null);
        void UpdateDetalhesCNAB(List<LancamentoCobranca> listaLancamentos);

        IList<DadosLancamentosVO> BuscarLancamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);
        IList<DadosLancamentosVO> BuscarLancamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);

        IList<DadosPagamentoVO> BuscarPagamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico tipoServico, int unidade);
        IList<DadosPagamentoVO> BuscarPagamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico tipoServico, int unidade, bool acrescentarCancelados = false);

        IList<DadosPagamentoVO> BuscarPagamentosContratoMensalistasPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
        IList<DadosPagamentoVO> BuscarPagamentosContratoMensalistasEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade, bool acrescentarCancelados = false);

        IList<DadosPagamentoVO> BuscarPagamentosPedidoSeloPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
        IList<DadosPagamentoVO> BuscarPagamentosPedidoSeloEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade, bool acrescentarCancelados = false);

        IList<DadosPagamentoVO> BuscarPagamentosPedidoLocacaoPagosRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
        IList<DadosPagamentoVO> BuscarPagamentosPedidoLocacaoEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, int unidade, bool acrescentarCancelados = false);

        IList<LancamentoCobranca> BuscarLancamentosCobrancaLinq(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, List<int> listaIds);

        IList<DadosPagamentoVO> BuscarPagamentosEfetuadosConferenciaContratoMensalistasRelatorio(DateTime dataInicio, DateTime dataFim, int unidade);
    }
}