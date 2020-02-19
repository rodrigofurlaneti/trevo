using Core.Extensions;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class FeriasClienteViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public DateTime DataInicio { get; set; }
        public string DataInicioStr
        {
            get
            {
                return DataInicio.ToShortDateString();
            }
        }
        public DateTime DataFim { get; set; }
        public string DataFimStr
        {
            get
            {
                return DataFim.ToShortDateString();
            }
        }
        public bool InutilizarTodasVagas { get; set; }
        public int TotalVagas { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public ContratoMensalistaViewModel ContratoMensalista { get; set; }
        public decimal ValorFeriasCalculada { get; set; }
        public UsuarioViewModel UsuarioCadastro { get; set; }

        public IList<FeriasClienteDetalheViewModel> ListaFeriasClienteDetalhe { get; set; }

        public DateTime DataCompetencia
        {
            get
            {
                return new DateTime(DataFim.AddMonths(1).Year, DataFim.AddMonths(1).Month, 1);
            }
        }
        public List<DateTime> ListaDataCompetenciaPeriodo
        {
            get
            {
                var meses = DataInicio.GetMonthDifference(DataFim);
                if (meses == 0)
                    return new List<DateTime> { new DateTime(DataInicio.AddMonths(1).Year, DataInicio.AddMonths(1).Month, 1) };

                var lista = new List<DateTime>();
                for (int i = 0; i <= meses; i++)
                {
                    lista.Add(new DateTime(DataInicio.AddMonths(i + 1).Year, DataInicio.AddMonths(i + 1).Month, 1));
                }
                return lista;
            }
        }
        public bool IsEdited { get; set; }

        public FeriasClienteViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public FeriasClienteViewModel(FeriasCliente obj)
        {
            Id = obj?.Id ?? 0;
            DataInsercao = obj?.DataInsercao ?? DateTime.Now;
            DataInicio = obj?.DataInicio ?? DateTime.Now;
            DataFim = obj?.DataFim ?? DateTime.Now;
            InutilizarTodasVagas = obj?.InutilizarTodasVagas ?? false;
            TotalVagas = obj?.TotalVagas ?? 0;
            Cliente = new ClienteViewModel(obj?.Cliente);
            ContratoMensalista = new ContratoMensalistaViewModel(obj?.ContratoMensalista);
            ValorFeriasCalculada = obj?.ValorFeriasCalculada ?? 0;
            UsuarioCadastro = obj?.UsuarioCadastro != null ? new UsuarioViewModel(obj?.UsuarioCadastro) : null;
            ListaFeriasClienteDetalhe = obj?.ListaFeriasClienteDetalhe?.Select(x => new FeriasClienteDetalheViewModel(x))?.ToList() ?? new List<FeriasClienteDetalheViewModel>();
        }

        public FeriasCliente ToEntity() => new FeriasCliente()
        {
            Id = this.Id,
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            DataInicio = this.DataInicio,
            DataFim = this.DataFim,
            InutilizarTodasVagas = this.InutilizarTodasVagas,
            TotalVagas = this.TotalVagas,
            Cliente = new Cliente { Id = this.Cliente?.Id ?? 0 },
            ContratoMensalista = new ContratoMensalista { Id = this.ContratoMensalista?.Id ?? 0 },
            ValorFeriasCalculada = this.ValorFeriasCalculada,
            UsuarioCadastro = this.UsuarioCadastro != null ? new Usuario { Id = this.UsuarioCadastro?.Id ?? 0 } : null,
            ListaFeriasClienteDetalhe = this.ListaFeriasClienteDetalhe != null && this.ListaFeriasClienteDetalhe.Any() ? this.ListaFeriasClienteDetalhe?.Select(x => x.ToEntity())?.ToList() : new List<FeriasClienteDetalhe>()
        };
    }
}
