using System.Collections.Generic;
using Aplicacao.ViewModels;
namespace Portal.Helpers
{
    public static class Contato
    {
        public static void DefineNumero(List<ContatoViewModel> contatosViewModel)
        {
            try
            {
              
                foreach(var contatoViewModel in contatosViewModel)
                {
                    if(!string.IsNullOrEmpty(contatoViewModel.Celular?.Trim().TrimStart().TrimEnd()))
                    {
                        contatoViewModel.Numero = contatoViewModel.Celular;
                    }
                    if (!string.IsNullOrEmpty(contatoViewModel.Telefone?.Trim().TrimStart().TrimEnd()))
                    {
                        contatoViewModel.Numero = contatoViewModel.Telefone;
                    }
                    if (!string.IsNullOrEmpty(contatoViewModel.Email?.Trim().TrimStart().TrimEnd()))
                    {
                        contatoViewModel.Email = contatoViewModel.Email;
                    }
                }
               
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}