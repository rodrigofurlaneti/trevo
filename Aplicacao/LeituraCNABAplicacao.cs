using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using BoletoNet;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ILeituraCNABAplicacao : IBaseAplicacao<LeituraCNAB>
    {
        List<LeituraCNABPreviaViewModel> SalvarLeituraCNAB(LeituraCNAB leitura, List<DetalheRetorno> ListaRetornoCNAB, int usuarioId, bool previa = false);
    }

    public class LeituraCNABAplicacao : BaseAplicacao<LeituraCNAB, ILeituraCNABServico>, ILeituraCNABAplicacao
    {
        private readonly ILeituraCNABServico _leituraCNABServico;
        
        public LeituraCNABAplicacao(ILeituraCNABServico leituraCNABServico)
        {
            _leituraCNABServico = leituraCNABServico;
        }
        
        public List<LeituraCNABPreviaViewModel> SalvarLeituraCNAB(LeituraCNAB leitura, List<DetalheRetorno> ListaRetornoCNAB, int usuarioId, bool previa = false)
        {
            return _leituraCNABServico.SalvarLeituraCNAB(leitura, ListaRetornoCNAB, usuarioId, previa)?
                        .Select(x => AutoMapper.Mapper.Map<LeituraCNABPreviaVO, LeituraCNABPreviaViewModel>(x))?
                        .ToList();
        }
    }
}