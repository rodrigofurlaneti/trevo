using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface IPagamentoReembolsoServico : IBaseServico<PagamentoReembolso>
    {
        IList<PagamentoReembolso> ListarLancamentoReembolso(int? unidade, DateTime? dataInsercao, int? departamento, string numeroRecibo);
    }

    public class PagamentoReembolsoServico : BaseServico<PagamentoReembolso, IPagamentoReembolsoRepositorio>, IPagamentoReembolsoServico
    {
        private readonly IPagamentoReembolsoRepositorio _pagamentoReembolsoRepositorio;

        public PagamentoReembolsoServico(IPagamentoReembolsoRepositorio pagamentoReembolsoRepositorio)
        {
            _pagamentoReembolsoRepositorio = pagamentoReembolsoRepositorio;
        }

        public IList<PagamentoReembolso> ListarLancamentoReembolso(int? unidade, DateTime? dataInsercao, int? departamento, string numeroRecibo)
        {
            if (DateTime.TryParse(dataInsercao.Value.ToString(), out DateTime dtValida) && dataInsercao.Value > (new DateTime(1900, 1, 1)))
                dataInsercao = dtValida;
            else
                dataInsercao = DateTime.MinValue;

            if (!unidade.HasValue)
                unidade = 0;

            if (!departamento.HasValue)
                departamento = 0;

            return _pagamentoReembolsoRepositorio
                .ListBy(x =>
                       (unidade == 0 || x.ContaAPagar.Unidade.Id == unidade)
                    && (dataInsercao == DateTime.MinValue || x.DataInsercao.Date == dtValida.Date)
                    && (departamento == 0 || x.ContaAPagar.Departamento.Id == departamento)
                    && (string.IsNullOrEmpty(numeroRecibo) || x.NumeroRecibo == numeroRecibo)
                    && (x.Status == StatusPagamentoReembolso.Pendente))
                .ToList();
        }
    }
}