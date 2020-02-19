using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System;

namespace Dominio
{
    public interface IFeriasClienteServico : IBaseServico<FeriasCliente>
    {
        bool VerificaExistenciaBoletoMensalista(int idCliente, DateTime dataCompetencia);
    }

    public class FeriasClienteServico : BaseServico<FeriasCliente, IFeriasClienteRepositorio>, IFeriasClienteServico
    {
        private readonly IFeriasClienteRepositorio _feriasClienteRepositorio;

        public FeriasClienteServico(IFeriasClienteRepositorio feriasClienteRepositorio)
        {
            _feriasClienteRepositorio = feriasClienteRepositorio;
        }

        public bool VerificaExistenciaBoletoMensalista(int idCliente, DateTime dataCompetencia)
        {
            return _feriasClienteRepositorio.VerificaExistenciaBoletoMensalista(idCliente, dataCompetencia);
        }
    }
}