using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class MaterialHistoricoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public string NumeroNota { get; set; }
        public int Quantidade { get; set; }
        public bool EhUmAtivo { get; set; }
        public AcaoEstoqueManual AcaoEstoqueManual { get; set; }

        public MaterialViewModel Material { get; set; }
        public EstoqueViewModel Estoque { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public UsuarioViewModel Usuario { get; set; }
    }
}
