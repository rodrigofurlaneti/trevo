using BoletoNet;
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
    public interface ILancamentoCobrancaContratoMensalistaServico : IBaseServico<LancamentoCobrancaContratoMensalista>
    {
        LancamentoCobrancaContratoMensalista RetornaUltimoLancamentoCobrancaPor(int id);
    }

    public class LancamentoCobrancaContratoMensalistaServico : BaseServico<LancamentoCobrancaContratoMensalista, ILancamentoCobrancaContratoMensalistaRepositorio>, ILancamentoCobrancaContratoMensalistaServico
    {
        private readonly ILancamentoCobrancaContratoMensalistaRepositorio _lancamentoCobrancaContratoMensalistaRepositorio;
        
        public LancamentoCobrancaContratoMensalistaServico(ILancamentoCobrancaContratoMensalistaRepositorio lancamentoCobrancaContratoMensalistaRepositorio)
        {
            _lancamentoCobrancaContratoMensalistaRepositorio = lancamentoCobrancaContratoMensalistaRepositorio;
        }
        
        public LancamentoCobrancaContratoMensalista RetornaUltimoLancamentoCobrancaPor(int id)
        {
            return _lancamentoCobrancaContratoMensalistaRepositorio.RetornaUltimoLancamentoCobrancaPor(id);
        }
    }
}