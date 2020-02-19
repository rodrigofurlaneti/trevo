using Dominio.IRepositorio.Base;
using Entidade;
using System;

namespace Dominio.IRepositorio
{
    public interface IFeriasClienteRepositorio : IRepository<FeriasCliente>
    {
        bool VerificaExistenciaBoletoMensalista(int idCliente, DateTime dataCompetencia);
    }
}