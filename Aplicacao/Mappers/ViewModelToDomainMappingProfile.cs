using Aplicacao.ViewModels;
using AutoMapper;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        // Não realizar este override na versão 4.x e superiores
        public override string ProfileName => "ViewModelToDomainMappings";

        public ViewModelToDomainMappingProfile()
        {
            CreateMap<PessoaViewModel, Pessoa>().MaxDepth(1);
            CreateMap<EmpresaViewModel, Empresa>().MaxDepth(1);
            CreateMap<EnderecoViewModel, Endereco>().MaxDepth(1);
            CreateMap<DocumentoViewModel, Documento>().MaxDepth(1);
            CreateMap<ContatoViewModel, Contato>().MaxDepth(1);
            CreateMap<GrupoViewModel, Grupo>().MaxDepth(1);
            CreateMap<UsuarioViewModel, Usuario>().MaxDepth(1);
            CreateMap<PerfilViewModel, Perfil>().ForMember(x => x.Usuarios, opt => opt.MapFrom(o => o.Usuarios)).MaxDepth(1);
            CreateMap<PermissaoViewModel, Permissao>().ForMember(x => x.Perfis, opt => opt.MapFrom(o => o.Perfis)).MaxDepth(1);
            CreateMap<UsuarioPerfilViewModel, UsuarioPerfil>().MaxDepth(1);
            CreateMap<PerfilMenuViewModel, PerfilMenu>().MaxDepth(1);
            CreateMap<LojaContatoViewModel, EmpresaContato>().MaxDepth(1);
            CreateMap<PessoaContatoViewModel, PessoaContato>().MaxDepth(1);
            CreateMap<PessoaLojaViewModel, PessoaLoja>().MaxDepth(1);
            CreateMap<MenuViewModel, Menu>().ForMember(x => x.MenuPai, opt => opt.MapFrom(o => o.MenuPai)).MaxDepth(1);
            CreateMap<CidadeViewModel, Cidade>();
            CreateMap<EstadoViewModel, Estado>();
            CreateMap<PaisViewModel, Pais>();
            CreateMap<RegionalViewModel, Regional>().MaxDepth(1);
            CreateMap<RegionalEstadoViewModel, RegionalEstado>().MaxDepth(1);
            CreateMap<ArquivoViewModel, Arquivo>().MaxDepth(1);
            CreateMap<TipoFilialViewModel, TipoFilial>().MaxDepth(1);
            CreateMap<FilialViewModel, Filial>().MaxDepth(1);
            CreateMap<FornecedorViewModel, Fornecedor>().MaxDepth(1);
            CreateMap<TipoMensalistaViewModel, TipoMensalista>().MaxDepth(1);

            CreateMap<SelecaoDespesaViewModel, SelecaoDespesa>().MaxDepth(1);
            CreateMap<DespesaContasAPagarViewModel, DespesaContasAPagar>().MaxDepth(1);

            CreateMap<PagamentoViewModel, Pagamento>().MaxDepth(1);
            CreateMap<ClienteViewModel, Cliente>().MaxDepth(1);
            CreateMap<ContaFinanceiraViewModel, ContaFinanceira>().MaxDepth(1);
            CreateMap<BancoViewModel, Banco>().MaxDepth(1);
            CreateMap<LancamentoCobrancaViewModel, LancamentoCobranca>()
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente != null && src.Cliente.Id > 0 ? src.Cliente : null))
                .MaxDepth(1);
            CreateMap<RecebimentoViewModel, Recebimento>().MaxDepth(1);


            CreateMap<HorarioUnidadeViewModel, HorarioUnidade>().MaxDepth(1);
            CreateMap<HorarioPrecoViewModel, HorarioPreco>().MaxDepth(1);
            CreateMap<PrecoParametroSeloViewModel, PrecoParametroSelo>().MaxDepth(1);
            
            CreateMap<PessoaDocumentoViewModel, PessoaDocumento>().MaxDepth(1);
            CreateMap<PessoaEnderecoViewModel, PessoaEndereco>().MaxDepth(1);

            CreateMap<NotificacaoViewModel, Notificacao>().MaxDepth(1);
            CreateMap<ParametroEquipeNotificacaoViewModel, ParametroEquipeNotificacao>().MaxDepth(1);
            CreateMap<HorarioUnidadeNotificacaoViewModel, HorarioUnidadeNotificacao>().MaxDepth(1);
            CreateMap<NegociacaoSeloDescontoNotificacaoViewModel, NegociacaoSeloDescontoNotificacao>().MaxDepth(1);
            CreateMap<PedidoSeloNotificacaoViewModel, PedidoSeloNotificacao>().MaxDepth(1);
            CreateMap<TipoNotificacaoViewModel, TipoNotificacao>().MaxDepth(1);
            CreateMap<ParametroNotificacaoViewModel, ParametroNotificacao>().MaxDepth(1);

            CreateMap<PedidoLocacaoViewModel, PedidoLocacao>().MaxDepth(1);

            CreateMap<MaterialViewModel, Material>()
                    .ForMember(dest => dest.Imagem, opt =>
                        opt.MapFrom(src =>
                            !string.IsNullOrEmpty(src.Imagem) && src.Imagem.Contains("base64")
                            ? Convert.FromBase64String(src.Imagem.Substring(src.Imagem.IndexOf("base64,") + 7))
                            : null
                        )
                    )
                    .MaxDepth(1);

            CreateMap<MaterialFornecedorViewModel, MaterialFornecedor>().MaxDepth(1);
            CreateMap<EstoqueMaterialViewModel, EstoqueMaterial>().MaxDepth(1);
            CreateMap<EstoqueManualViewModel, EstoqueManual>()
                .ForMember(dest => dest.Unidade, opt => opt.MapFrom(src => src.Unidade.Id > 0 ? src.Unidade : null))
                .ForMember(dest => dest.PedidoCompra, opt => opt.MapFrom(src => src.PedidoCompra.Id > 0 ? src.PedidoCompra : null))
                .MaxDepth(1);

            CreateMap<CotacaoViewModel, Cotacao>().MaxDepth(1);
            CreateMap<CotacaoMaterialFornecedorViewModel, CotacaoMaterialFornecedor>().MaxDepth(1);

            CreateMap<NecessidadeCompraViewModel, NecessidadeCompra>().MaxDepth(1);
            CreateMap<NecessidadeCompraMaterialFornecedorViewModel, NecessidadeCompraMaterialFornecedor>().MaxDepth(1);

            CreateMap<PedidoCompraViewModel, PedidoCompra>().MaxDepth(1);
            CreateMap<PedidoCompraCotacaoMaterialFornecedorViewModel, PedidoCompraCotacaoMaterialFornecedor>().MaxDepth(1);

            CreateMap<ContratoMensalistaViewModel, ContratoMensalista>().MaxDepth(1);

            CreateMap<UnidadeViewModel, Unidade>().MaxDepth(1);

            CreateMap<UnidadeCheckListAtividadeTipoAtividadeViewModel, UnidadeCheckListAtividadeTipoAtividade>().MaxDepth(1);

            CreateMap<MaquinaCartaoViewModel, MaquinaCartao>().MaxDepth(1);

            CreateMap<OISImagemViewModel, OISImagem>()
                    .ForMember(dest => dest.ImagemUpload, opt =>
                        opt.MapFrom(src =>
                            !string.IsNullOrEmpty(src.ImagemUpload) && src.ImagemUpload.Contains("base64")
                            ? Convert.FromBase64String(src.ImagemUpload.Substring(src.ImagemUpload.IndexOf("base64,") + 7))
                            : null
                        )
                    )
                    .MaxDepth(1);

            CreateMap<OISViewModel, OIS>()
                .ForMember(e => e.Marca, opt => opt.MapFrom(vm => vm.Marca.Id > 0 ? vm.Marca : null))
                .ForMember(e => e.Modelo, opt => opt.MapFrom(vm => vm.Modelo.Id > 0 ? vm.Modelo : null))
                .ForMember(e => e.Unidade, opt => opt.MapFrom(vm => vm.Unidade.Id > 0 ? vm.Unidade : null))
                .MaxDepth(1);

            CreateMap<ItemFuncionarioViewModel, ItemFuncionario>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemFuncionarioId))
                .ForMember(dest => dest.ResponsavelEntrega, opt => opt.MapFrom(src => src.ResponsavelEntrega.Id > 0 ? src.ResponsavelEntrega : null))
                .ForMember(dest => dest.ResponsavelDevolucao, opt => opt.MapFrom(src => src.ResponsavelDevolucao.Id > 0 ? src.ResponsavelDevolucao : null))
                .MaxDepth(1);

            CreateMap<BeneficioFuncionarioViewModel, BeneficioFuncionario>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BeneficioFuncionarioId))
                .MaxDepth(1);

            CreateMap<OcorrenciaFuncionarioViewModel, OcorrenciaFuncionario>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OcorrenciaFuncionarioId))
                .MaxDepth(1);

            CreateMap<ItemFuncionarioDetalheViewModel, ItemFuncionarioDetalhe>().MaxDepth(1);

            CreateMap<OficinaViewModel, Oficina>().MaxDepth(1);

            CreateMap<OrcamentoSinistroViewModel, OrcamentoSinistro>()
                .ForMember(dest => dest.OIS, opt => opt.MapFrom(src => src.OIS.Id > 0 ? src.OIS : null))
                .MaxDepth(1);

            CreateMap<OrcamentoSinistroCotacaoViewModel, OrcamentoSinistroCotacao>().MaxDepth(1);

            CreateMap<OrcamentoSinistroCotacaoItemViewModel, OrcamentoSinistroCotacaoItem>()
                .ForMember(dest => dest.DataServico, opt => opt.MapFrom(src => DateTime.Parse(src.DataServico)))
                .MaxDepth(1);

            CreateMap<OrcamentoSinistroCotacaoHistoricoDataItemViewModel, OrcamentoSinistroCotacaoHistoricoDataItem>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => DateTime.Parse(src.Data)))
                .MaxDepth(1);

            CreateMap<ControleCompraViewModel, ControleCompra>().MaxDepth(1);
        }
    }
}
