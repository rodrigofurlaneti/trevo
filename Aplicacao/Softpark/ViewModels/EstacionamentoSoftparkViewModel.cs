using Core.Extensions;
using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a Unidade
    /// </summary>
    public class EstacionamentoSoftparkViewModel : BaseSoftparkViewModel
    {
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string Dizeres { get; set; }
        public string Horario { get; set; }
        public string Codigo { get; set; }
        public string RazaoSocial { get; set; }
        public bool InAtivo { get; set; }
        public string UserName { get; set; }
        public int DiaPgtoMensal { get; set; }
        public decimal JurosMulta { get; set; }
        public decimal JurosMora { get; set; }
        public string Ccm { get; set; }
        public string Cnpj { get; set; }
        public string CaminhoLogoTipo { get; set; }
        public string CaminhoLogoEmpresa { get; set; }
        public int ToleranciaMulta { get; set; }
        public string EmpresaNome { get; set; }

        public EstacionamentoSoftparkViewModel()
        {
        }

        public EstacionamentoSoftparkViewModel(Unidade unidade)
        {
            Id = unidade.Id;
            DataInsercao = unidade.DataInsercao;
            Nome = unidade.Nome;
            Endereco = unidade.Endereco?.Resumo ?? "null";
            Complemento = unidade.Endereco?.Complemento ?? "null";
            Cep = unidade.Endereco?.Cep?.RemoveSpecialCharacters() ?? "null";
            Telefone = "";
            Dizeres = "";
            Horario = unidade.HorarioInicial + " - " + unidade.HorarioFinal;
            Codigo = unidade.Codigo;
            RazaoSocial = unidade.Nome;
            InAtivo = unidade.Ativa;
            UserName = "";
            DiaPgtoMensal = 0;
            JurosMulta = 0;
            JurosMora = 0;
            Ccm = unidade.CCM;
            Cnpj = unidade.CNPJ?.UnformatCpfCnpj();
            CaminhoLogoTipo = "";
            CaminhoLogoEmpresa = "";
            ToleranciaMulta = 0;
            EmpresaNome = unidade.Empresa?.RazaoSocial;
        }
    }
}
