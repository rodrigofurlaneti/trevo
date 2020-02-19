using Aplicacao.Base;
using Aplicacao.ViewModels;
using Core.Extensions;
using Core.Helpers;
using Dominio;
using Entidade;
using Microsoft.Practices.ServiceLocation;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aplicacao
{
    public interface IPropostaAplicacao : IBaseAplicacao<Proposta>
    {
        Stream GerarPdfStream(PedidoSelo pedido, ControllerContext controllerContext);
        byte[] GerarPdfByte(PedidoSelo pedido, ControllerContext controllerContext);
        string BuscarTelefoneHelper(int idEmpresa);
        string BuscarEnderecoHelper(int idFilial);
        string BuscarHorarioFuncionamentoHelper(int idFilial);
        List<string> BuscarEmailsHelper(int idEmpresa);
        int RetornaNumeroPropostaDisponivel();
        List<PropostaGridViewModel> PopularGrid();
        IList<Proposta> BuscarPorClienteUnidade(int idCliente, int idUnidade);
        void Salvar(PropostaViewModel proposta);
        PropostaViewModel PrepararViewModelEdicao(int id);

        IList<ClienteViewModel> BuscarClientes(string nome);
        IList<UnidadeViewModel> ListaUnidade();
    }

    public class PropostaAplicacao : BaseAplicacao<Proposta, IPropostaServico>, IPropostaAplicacao
    {
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IPropostaServico _propostaServico;
        private readonly IPedidoSeloServico _pedidoSeloServico;

        public PropostaAplicacao(
            IClienteAplicacao clienteAplicacao,
            IUnidadeAplicacao unidadeAplicacao,
            IPropostaServico propostaServico,
            IPedidoSeloServico pedidoSeloServico)
        {   
            _clienteAplicacao = clienteAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _propostaServico = propostaServico;
            _pedidoSeloServico = pedidoSeloServico;
        }

        public Stream GerarPdfStream(PedidoSelo pedido, ControllerContext controllerContext)
        {
            return new MemoryStream(GerarPdfByte(pedido, controllerContext));
        }

        public byte[] GerarPdfByte(PedidoSelo pedido, ControllerContext controllerContext)
        {
            var pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), "A4", true);
            var pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), "Portrait", true);

            var converter = new HtmlToPdf();
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

            var valorTotal = _pedidoSeloServico.RetornaValorSeloComDesconto(pedido) * pedido.Quantidade;

            var contexto = HttpContext.Current;
            var viewModel = new PropostaPdfViewModel
            {
                Empresa = RetornaNomeCliente(pedido.Proposta),
                Telefone = BuscarTelefoneHelper(pedido.Proposta.Cliente.Id),
                Filial = pedido.Proposta.Unidade.Nome,
                Endereco = BuscarEnderecoHelper(pedido.Unidade.Id),
                HorarioFuncionamento = BuscarHorarioFuncionamentoHelper(pedido.Unidade.Id),
                CaminhoImagemLogo = $"{contexto.Request.Url.Scheme}://{contexto.Request.Url.Authority}/content/img/logo.png",
                CaminhoImagemTicket = $"{contexto.Request.Url.Scheme}://{contexto.Request.Url.Authority}/content/img/ticket.png",
                PossuiValidade = pedido.TipoSelo.ComValidade,
                TipoPagamento = pedido.TiposPagamento,
                Quantidade = pedido.Quantidade.ToString(),
                ValorTotal = valorTotal.ToString("N2"),
                Periodo = pedido.TipoSelo.Nome
            };

            var html = RazorHelper.RenderRazorViewToString(controllerContext, "_Pdf", viewModel);
            var doc = converter.ConvertHtmlString(html);
            var pdf = doc.Save();
            doc.Close();

            return pdf;
        }

        public string BuscarTelefoneHelper(int idEmpresa)
        {
            var cliente = _clienteAplicacao.BuscarPorId(idEmpresa);
            if (cliente?.Pessoa?.Contatos == null || !cliente.Pessoa.Contatos.Any(x => !string.IsNullOrEmpty(x?.Contato?.Numero)))
                return "";

            return cliente.Pessoa.Contatos
                .Where(x => !string.IsNullOrEmpty(x.Contato.Numero))
                .FirstOrDefault()
                .Contato.Numero;
        }

        public string BuscarEnderecoHelper(int idFilial)
        {
            var unidade = _unidadeAplicacao.BuscarPorId(idFilial);
            if (unidade?.Endereco == null || string.IsNullOrEmpty(unidade.Endereco.Logradouro) || string.IsNullOrEmpty(unidade.Endereco.Numero))
                return "";

            return $"{unidade.Endereco.Logradouro}, {unidade.Endereco.Numero}";
        }

        public string BuscarHorarioFuncionamentoHelper(int idFilial)
        {
            var unidade = _unidadeAplicacao.BuscarPorId(idFilial);
            if (unidade == null || string.IsNullOrEmpty(unidade?.HorarioInicial?.Trim()) || string.IsNullOrEmpty(unidade?.HorarioFinal?.Trim()))
                return "";

            return $"{unidade.HorarioInicial.Trim()} - {unidade.HorarioFinal.Trim()}";
        }

        public List<string> BuscarEmailsHelper(int idEmpresa)
        {
            var cliente = _clienteAplicacao.BuscarPorId(idEmpresa);
            if (cliente?.Pessoa?.Contatos != null && !cliente.Pessoa.Contatos.Any(x => !string.IsNullOrEmpty(x?.Contato?.Email)))
                return null;

            return cliente.Pessoa.Contatos
                .Where(x => !string.IsNullOrEmpty(x?.Contato?.Email))
                .Select(x => x.Contato.Email)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        public int RetornaNumeroPropostaDisponivel()
        {
            return _propostaServico.RetornaNumeroPropostaDisponivel();
        }

        public List<PropostaGridViewModel> PopularGrid()
        {
            var lista = new List<PropostaGridViewModel>();

            var propostas = Buscar().ToList();
            if (!propostas.Any())
                return lista;

            var pedidos = _pedidoSeloServico.Buscar();

            if (!pedidos.Any())
            {
                lista.AddRange(propostas.Select(x => new PropostaGridViewModel
                {
                    Id = x.Id,
                    EmailEnviado = "",
                    Empresa = RetornaNomeCliente(x),
                    Filial = x.Unidade.Nome,
                    Pedido = "",
                    StatusPedido = "",
                    TemPedido = false
                }).ToList());

                return lista;
            }

            var emailsPedidoEnviado = _pedidoSeloServico.EmailForamEnviados(pedidos.Select(x => x.Id).ToList());

            foreach (var proposta in propostas)
            {
                var propostaPossuiPedido = false;

                foreach (var pedido in pedidos.Where(x => x?.Proposta != null && x.Proposta.Id == proposta.Id))
                {
                    propostaPossuiPedido = true;
                    var emailFoiEnviado = emailsPedidoEnviado.FirstOrDefault(x => x.Key == pedido.Id).Value;
                    lista.Add(new PropostaGridViewModel
                    {
                        Id = pedido.Proposta.Id,
                        EmailEnviado = emailFoiEnviado ? "Sim" : "Não",
                        Empresa = RetornaNomeCliente(proposta),
                        Filial = pedido.Proposta.Unidade.Nome,
                        Pedido = pedido.Id.ToString(),
                        StatusPedido = pedido.StatusPedido.ToDescription(),
                        TemPedido = true
                    });
                }

                if (!propostaPossuiPedido)
                {
                    lista.Add(new PropostaGridViewModel
                    {
                        Id = proposta.Id,
                        EmailEnviado = "Não",
                        Empresa = RetornaNomeCliente(proposta),
                        Filial = proposta.Unidade.Nome,
                        Pedido = "",
                        StatusPedido = "",
                        TemPedido = false
                    });
                }
            }

            return lista;
        }

        public IList<Proposta> BuscarPorClienteUnidade(int idCliente, int idUnidade)
        {
            return _propostaServico.BuscarPorClienteUnidade(idCliente, idUnidade);
        }

        public void Salvar(PropostaViewModel proposta)
        {
            var entidade = proposta.ToEntity();
            _propostaServico.Salvar(entidade);
        }

        public IList<ClienteViewModel> BuscarClientes(string cliente)
        {
            return _clienteAplicacao.Buscar().Select(x => new ClienteViewModel(x)).ToList();
        }

        public IList<UnidadeViewModel> ListaUnidade()
        {
            return _unidadeAplicacao.Buscar().Select(x => new UnidadeViewModel(x)).ToList();
        }

        public PropostaViewModel PrepararViewModelEdicao(int id)
        {
            var viewModel = new PropostaViewModel(_propostaServico.BuscarPorId(id));
            viewModel.Telefone = BuscarTelefoneHelper(viewModel.Empresa.Id);
            viewModel.Endereco = BuscarEnderecoHelper(viewModel.Filial.Id);
            viewModel.HorarioFuncionamento = BuscarHorarioFuncionamentoHelper(viewModel.Filial.Id);

            var pedido = _pedidoSeloServico.BuscarPor(x => x.Proposta.Id == id)?.FirstOrDefault();
            if (pedido != null)
                viewModel.TemPedido = true;

            return viewModel;
        }

        private string RetornaNomeCliente(Proposta proposta)
        {
            return string.IsNullOrEmpty(proposta.Cliente.Pessoa.Nome) ? proposta.Cliente.NomeFantasia : proposta.Cliente.Pessoa.Nome;
        }
    }
}