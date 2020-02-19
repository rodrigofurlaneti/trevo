using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class MaquinaCartaoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public int NumeroMaquina { get; set; }
        public string Observacao { get; set; }
        public string MarcaMaquina { get; set; }
        public DocumentoViewModel CNPJ { get; set; }
        public FuncionarioViewModel Responsavel { get; set; }
        public string IdSupervisorMaquina { get; set; }

        public MaquinaCartaoViewModel()
        {

        }

        public MaquinaCartaoViewModel(MaquinaCartao maquinaCartao)
        {
            Id = maquinaCartao.Id;
            DataInsercao = maquinaCartao.DataInsercao;
            NumeroMaquina = maquinaCartao.NumeroMaquina;
            Observacao = maquinaCartao.Observacao;
            MarcaMaquina = maquinaCartao.MarcaMaquina;
            CNPJ = maquinaCartao?.CNPJ != null ? new DocumentoViewModel(maquinaCartao.CNPJ) : new DocumentoViewModel();
        }

        public MaquinaCartao ToEntity()
        {
            return new MaquinaCartao
            {
                Id = Id,
                DataInsercao = DataInsercao,
                NumeroMaquina = NumeroMaquina,
                Observacao = Observacao,
                MarcaMaquina = MarcaMaquina,
                CNPJ = CNPJ?.ToEntity()
            };
        }

        public MaquinaCartaoViewModel ToViewModel(MaquinaCartao maquinaCartao)
        {
            return new MaquinaCartaoViewModel
            {
                Id = maquinaCartao.Id,
                DataInsercao = maquinaCartao.DataInsercao,
                NumeroMaquina = maquinaCartao.NumeroMaquina,
                Observacao = maquinaCartao.Observacao,
                MarcaMaquina = maquinaCartao.MarcaMaquina,
                CNPJ = maquinaCartao?.CNPJ != null ? new DocumentoViewModel(maquinaCartao.CNPJ) : new DocumentoViewModel()
        };
            
        }

    }
}
