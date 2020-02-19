using Entidade.Base;
using System;

namespace Aplicacao.ViewModels
{
    public class AuditViewModel
    {
        public string Usuario { get; set; }
        public string Entidade { get; set; }
        public string Atributo { get; set; }
        public int CodigoEntidade { get; set; }
        public string ValorAntigo { get; set; }
        public string ValorNovo { get; set; }
        public DateTime Data { get; set; }
        public string UsuarioNome { get; set; }
        public string DataEHora => Data.ToString("dd/MM/yyyy HH:mm:ss");

        public AuditViewModel(Audit entity)
        {
            this.Usuario = entity.Usuario;
            this.Entidade = entity.Entidade;
            this.Atributo = entity.Atributo;
            this.CodigoEntidade = entity.CodigoEntidade;
            this.ValorAntigo = entity.ValorAntigo;
            this.ValorNovo = entity.ValorNovo;
            this.Data = entity.Data;
            this.UsuarioNome = entity.UsuarioNome;
        }
    }
}