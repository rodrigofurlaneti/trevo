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
    public interface ICheckListEstruturaUnidadeAplicacao : IBaseAplicacao<CheckListEstruturaUnidade>
    { }

    public class CheckListEstruturaUnidadeAplicacao : BaseAplicacao<CheckListEstruturaUnidade, ICheckListEstruturaUnidadeServico>, ICheckListEstruturaUnidadeAplicacao
    {

    }
}