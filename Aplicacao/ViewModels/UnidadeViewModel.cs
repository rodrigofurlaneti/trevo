using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class UnidadeViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int NumeroVaga { get; set; }
        public int DiaVencimento { get; set; }
        public TiposUnidades TiposUnidades { get; set; }
        public FuncionarioViewModel Responsavel { get; set; }
        public EmpresaViewModel Empresa { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public MaquinaCartaoViewModel MaquinaCartao { get; set; }
        public IList<TipoPagamentoViewModel> TiposPagamento { get; set; }
        public IList<EstruturaUnidadeViewModel> EstruturasUnidade { get; set; }
        public IList<UnidadeCheckListAtividadeViewModel> UnidadeCheckListAtividades { get; set; }
        public IList<UnidadeFuncionarioViewModel> UnidadeFuncionarios { get; set; }
        public List<int> IdsTipoPagamento { get; set; }
        public string IdResponsavel { get; set; }
        public IList<CheckListEstruturaUnidade> CheckListEstruturaUnidade { get; set; }
        public CheckListAtividade CheckListAtividade { get; set; }
        public string CNPJ { get; set; }
        public string CCM { get; set; }
        public string HorarioInicial { get; set; }
        public string HorarioFinal { get; set; }
        public bool Ativa { get; set; }

        public virtual IList<UnidadeCheckListAtividadeTipoAtividadeViewModel> UnidadeCheckListTipoAtividades { get; set; }

        public UnidadeViewModel(Unidade unidade)
        {
            if (unidade != null)
            {
                Id = unidade.Id;
                Codigo = unidade.Codigo;
                Nome = unidade.Nome;
                DataInsercao = DateTime.Now;
                DiaVencimento = unidade.DiaVencimento;
                if (unidade.Responsavel != null)
                {
                    Responsavel = new FuncionarioViewModel
                    {
                        Id = unidade?.Responsavel?.Id ?? 0,
                        Cargo = new CargoViewModel(unidade?.Responsavel?.Cargo),
                        Salario = unidade?.Responsavel?.Salario == null ? string.Empty : unidade.Responsavel.Salario.ToString(),
                        DataInsercao = unidade?.Responsavel?.DataInsercao ?? DateTime.Now,
                        Pessoa = new PessoaViewModel
                        {
                            Id = unidade?.Responsavel?.Pessoa?.Id ?? 0,
                            Nome = unidade?.Responsavel?.Pessoa?.Nome,
                            Documentos = unidade?.Responsavel?.Pessoa?.Documentos?.Select(d => new DocumentoViewModel(d.Documento))?.ToList()
                        }
                    };
                }
                Endereco = new EnderecoViewModel(unidade.Endereco);

                CNPJ = unidade.CNPJ;
                CCM = unidade.CCM;
                CheckListAtividade = unidade.CheckListAtividade;
                CheckListEstruturaUnidade = unidade.CheckListEstruturaUnidade;
                HorarioInicial = unidade.HorarioInicial;
                HorarioFinal = unidade.HorarioFinal;
                Ativa = unidade.Ativa;
            }
        }

        public UnidadeViewModel()
        {
            Empresa = new EmpresaViewModel();
            Responsavel = new FuncionarioViewModel();
        }

        public Unidade ToEntity()
        {
            return new Unidade
            {
                Id = Id,
                Codigo = Codigo,
                Nome = Nome,
                DataInsercao = DataInsercao,
                DiaVencimento = DiaVencimento,
                Endereco = Endereco?.ToEntity(),
                MaquinaCartao = MaquinaCartao?.ToEntity(),
                Empresa = AutoMapper.Mapper.Map<EmpresaViewModel, Empresa>(Empresa),
                EstruturasUnidade = EstruturasUnidade?.Select(x => x.ToEntity()).ToList() ?? new List<EstruturaUnidade>(),
                UnidadeCheckListAtividades = UnidadeCheckListAtividades?.Select(x => x.ToEntity()).ToList() ?? new List<UnidadeCheckListAtividade>(),
                UnidadeFuncionarios = UnidadeFuncionarios?.Select(x => x.ToEntity()).ToList() ?? new List<UnidadeFuncionario>(),
                CheckListEstruturaUnidade = CheckListEstruturaUnidade,
                CheckListAtividade = CheckListAtividade,
                CNPJ = CNPJ,
                CCM = CCM,
                Responsavel = Responsavel?.ToEntity(),
                HorarioInicial = HorarioInicial,
                HorarioFinal = HorarioFinal,
                Ativa = Ativa
            };
        }

        public UnidadeViewModel ToViewModel(Unidade unidade)
        {
            return new UnidadeViewModel
            {
                Id = unidade.Id,
                Codigo = unidade.Codigo,
                Nome = unidade.Nome,
                DataInsercao = unidade.DataInsercao,
                DiaVencimento = unidade.DiaVencimento,
                Endereco = new EnderecoViewModel(unidade?.Endereco ?? new Endereco()),
                MaquinaCartao = unidade?.MaquinaCartao == null ? null : new MaquinaCartaoViewModel(unidade?.MaquinaCartao),
                EstruturasUnidade = unidade.EstruturasUnidade?.Select(x => new EstruturaUnidadeViewModel(x)).ToList() ?? new List<EstruturaUnidadeViewModel>(),
                UnidadeCheckListAtividades = unidade.UnidadeCheckListAtividades?.Select(x => new UnidadeCheckListAtividadeViewModel(x)).ToList() ?? new List<UnidadeCheckListAtividadeViewModel>(),
                UnidadeFuncionarios = unidade.UnidadeFuncionarios?.Select(x => new UnidadeFuncionarioViewModel(x)).ToList() ?? new List<UnidadeFuncionarioViewModel>(),
                CheckListEstruturaUnidade = unidade.CheckListEstruturaUnidade,
                CheckListAtividade = unidade.CheckListAtividade,
                CNPJ = CNPJ,
                CCM = CCM,
                HorarioInicial = unidade.HorarioInicial,
                HorarioFinal = unidade.HorarioFinal,
                Ativa = unidade.Ativa
            };

        }

        public string NomeSupervisor
        {
            get
            {
                return Responsavel?.Pessoa?.Nome ?? string.Empty;
            }
        }
    }
}