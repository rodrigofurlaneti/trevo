using System;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using Core.Extensions;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IPlanoCarreiraAplicacao : IBaseAplicacao<PlanoCarreira>
    {
        string BuscarValorPeloPeriodo(TipoBeneficioViewModel tipoBeneficio, int funcionarioId);
    }

    public class PlanoCarreiraAplicacao : BaseAplicacao<PlanoCarreira, IPlanoCarreiraServico>, IPlanoCarreiraAplicacao
    {
        private readonly IFuncionarioServico _funcionarioServico;

        public PlanoCarreiraAplicacao(IFuncionarioServico funcionarioServico)
        {
            _funcionarioServico = funcionarioServico;
        }

        public string BuscarValorPeloPeriodo(TipoBeneficioViewModel tipoBeneficio, int funcionarioId)
        {
            if (tipoBeneficio.Descricao == "Plano de Carreira")
            {
                var funcionario = _funcionarioServico.BuscarPorId(funcionarioId);
                if (funcionario.DataAdmissao.HasValue)
                {
                    var tempoAdmissao = funcionario.DataAdmissao?.TotalAnos(DateTime.Now) ?? 0;
                    var planocarreira = Servico.PrimeiroPor(x => tempoAdmissao >= x.AnoDe && tempoAdmissao <= x.AnoAte);

                    return planocarreira?.Valor.ToString() ?? "0,00";
                }
            }

            return "0,00";
        }
    }
}