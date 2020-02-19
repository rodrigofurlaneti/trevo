using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class RetiradaCofreViewModel
    {
        public int Id { get; set; }
        public ContasAPagarViewModel ContasAPagar { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public Usuario Usuario { get; set; }
        public StatusRetiradaCofre StatusRetiradaCofre { get; set; }
        public string Observacoes { get; set; }

        public int? ContaFinanceiraId { get; set; }
        public int? DepartamentoId { get; set; }
        public int? UsuarioId { get; set; }

        public RetiradaCofreViewModel()
        {
            ContasAPagar = new ContasAPagarViewModel();
            Usuario = new Usuario();
        }
    }
}