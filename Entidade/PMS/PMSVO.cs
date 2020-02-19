using Entidade.Uteis;

namespace Entidade
{
    public class PMSVO
    {
        public int Dia { get; set; }
        public decimal ValorMensalidade { get; set; }
        public decimal ValorDia { get; set; }
        public StatusPMS StatusPMS { get; set; }

        public decimal ValorResultadoFinal
        {
            get
            {
                return ValorMensalidade + ValorDia;
            }
        }

        public PMSVO()
        {

        }

        public PMSVO(int dia, decimal vlrMensalidade, decimal vlrDia, StatusPMS statusPMS)
        {
            Dia = dia;
            ValorMensalidade = vlrMensalidade;
            ValorDia = vlrDia;
            StatusPMS = statusPMS;
        }
    }
}