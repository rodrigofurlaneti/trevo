using Entidade;
using Entidade.Uteis;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aplicacao.ViewModels
{
    public class OcorrenciaClienteViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
       
        [Display(Name = "Número do Protocolo")]
        public string NumeroProtocolo { get; set; }
        public DateTime DataCompetencia { get; set; }

        public UnidadeViewModel Unidade { get; set; }
        public VeiculoViewModel Veiculo { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [Display(Name = "Atribuído")]
        public FuncionarioViewModel FuncionarioAtribuido { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [Display(Name = "Natureza")]
        public TipoNatureza Natureza { get; set; }

        [Required(ErrorMessage = "o campo Origem é obrigatório")]
        [Display(Name = "Valor")]
        public TipoOrigem Origem { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [Display(Name = "Prioridade")]
        public TipoPrioridade Prioridade { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [Display(Name = "Status")]
        public StatusOcorrencia StatusOcorrencia { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [Display(Name = "Cliente")]
        public ClienteViewModel Cliente { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Solução")]
        public string Solucao { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [Display(Name = "Data")]
        [DataType(DataType.Date, ErrorMessage ="Informe uma Data válida")]
        public DateTime DataOcorrencia { get; set; }

        public OcorrenciaClienteViewModel()
        {                  
        }

        public OcorrenciaClienteViewModel(OcorrenciaCliente ocorrenciaCliente)
        {
            Id = ocorrenciaCliente?.Id ?? 0;
            DataInsercao = ocorrenciaCliente?.DataInsercao == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : DateTime.Now;
            NumeroProtocolo = ocorrenciaCliente?.NumeroProtocolo ?? $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
            DataCompetencia = ocorrenciaCliente?.DataCompetencia ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            Unidade = ocorrenciaCliente.Unidade == null ? new UnidadeViewModel() : new UnidadeViewModel(ocorrenciaCliente.Unidade);
            Cliente = ocorrenciaCliente.Cliente == null ? new ClienteViewModel() : new ClienteViewModel(ocorrenciaCliente.Cliente); 
            FuncionarioAtribuido = ocorrenciaCliente.FuncionarioAtribuido == null ? new FuncionarioViewModel() : new FuncionarioViewModel(ocorrenciaCliente.FuncionarioAtribuido);
            Veiculo = ocorrenciaCliente.Veiculo == null ? new VeiculoViewModel() : new VeiculoViewModel(ocorrenciaCliente.Veiculo);
            Natureza = ocorrenciaCliente.Natureza;
            Origem = ocorrenciaCliente.Origem;
            Prioridade = ocorrenciaCliente.Prioridade;
            StatusOcorrencia = ocorrenciaCliente.StatusOcorrencia;
            Descricao = ocorrenciaCliente.Descricao;
            Solucao = ocorrenciaCliente.Solucao;
            DataOcorrencia = ocorrenciaCliente.DataOcorrencia;
        }

        public void GerarValoresPadrao()
        {
            DataInsercao = DateTime.Now;
            Natureza = TipoNatureza.Elogio;
            Origem = TipoOrigem.Supervisor;
            Prioridade = TipoPrioridade.Baixa;
            StatusOcorrencia = StatusOcorrencia.Novo;
            Unidade = new UnidadeViewModel();
            Cliente = new ClienteViewModel();
        }

        public void GerarProtocolo()
        {
            NumeroProtocolo = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
        }

        public OcorrenciaCliente ToEntity()
        {
            return new OcorrenciaCliente
            {
                Id = Id,
                DataInsercao = DataInsercao,
                NumeroProtocolo = NumeroProtocolo,
                DataCompetencia = DataCompetencia,
                Cliente = Cliente?.ToEntity(),
                Unidade = Unidade?.ToEntity(),
                Veiculo = Veiculo?.ToEntity(),
                FuncionarioAtribuido = FuncionarioAtribuido.ToEntity(),
                Natureza = Natureza,
                Origem = Origem,
                Prioridade = Prioridade,
                StatusOcorrencia = StatusOcorrencia,
                Descricao = Descricao,
                Solucao = Solucao,
                DataOcorrencia = DataOcorrencia
            };
        }
    }

    public class GridOcorrenciaClienteViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Protocolo { get; set; }

        public StatusOcorrencia Status { get; set; }
    }
}
