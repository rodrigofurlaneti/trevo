using Aplicacao.ViewModels;
using AutoMapper;
using Entidade;
using Entidade.Uteis;
using System;
using System.Linq;

namespace Aplicacao.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        // Não realizar este override na versão 4.x e superiores
        public override string ProfileName => "DomainToViewModelMappings";

        public DomainToViewModelMappingProfile()
        {
            CreateMap<Pessoa, PessoaViewModel>()
                .ForSourceMember(x => x.DocumentoCpf, opt => opt.Ignore())
                .MaxDepth(1);

            CreateMap<Empresa, EmpresaViewModel>().MaxDepth(1);
            CreateMap<Endereco, EnderecoViewModel>().MaxDepth(1);
            CreateMap<Documento, DocumentoViewModel>().MaxDepth(1);
            CreateMap<Contato, ContatoViewModel>().MaxDepth(1);
            CreateMap<Grupo, GrupoViewModel>().MaxDepth(1);
            CreateMap<Usuario, UsuarioViewModel>().MaxDepth(1);
            CreateMap<Perfil, PerfilViewModel>().ForMember(x => x.Usuarios, opt => opt.MapFrom(o => o.Usuarios)).MaxDepth(1);
            CreateMap<Permissao, PermissaoViewModel>().ForMember(x => x.Perfis, opt => opt.MapFrom(o => o.Perfis)).MaxDepth(1);
            CreateMap<UsuarioPerfil, UsuarioPerfilViewModel>().MaxDepth(1);
            CreateMap<PerfilMenu, PerfilMenuViewModel>().MaxDepth(1);
            CreateMap<EmpresaContato, LojaContatoViewModel>().MaxDepth(1);
            CreateMap<PessoaContato, PessoaContatoViewModel>().MaxDepth(1);
            CreateMap<PessoaLoja, PessoaLojaViewModel>().MaxDepth(1);
            CreateMap<Menu, MenuViewModel>().ForMember(x => x.MenuPai, opt => opt.MapFrom(o => o.MenuPai)).MaxDepth(1);
            CreateMap<Cidade, CidadeViewModel>();
            CreateMap<Estado, EstadoViewModel>();
            CreateMap<Pais, PaisViewModel>();
            CreateMap<Regional, RegionalViewModel>().MaxDepth(1);
            CreateMap<RegionalEstado, RegionalEstadoViewModel>().MaxDepth(1);
            CreateMap<Arquivo, ArquivoViewModel>().MaxDepth(1);
            CreateMap<TipoFilial, TipoFilialViewModel>().MaxDepth(1);
            CreateMap<Filial, FilialViewModel>().MaxDepth(1);
            CreateMap<Fornecedor, FornecedorViewModel>().MaxDepth(1);
            CreateMap<TipoMensalista, TipoMensalistaViewModel>().MaxDepth(1);
            CreateMap<SelecaoDespesa, SelecaoDespesaViewModel>().MaxDepth(1);
            CreateMap<DespesaContasAPagar, DespesaContasAPagarViewModel>().MaxDepth(1);
            CreateMap<Pagamento, PagamentoViewModel>().MaxDepth(1);
            CreateMap<Cliente, ClienteViewModel>().MaxDepth(1);
            CreateMap<ContaFinanceira, ContaFinanceiraViewModel>().MaxDepth(1);
            CreateMap<Banco, BancoViewModel>().MaxDepth(1);
            CreateMap<LancamentoCobranca, LancamentoCobrancaViewModel>().MaxDepth(1);
            CreateMap<Recebimento, RecebimentoViewModel>().MaxDepth(1);

            CreateMap<Recebimento, RecebimentoViewModel>()
                    .ForMember(
                        dest => dest.Pagamentos,
                        opt => opt.MapFrom(src => src.Pagamentos)).MaxDepth(1);

            CreateMap<Recebimento, RecebimentoViewModel>()
                    .ForMember(
                        dest => dest.LancamentosCobranca,
                        opt => opt.MapFrom(src => src.LancamentosCobranca)).MaxDepth(1);

            CreateMap<LancamentoCobranca, LancamentoCobrancaViewModel>()
                   .ForMember(
                       dest => dest.ContaFinanceira,
                       opt => opt.MapFrom(src => src.ContaFinanceira)).MaxDepth(1);

            CreateMap<LancamentoCobranca, LancamentoCobrancaViewModel>()
                    .ForMember(
                        dest => dest.Cliente,
                        opt => opt.MapFrom(src => src.Cliente)).MaxDepth(1);

            CreateMap<LancamentoCobranca, LancamentoCobrancaViewModel>()
                    .ForMember(
                        dest => dest.Unidade,
                        opt => opt.MapFrom(src => src.Unidade)).MaxDepth(1);

            CreateMap<ContaFinanceira, ContaFinanceiraViewModel>()
                    .ForMember(
                        dest => dest.Banco,
                        opt => opt.MapFrom(src => src.Banco)).MaxDepth(1);



            CreateMap<HorarioUnidade, HorarioPrecoViewModel>().MaxDepth(1);
            CreateMap<HorarioPreco, HorarioPrecoViewModel>().MaxDepth(1);
            CreateMap<PrecoParametroSelo, PrecoParametroSeloViewModel>().MaxDepth(1);

            CreateMap<PessoaDocumento, PessoaDocumentoViewModel>().MaxDepth(1);
            CreateMap<PessoaEndereco, PessoaEnderecoViewModel>().MaxDepth(1);

            CreateMap<Notificacao, NotificacaoViewModel>().MaxDepth(1);
            CreateMap<ParametroEquipeNotificacao, ParametroEquipeNotificacaoViewModel>().MaxDepth(1);
            CreateMap<HorarioUnidadeNotificacao, HorarioUnidadeNotificacaoViewModel>().MaxDepth(1);
            CreateMap<NegociacaoSeloDescontoNotificacao, NegociacaoSeloDescontoNotificacaoViewModel>().MaxDepth(1);
            CreateMap<PedidoSeloNotificacao, PedidoSeloNotificacaoViewModel>().MaxDepth(1);
            CreateMap<TipoNotificacao, TipoNotificacaoViewModel>().MaxDepth(1);
            CreateMap<ParametroNotificacao, ParametroNotificacaoViewModel>().MaxDepth(1);

            CreateMap<PedidoLocacao, PedidoLocacaoViewModel>()
                    .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor.ToString("N2")))
                    .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.ValorTotal.ToString("N2")))
                    .ForMember(dest => dest.ValorReajuste, opt => opt.MapFrom(src => src.ValorReajuste.ToString("N2")))
                    .ForMember(dest => dest.ValorPrimeiroPagamento, opt => opt.MapFrom(src => src.ValorPrimeiroPagamento.ToString("N2")))
                    .MaxDepth(1);

            CreateMap<PedidoLocacaoLancamentoAdicional, PedidoLocacaoLancamentoAdicionalViewModel>()
                    .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor.ToString("N2")))
                    .MaxDepth(1);

            CreateMap<Material, MaterialViewModel>()
                    .ForMember(dest => dest.Imagem, opt =>
                        opt.MapFrom(src =>
                            src.Imagem != null && src.Imagem.Any()
                            ? $"data:image/jpg;base64,{Convert.ToBase64String(src.Imagem)}"
                            : "../../Content/img/avatars/sunny-big.png"
                        )
                    )
                    .MaxDepth(1);

            CreateMap<MaterialFornecedor, MaterialFornecedorViewModel>().MaxDepth(1);

            CreateMap<EstoqueMaterial, EstoqueMaterialViewModel>()
                    .ForMember(dest => dest.Preco, opt => opt.MapFrom(src => src.Preco.ToString("N2")))
                    .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.ValorTotal.ToString("N2")))
                    .MaxDepth(1);

            CreateMap<EstoqueManual, EstoqueManualViewModel>()
                    .ForMember(dest => dest.Preco, opt => opt.MapFrom(src => src.Preco))
                    .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.ValorTotal.ToString("N2")))
                    .MaxDepth(1);

            CreateMap<Cotacao, CotacaoViewModel>().MaxDepth(1);
            CreateMap<CotacaoMaterialFornecedor, CotacaoMaterialFornecedorViewModel>()
                    .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor.ToString("N2")))
                    .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.ValorTotal.ToString("N2")))
                    .MaxDepth(1);


            CreateMap<NecessidadeCompra, NecessidadeCompraViewModel>().MaxDepth(1);
            CreateMap<NecessidadeCompraMaterialFornecedor, NecessidadeCompraMaterialFornecedorViewModel>().MaxDepth(1);

            CreateMap<PedidoCompra, PedidoCompraViewModel>().MaxDepth(1);
            CreateMap<PedidoCompraCotacaoMaterialFornecedor, PedidoCompraCotacaoMaterialFornecedorViewModel>().MaxDepth(1);

            CreateMap<ContratoMensalista, ContratoMensalistaViewModel>()
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor.ToString("N2")))
                .ForMember(dest => dest.ValorReais, opt => opt.MapFrom(src => src.Valor.ToString("C")))
                .MaxDepth(1);

            CreateMap<Unidade, UnidadeViewModel>().MaxDepth(1);

            CreateMap<UnidadeCheckListAtividadeTipoAtividade, UnidadeCheckListAtividadeTipoAtividadeViewModel>().MaxDepth(1);

            CreateMap<MaquinaCartao, MaquinaCartaoViewModel>().MaxDepth(1);

            CreateMap<OISImagem, OISImagemViewModel>()
                    .ForMember(dest => dest.ImagemUpload, opt =>
                        opt.MapFrom(src =>
                            src.ImagemUpload != null && src.ImagemUpload.Any()
                            ? $"data:image/jpg;base64,{Convert.ToBase64String(src.ImagemUpload)}"
                            : "../../Content/img/avatars/sunny-big.png"
                        )
                    )
                    .MaxDepth(1);

            CreateMap<OIS, OISViewModel>()
                .ForMember(dest => dest.TelefoneFixo, opt =>
                    opt.MapFrom(vm => vm.OISContatos != null && vm.OISContatos.Any() ?
                    vm.OISContatos.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Residencial).Contato.Numero : string.Empty))
                .ForMember(dest => dest.Celular, opt =>
                    opt.MapFrom(vm => vm.OISContatos != null && vm.OISContatos.Any() ?
                    vm.OISContatos.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Celular).Contato.Numero : string.Empty))
                .ForMember(dest => dest.Email, opt =>
                    opt.MapFrom(vm => vm.OISContatos != null && vm.OISContatos.Any() ?
                    vm.OISContatos.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Email).Contato.Email : string.Empty))
                .MaxDepth(1);

            CreateMap<ItemFuncionario, ItemFuncionarioViewModel>()
                .ForMember(dest => dest.ItemFuncionarioId, opt => opt.MapFrom(src => src.Id))
                .MaxDepth(1);

            CreateMap<ItemFuncionarioDetalhe, ItemFuncionarioDetalheViewModel>()
                    .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor.ToString("N2")))
                    .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.ValorTotal.ToString("N2")))
                    .MaxDepth(1);

            CreateMap<BeneficioFuncionario, BeneficioFuncionarioViewModel>()
                .ForMember(dest => dest.BeneficioFuncionarioId, opt => opt.MapFrom(src => src.Id))
                .MaxDepth(1);

            CreateMap<OcorrenciaFuncionario, OcorrenciaFuncionarioViewModel>()
                .ForMember(dest => dest.OcorrenciaFuncionarioId, opt => opt.MapFrom(src => src.Id))
                .MaxDepth(1);

            CreateMap<Oficina, OficinaViewModel>().MaxDepth(1);

            CreateMap<OrcamentoSinistro, OrcamentoSinistroViewModel>().MaxDepth(1);

            CreateMap<OrcamentoSinistroCotacao, OrcamentoSinistroCotacaoViewModel>().MaxDepth(1);

            CreateMap<OrcamentoSinistroCotacaoItem, OrcamentoSinistroCotacaoItemViewModel>()
                .ForMember(dest => dest.DataServico, opt => opt.MapFrom(src => src.DataServico.ToShortDateString()))
                .ForMember(dest => dest.ValorUnitario, opt => opt.MapFrom(src => src.ValorUnitario.ToString("N2")))
                .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.ValorTotal.ToString("N2")))
                .ForMember(dest => dest.ValorReembolso, opt => opt.MapFrom(src => src.ValorReembolso.ToString("N2")))
                .MaxDepth(1);

            CreateMap<OrcamentoSinistroCotacaoHistoricoDataItem, OrcamentoSinistroCotacaoHistoricoDataItemViewModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data.ToShortDateString()))
                .MaxDepth(1);

            CreateMap<ControleCompra, ControleCompraViewModel>().MaxDepth(1);

            CreateMap<ContasAPagarItem, ContasAPagarItemViewModel>()
                    .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor.ToString("N2")))
                    .MaxDepth(1);

            CreateMap<PlanoCarreira, PlanoCarreiraViewModel>()
                    .ForMember(dest => dest.AnoAte, opt => opt.MapFrom(src => src.AnoAte.ToString("N2")))
                    .ForMember(dest => dest.AnoDe, opt => opt.MapFrom(src => src.AnoDe.ToString("N2")))
                    .MaxDepth(1);
        }
    }
}
