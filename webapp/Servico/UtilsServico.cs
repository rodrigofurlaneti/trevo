using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entidade;
using Entidade.Uteis;

namespace Portal.Utils
{
    public static class UtilsServico
    {

        public static int RetornarNumeroMes(Entidade.Uteis.Mes Mes)
        {
            int ret = 0;

            switch (Mes)
            {
                case Mes.Janeiro:
                    ret = 1;
                    break;
                case Mes.Fevereiro:
                    ret = 2;
                    break;
                case Mes.Março:
                    ret = 3;
                    break;
                case Mes.Abril:
                    ret = 4;
                    break;
                case Mes.Maio:
                    ret = 5;
                    break;
                case Mes.Junho:
                    ret = 6;
                    break;
                case Mes.Julho:
                    ret = 7;
                    break;
                case Mes.Agosto:
                    ret = 8;
                    break;
                case Mes.Setembro:
                    ret = 9;
                    break;
                case Mes.Outubro:
                    ret = 10;
                    break;
                case Mes.Novembro:
                    ret = 11;
                    break;
                case Mes.Dezembro:
                    ret = 12;
                    break;
                default:
                    break;
            }


            return ret;


        }



        public static List<string> ListaAnosAnteriores(int ano, int numeroAnos)
        {
            List<string> Anos = new List<string>();

            int anoinc = ano;

            for (int i = numeroAnos; i > 0; i--)
            {

                Anos.Add(anoinc.ToString());

                anoinc--;
            }

            return Anos;

        }
    }
}