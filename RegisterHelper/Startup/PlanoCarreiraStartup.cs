using System;
using System.Collections.Generic;
using Aplicacao;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace InitializerHelper.Startup
{
    public static class TipoBeneficioStartup
    {
        #region Private Members
        #endregion

        #region Public Members

        public static void Start()
        {
            AdicionarTipoBeneficio();
        }

        private static void AdicionarTipoBeneficio()
        {
            var _tipoBeneficioAplicacao = ServiceLocator.Current.GetInstance<ITipoBeneficioAplicacao>();

            if (_tipoBeneficioAplicacao.PrimeiroPor(x => x.Descricao == "Plano de Carreira") == null)
            {
                var tipoBeneficio = new TipoBeneficio
                {
                    Descricao = "Plano de Carreira",
                    Ativo = true,
                    DataInsercao = DateTime.Now
                };

                _tipoBeneficioAplicacao.Salvar(tipoBeneficio);
            }
        }

        #endregion
    }
}