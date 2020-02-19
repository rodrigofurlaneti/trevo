using System;
using System.ComponentModel;
using Entidade.Base;

namespace Aplicacao.ViewModels
{
    public enum TipoModal
    {
        [Description("success")]
        Success,
        [Description("danger")]
        Danger,
        [Description("warning")]
        Warning,
        [Description("info")]
        Info
    }

    public class DadosModal : IEntity
    {
        public bool Sucesso => TipoModal == TipoModal.Success;
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string RedirectUrl { get; set; }
        public string AcaoConfirma { get; set; }
        public string AcaoFunction { get; set; }
        public string TituloConfirma { get; set; }
        public TipoModal TipoModal { get; set; }
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public void GerarDadosModal(string titulo, string msg, TipoModal tipo, string acaoConfirma = null, string tituloConfirma = null, int? id = null, string redirectUrl = null)
        {
            Titulo = titulo;
            Mensagem = msg;
            TipoModal = tipo;

            if (acaoConfirma != null)
                AcaoConfirma = acaoConfirma;

            if (tituloConfirma != null)
                TituloConfirma = tituloConfirma;

            if (id != null)
                Id = id.Value;
                
            if (redirectUrl != null)
                RedirectUrl = redirectUrl;
        }
    }
}