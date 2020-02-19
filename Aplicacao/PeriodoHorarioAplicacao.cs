using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;
using System.Linq;
using Aplicacao.ViewModels;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IPeriodoHorarioAplicacao : IBaseAplicacao<PeriodoHorario>
    { }

    public class PeriodoHorarioAplicacao : BaseAplicacao<PeriodoHorario, IPeriodoHorarioServico>, IPeriodoHorarioAplicacao
    {
        //public bool ObjetoValido(PeriodoHorario entity)
        //{
        //    if (string.IsNullOrEmpty(entity.Descricao))
        //        throw new BusinessRuleException("Informe o Tipo de Atividade!");

        //    return true;
        //}
    }
}