using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class FuncionarioViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public PessoaViewModel Pessoa { get; set; }
        public FuncionarioViewModel Supervisor { get; set; }
        public string Salario { get; set; }
        public CargoViewModel Cargo { get; set; }
        public string DocumentoCpf => Pessoa?.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Cpf)?.Numero ?? string.Empty;
        public string Imagem { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public StatusFuncionario Status { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Codigo { get; set; }
        public bool Selected { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
        public SituacaoSageFuncionario SituacaoSage { get; set; }
        public ItemFuncionarioViewModel ItemFuncionario { get; set; }
        public BeneficioFuncionarioViewModel BeneficioFuncionario { get; set; }
        public OcorrenciaFuncionarioViewModel OcorrenciaFuncionario { get; set; }
        public List<ControleFeriasViewModel> ControlesFerias { get; set; }
        public List<ControlePontoDiaViewModel> ControlePontoDiaFaltas { get; set; }
        public List<FuncionarioIntervaloDozeTrintaSeisViewModel> ListaIntervaloDozeTrintaSeis { get; set; }
        public List<FuncionarioIntervaloCompensacaoViewModel> ListaIntervaloCompensacao { get; set; }
        public List<FuncionarioIntervaloNoturnoViewModel> ListaIntervaloNoturno { get; set; }

        public TipoEscalaFuncionario TipoEscala { get; set; }

        //Controle de Tela
        public string Cpf { get; set; }

        public FuncionarioViewModel()
        {
            Pessoa = new PessoaViewModel();
            //Supervisor = new FuncionarioViewModel();
        }

        public FuncionarioViewModel(Funcionario funcionario)
        {
            Id = funcionario.Id;
            Cpf = funcionario?.Pessoa?.DocumentoCpf;
            DataInsercao = funcionario.Pessoa != null ? funcionario.Pessoa.DataInsercao : DateTime.Now;
            Cargo = funcionario?.Cargo == null ? new CargoViewModel() : new CargoViewModel(funcionario?.Cargo ?? new Cargo());
            Pessoa = new PessoaViewModel
            {
                Id = funcionario?.Pessoa?.Id ?? 0,
                Nome = funcionario?.Pessoa?.Nome,
                Documentos = funcionario?.Pessoa?.Documentos == null || (funcionario?.Pessoa?.Documentos?.Any() ?? false) ? new List<DocumentoViewModel>() : funcionario?.Pessoa?.Documentos?.Select(d => new DocumentoViewModel(d.Documento))?.ToList(),
                Contatos = funcionario?.Pessoa?.Contatos == null || (funcionario?.Pessoa?.Contatos?.Any() ?? false) ? new List<ContatoViewModel>() : funcionario?.Pessoa?.Contatos?.Select(x => new ContatoViewModel(x.Contato))?.ToList()
            };
            Supervisor = funcionario?.Supervisor == null ? new FuncionarioViewModel() : ToViewModel(funcionario?.Supervisor ?? new Funcionario());
            Unidade = new UnidadeViewModel
            {
                Id = funcionario?.Unidade?.Id ?? 0,
                CNPJ = funcionario?.Unidade?.CNPJ,
                DataInsercao = funcionario?.Unidade?.DataInsercao ?? DateTime.Now,
                Codigo = funcionario?.Unidade?.Codigo,
                Nome = funcionario?.Unidade?.Nome,
                NumeroVaga = funcionario?.Unidade?.NumeroVaga ?? 0,
                TiposUnidades = funcionario?.Unidade?.TiposUnidades ?? 0,
                HorarioInicial = funcionario?.Unidade?.HorarioInicial,
                HorarioFinal = funcionario?.Unidade?.HorarioFinal
            };
            Status = funcionario.Status;
            Salario = funcionario.Salario;
            DataAdmissao = funcionario.DataAdmissao;
            Imagem = funcionario.Imagem != null && funcionario.Imagem.Any()
                ? $"data:image/jpg;base64,{Convert.ToBase64String(funcionario.Imagem)}"
                : "../../Content/img/avatars/sunny-big.png";
            ItemFuncionario = funcionario?.ItemFuncionario == null ? new ItemFuncionarioViewModel() : AutoMapper.Mapper.Map<ItemFuncionarioViewModel>(funcionario.ItemFuncionario);
            BeneficioFuncionario = funcionario?.BeneficioFuncionario == null ? new BeneficioFuncionarioViewModel() : AutoMapper.Mapper.Map<BeneficioFuncionarioViewModel>(funcionario.BeneficioFuncionario);
            OcorrenciaFuncionario = funcionario?.OcorrenciaFuncionario == null ? new OcorrenciaFuncionarioViewModel() : AutoMapper.Mapper.Map<OcorrenciaFuncionarioViewModel>(funcionario.OcorrenciaFuncionario);
            ControlesFerias = funcionario?.ControlesFerias == null || (!funcionario?.ControlesFerias?.Any() ?? false) ? new List<ControleFeriasViewModel>() : AutoMapper.Mapper.Map<List<ControleFeriasViewModel>>(funcionario.ControlesFerias);
            ListaIntervaloDozeTrintaSeis = funcionario?.ListaIntervaloDozeTrintaSeis == null || (!funcionario?.ListaIntervaloDozeTrintaSeis?.Any() ?? false) ? new List<FuncionarioIntervaloDozeTrintaSeisViewModel>() : AutoMapper.Mapper.Map<List<FuncionarioIntervaloDozeTrintaSeisViewModel>>(funcionario.ListaIntervaloDozeTrintaSeis);
            ListaIntervaloCompensacao = funcionario?.ListaIntervaloCompensacao == null || (!funcionario?.ListaIntervaloCompensacao?.Any() ?? false) ? new List<FuncionarioIntervaloCompensacaoViewModel>() : AutoMapper.Mapper.Map<List<FuncionarioIntervaloCompensacaoViewModel>>(funcionario.ListaIntervaloCompensacao);
            ListaIntervaloNoturno = funcionario?.ListaIntervaloNoturno == null || (!funcionario?.ListaIntervaloNoturno?.Any() ?? false) ? new List<FuncionarioIntervaloNoturnoViewModel>() : AutoMapper.Mapper.Map<List<FuncionarioIntervaloNoturnoViewModel>>(funcionario.ListaIntervaloNoturno);
            TipoEscala = funcionario.TipoEscala;
        }

        public Funcionario ToEntity(Pessoa pessoa = null, Funcionario supervisor = null)
        {
            var pessoaEntity = Pessoa?.ToEntity();
            var funcionario = new Funcionario();

            funcionario.Id = Id;
            funcionario.DataInsercao = DateTime.Now;
            funcionario.Salario = Salario;
            funcionario.Imagem = !string.IsNullOrEmpty(Imagem) && Imagem.Contains("base64")
                ? Convert.FromBase64String(Imagem.Substring(Imagem.IndexOf("base64,") + 7))
                : null;

            funcionario.Cargo = Cargo?.Id == 0 ? null : Cargo?.ToEntity();
            funcionario.Unidade = Unidade?.ToEntity();
            funcionario.Status = Status;

            if (supervisor != null)
                funcionario.Supervisor = supervisor;
            else
                funcionario.Supervisor = Supervisor?.Id == 0 ? null : Supervisor?.ToEntity();

            funcionario.Pessoa = pessoa ?? pessoaEntity;
            funcionario.Pessoa.Contatos = pessoaEntity?.Contatos;
            funcionario.Pessoa.Nome = Pessoa.Nome;
            funcionario.Pessoa.DocumentoCpf = Cpf;
            funcionario.DataAdmissao = DataAdmissao;

            funcionario.ItemFuncionario = ItemFuncionario?.ItemFuncionarioId > 0 || ItemFuncionario?.ItemFuncionariosDetalhes?.Count > 0 ?
                                            AutoMapper.Mapper.Map<ItemFuncionario>(ItemFuncionario) :
                                            null;

            if (funcionario.ItemFuncionario != null && funcionario.ItemFuncionario.ItemFuncionariosDetalhes.Count == 0)
                funcionario.ItemFuncionario.Funcionario = null;

            funcionario.BeneficioFuncionario = BeneficioFuncionario?.BeneficioFuncionarioId > 0 || BeneficioFuncionario?.BeneficioFuncionarioDetalhes?.Count > 0 ?
                                                    AutoMapper.Mapper.Map<BeneficioFuncionario>(BeneficioFuncionario) :
                                                    null;

            if (funcionario.BeneficioFuncionario != null && funcionario.BeneficioFuncionario.BeneficioFuncionarioDetalhes.Count == 0)
                funcionario.BeneficioFuncionario.Funcionario = null;

            funcionario.OcorrenciaFuncionario = OcorrenciaFuncionario?.OcorrenciaFuncionarioId > 0 || OcorrenciaFuncionario?.OcorrenciaFuncionarioDetalhes?.Count > 0 ?
                                                    AutoMapper.Mapper.Map<OcorrenciaFuncionario>(OcorrenciaFuncionario) :
                                                    null;

            if (funcionario.OcorrenciaFuncionario != null && funcionario.OcorrenciaFuncionario.OcorrenciaFuncionarioDetalhes.Count == 0)
                funcionario.OcorrenciaFuncionario.Funcionario = null;

            funcionario.ControlesFerias = AutoMapper.Mapper.Map<List<ControleFerias>>(ControlesFerias);

            funcionario.ListaIntervaloDozeTrintaSeis = AutoMapper.Mapper.Map<List<FuncionarioIntervaloDozeTrintaSeis>>(funcionario.ListaIntervaloDozeTrintaSeis) ?? new List<FuncionarioIntervaloDozeTrintaSeis>();
            funcionario.ListaIntervaloCompensacao = AutoMapper.Mapper.Map<List<FuncionarioIntervaloCompensacao>>(funcionario.ListaIntervaloCompensacao) ?? new List<FuncionarioIntervaloCompensacao>();
            funcionario.ListaIntervaloNoturno = AutoMapper.Mapper.Map<List<FuncionarioIntervaloNoturno>>(funcionario.ListaIntervaloNoturno) ?? new List<FuncionarioIntervaloNoturno>();
            funcionario.TipoEscala = TipoEscala;

            return funcionario;
        }

        public FuncionarioViewModel ToViewModel(Funcionario funcionario)
        {
            return new FuncionarioViewModel
            {
                Id = funcionario.Id,
                DataInsercao = funcionario.Pessoa != null ? funcionario.Pessoa.DataInsercao : DateTime.Now,
                Cargo = funcionario?.Cargo == null ? new CargoViewModel() : new CargoViewModel(funcionario?.Cargo),
                Pessoa = new PessoaViewModel
                {
                    Id = funcionario?.Pessoa?.Id ?? 0,
                    Nome = funcionario?.Pessoa?.Nome,
                    Documentos = funcionario?.Pessoa?.Documentos == null || (funcionario?.Pessoa?.Documentos?.Any() ?? false) ? new List<DocumentoViewModel>() : funcionario?.Pessoa?.Documentos?.Select(d => new DocumentoViewModel(d.Documento))?.ToList(),
                    Contatos = funcionario?.Pessoa?.Contatos == null || (funcionario?.Pessoa?.Contatos?.Any() ?? false) ? new List<ContatoViewModel>() : funcionario?.Pessoa?.Contatos?.Select(x => new ContatoViewModel(x.Contato))?.ToList()
                },
                Salario = funcionario.Salario,
                Unidade = funcionario?.Unidade == null ? new UnidadeViewModel() : new UnidadeViewModel(funcionario?.Unidade),
                Status = Status,
                Cpf = DocumentoCpf,
                Imagem = funcionario.Imagem != null && funcionario.Imagem.Any()
                        ? $"data:image/jpg;base64,{Convert.ToBase64String(funcionario.Imagem)}"
                        : "../../Content/img/avatars/sunny-big.png",

                Email = Pessoa?.ContatoEmail?.ToString(),
                Telefone = Pessoa?.ContatoComercial?.ToString(),
                Celular = Pessoa?.ContatoCel?.ToString(),
                DataAdmissao = funcionario.DataAdmissao,
                ItemFuncionario = funcionario?.ItemFuncionario == null ? new ItemFuncionarioViewModel() : AutoMapper.Mapper.Map<ItemFuncionarioViewModel>(funcionario.ItemFuncionario),
                BeneficioFuncionario = funcionario?.BeneficioFuncionario == null ? new BeneficioFuncionarioViewModel() : AutoMapper.Mapper.Map<BeneficioFuncionarioViewModel>(funcionario.BeneficioFuncionario),
                OcorrenciaFuncionario = funcionario?.OcorrenciaFuncionario == null ? new OcorrenciaFuncionarioViewModel() : AutoMapper.Mapper.Map<OcorrenciaFuncionarioViewModel>(funcionario.OcorrenciaFuncionario),
                ControlesFerias = funcionario?.ControlesFerias == null || (!funcionario?.ControlesFerias?.Any() ?? false) ? new List<ControleFeriasViewModel>() : AutoMapper.Mapper.Map<List<ControleFeriasViewModel>>(funcionario.ControlesFerias),
                ListaIntervaloDozeTrintaSeis = funcionario?.ListaIntervaloDozeTrintaSeis == null || (!funcionario?.ListaIntervaloDozeTrintaSeis?.Any() ?? false) ? new List<FuncionarioIntervaloDozeTrintaSeisViewModel>() : AutoMapper.Mapper.Map<List<FuncionarioIntervaloDozeTrintaSeisViewModel>>(funcionario.ListaIntervaloDozeTrintaSeis),
                ListaIntervaloCompensacao = funcionario?.ListaIntervaloCompensacao == null || (!funcionario?.ListaIntervaloCompensacao?.Any() ?? false) ? new List<FuncionarioIntervaloCompensacaoViewModel>() : AutoMapper.Mapper.Map<List<FuncionarioIntervaloCompensacaoViewModel>>(funcionario.ListaIntervaloCompensacao),
                ListaIntervaloNoturno = funcionario?.ListaIntervaloNoturno == null || (!funcionario?.ListaIntervaloNoturno?.Any() ?? false) ? new List<FuncionarioIntervaloNoturnoViewModel>() : AutoMapper.Mapper.Map<List<FuncionarioIntervaloNoturnoViewModel>>(funcionario.ListaIntervaloNoturno),
                TipoEscala = funcionario.TipoEscala
            };
        }
    }
}