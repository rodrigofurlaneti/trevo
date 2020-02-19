using API.Filters;
using Aplicacao;
using Entidade;
using Swagger.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Aplicacao.ViewModels;

namespace API.Controllers
{
    public class ClienteController : ControllerBase
    {
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IPessoaAplicacao _pessoaAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IEnderecoAplicacao _enderecoAplicacao;
        private readonly IVeiculoAplicacao _veiculoAplicacao;
        private readonly ICidadeAplicacao _cidadeAplicacao;
        private readonly IContratoMensalistaAplicacao _contratoMensalistaAplicacao;
        private readonly IClienteCondominoAplicacao _condominoAplicacao;
        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;
        private readonly IModeloAplicacao _modeloAplicacao;

        public ClienteController(
            IClienteAplicacao clienteAplicacao,
            IPessoaAplicacao pessoaAplicacao,
            IUnidadeAplicacao unidadeAplicacao, 
            IEnderecoAplicacao enderecoAplicacao, 
            IVeiculoAplicacao veiculoAplicacao,
            ICidadeAplicacao cidadeAplicacao,
            IContratoMensalistaAplicacao contratoMensalistaAplicacao, 
            IClienteCondominoAplicacao condominoAplicacao,
            ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao,
            IModeloAplicacao modeloAplicacao)
        {
            _clienteAplicacao = clienteAplicacao;
            _pessoaAplicacao = pessoaAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _enderecoAplicacao = enderecoAplicacao;
            _veiculoAplicacao = veiculoAplicacao;
            _cidadeAplicacao = cidadeAplicacao;
            _contratoMensalistaAplicacao = contratoMensalistaAplicacao;
            _condominoAplicacao = condominoAplicacao;
            _lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
            _modeloAplicacao = modeloAplicacao;
        }

        
        [HttpGet]
        [PerformanceFilter]
        [SwaggerOperation("GetAll")]
        [Route("api/v1/Clientes/GetAll")]
        [SwaggerResponse(HttpStatusCode.OK, "<b>Nota: </b>Obter todos os clientes", typeof(List<CondutorSoftparkViewModel>))]
        public HttpResponseMessage GetAll()
        {
            try
            {
                JsonResult.Status = true;
                var clientes = _clienteAplicacao.Buscar().ToList();

                var clienteIds = clientes.Select(x => x.Id).ToList();
                var clienteCpfs = clientes.Select(x => x.Pessoa.DocumentoCpf).ToList();

                var contratosMensalistas = _contratoMensalistaAplicacao
                                           .BuscarPor(x => clienteIds.Contains(x.Cliente.Id));

                var condominos = _condominoAplicacao.BuscarPor(x => clienteIds.Contains(x.Cliente.Id));
                
                var condutores = new List<CondutorSoftparkViewModel>();
                foreach (var cliente in clientes)
                {
                    var cpf = cliente.Pessoa.DocumentoCpf;
                    var clienteUnidadeIds = cliente.Unidades.Select(x => x.Unidade.Id).ToList();

                    var contratosMensalistasDoCliente = contratosMensalistas
                                           .Where(x => x.Cliente.Id == cliente.Id);

                    var contratosCondominoDoCliente = condominos
                       .Where(x => x.Cliente.Id == cliente.Id);

                    var condutor = new CondutorSoftparkViewModel(cliente, contratosCondominoDoCliente.ToList(), contratosMensalistasDoCliente.ToList());

                    condutores.Add(condutor);
                }

                JsonResult.Object = condutores;
                if (clientes.Count > 0) { JsonResult.Message = "Foram encontrados " + clientes.Count.ToString() + " clientes. "; } else { JsonResult.Message = "Não foram encontrados clientes."; };
                return Request.CreateResponse(HttpStatusCode.OK, JsonResult);
            }
            catch (Exception ex)
            {
                JsonResult.Status = false;
                JsonResult.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult);
            }
        }

        [HttpPost]
        [PerformanceFilter]
        [SwaggerOperation("Customers")]
        [Route("api/v1/Clientes/Customers")]
        [SwaggerResponse(HttpStatusCode.Created, "<b>Nota: </b>Cadastro de Cliente", typeof(CondutorSoftparkViewModel))]
        public HttpResponseMessage Customers([FromBody]CondutorSoftparkViewModel condutorViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    JsonResult.Message = String.Join(" ", ModelState.Values.SelectMany(value => value.Errors).Select(x => x.ErrorMessage));
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, JsonResult);
                }

                var clienteId = condutorViewModel.ERPId ?? 0;
                var clienteJaExitente = _clienteAplicacao.BuscarPorId(clienteId);

                var unidadesIds = condutorViewModel.Contratos.Select(x => x.Estacionamento).Select(x => x.Id).ToList();
                var unidades = _unidadeAplicacao.BuscarPor(x => unidadesIds.Contains(x.Id)).ToList();

                var menssagemErro = Validar(unidades, unidadesIds, clienteJaExitente, clienteId);
                if (menssagemErro != null)
                    return menssagemErro;

                var cidade = _cidadeAplicacao.PrimeiroPor(x => x.Descricao == condutorViewModel.Cidade);

                var cliente = condutorViewModel.ToCliente(cidade);
                cliente.Unidades = condutorViewModel.ToClienteUnidades(unidades);

                if (clienteJaExitente != null)
                {
                    cliente.Id = clienteJaExitente.Id;
                }

                //Veiculos
                var placas = cliente.Veiculos?.Select(x => x.Veiculo.Placa).ToList() ?? new List<string>();
                var veiculosExistentes = _veiculoAplicacao.BuscarPor(x => placas.Contains(x.Placa)).ToList();

                if (cliente.Veiculos != null)
                {
                    foreach (var clienteVeiculo in cliente.Veiculos)
                    {
                        var existente = veiculosExistentes.FirstOrDefault(x => x.Placa == clienteVeiculo.Veiculo.Placa);
                        if (existente != null)
                            clienteVeiculo.Veiculo.Id = existente.Id;

                        if (clienteVeiculo.Veiculo.Modelo != null)
                        {
                            var modelo = _modeloAplicacao.BuscarPor(x => x.Descricao == clienteVeiculo.Veiculo.Modelo.Descricao).FirstOrDefault();
                            if (modelo == null)
                            {
                                modelo = new Modelo { Descricao = clienteVeiculo.Veiculo.Modelo.Descricao, DataInsercao = DateTime.Now };
                                clienteVeiculo.Veiculo.Modelo = _modeloAplicacao.SalvarComRetorno(modelo);
                            }
                            else
                                clienteVeiculo.Veiculo.Modelo = modelo;
                        }
                    }
                }
                //FIM Veiculos

                _clienteAplicacao.SalvarClienteCondutor(cliente);

                cliente = _clienteAplicacao.BuscarPorId(cliente.Id);

                //Contratos
                var contratos = condutorViewModel.ToContratos(cliente, cliente.Veiculos?.Select(x => x.Veiculo).ToList());
                var contratosNumeros = contratos?.Select(x => x.NumeroContrato).ToList() ?? new List<int>(); ;
                var contratosExistentes = _contratoMensalistaAplicacao.BuscarPor(x => contratosNumeros.Contains(x.NumeroContrato)).ToList();

                if (contratos != null)
                {
                    foreach (var contrato in contratos)
                    {
                        var existente = contratosExistentes?.FirstOrDefault(x => x.NumeroContrato == contrato.NumeroContrato);
                        if (existente != null)
                            contrato.Id = existente.Id;
                    }
                }
                //FIM Contratos

                _contratoMensalistaAplicacao.Salvar(contratos);

                JsonResult.Status = true;
                return Request.CreateResponse(HttpStatusCode.Created, new { ClienteId = cliente.Id, Message = "Cliente Cadastrado!" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonResult.Message = ex.InnerException.Message);
            }
        }

        private HttpResponseMessage Validar(List<Unidade> unidades, List<int> unidadeIds, Cliente cliente, int clienteId)
        {
            foreach (var id in unidadeIds)
            {
                var unidade = unidades.FirstOrDefault(x => x.Id == id);
                if (unidade == null)
                {
                    JsonResult.Message = $"Não existe a unidade com Id {id}";
                    return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
                }
            }

            if (cliente == null && clienteId > 0)
            {
                JsonResult.Message = $"Não existe um cliente com o id {clienteId}";
                return Request.CreateResponse(HttpStatusCode.NotFound, JsonResult);
            }

            return null;
        }
    }
}
