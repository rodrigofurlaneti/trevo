using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aplicacao.Base;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IUtilsAplicacao
    {
        bool Validar(string campo, TipoValidacao tipoValidacao);
    }

    public class UtilsAplicacao : IUtilsAplicacao
    {
        public bool Validar(string campo, TipoValidacao tipoValidacao)
        {
            bool retorno = true;
            switch (tipoValidacao)
            {
                case TipoValidacao.Alfanumerico:
                    {
                        var reg = new Regex("^[a-zA-Z0-9 ]*$");
                        retorno = reg.IsMatch(campo);
                        break;
                    }
                case TipoValidacao.SomenteData:
                    DateTime data;
                    var isDate = DateTime.TryParse(campo, out data);
                    retorno = isDate;
                    break;
                case TipoValidacao.SomenteLetras:
                    {
                        var reg = new Regex("^[a-zA-Z ]*$");
                        retorno = reg.IsMatch(campo);
                        break;
                    }
                case TipoValidacao.SomenteNumeros:
                    int n;
                    var isNumeric = int.TryParse(campo, out n);
                    retorno = isNumeric;
                    break;
            }

            return retorno;
        }
    }
}