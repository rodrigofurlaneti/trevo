using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entidade.Uteis;

namespace Portal.Models
{
    public class NotificacaoMainData
    {
        public int totalNotificacoes;
        public List<NotificacaoData> data;

        public NotificacaoMainData()
        {
            totalNotificacoes = 0;
            data = new List<NotificacaoData>();
        }
    }

    public class NotificacaoData
    {
        public int IdTipoNotificacao { get; set; }
        public string Mensagem { get; set; }
        public string Entidade { get; set; }
        public string EntidadeForm { get; set; }
        public string UltimaAtualizacao { get; set; }
    }
}