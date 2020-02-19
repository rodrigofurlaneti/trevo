using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Linq;
using System.Web;
using System.Collections.Generic;

namespace Dominio
{
    public interface IFuncionarioServico : IBaseServico<Funcionario>
    {
        List<Funcionario> BuscarComDadosSimples();
        List<Funcionario> BuscarFuncionariosDoSupervisor(int supervisorId);
    }

    public class FuncionarioServico : BaseServico<Funcionario, IFuncionarioRepositorio>, IFuncionarioServico
    {
        private readonly IBeneficioFuncionarioRepositorio _beneficioFuncionarioRepositorio;
        private readonly IItemFuncionarioRepositorio _itemFuncionarioRepositorio;
        private readonly IOcorrenciaFuncionarioRepositorio _ocorrenciaFuncionarioRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IEstoqueManualServico _estoqueManualServico;
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public FuncionarioServico(
            IBeneficioFuncionarioRepositorio beneficioFuncionarioRepositorio,
            IItemFuncionarioRepositorio itemFuncionarioRepositorio,
            IOcorrenciaFuncionarioRepositorio ocorrenciaFuncionarioRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            IEstoqueManualServico estoqueManualServico,
            IFuncionarioRepositorio funcionarioRepositorio
            )
        {
            _beneficioFuncionarioRepositorio = beneficioFuncionarioRepositorio;
            _itemFuncionarioRepositorio = itemFuncionarioRepositorio;
            _ocorrenciaFuncionarioRepositorio = ocorrenciaFuncionarioRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _estoqueManualServico = estoqueManualServico;
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public override void Salvar(Funcionario funcionario)
        {
            if (funcionario.ItemFuncionario != null)
            {
                var itemFuncionarioAntigo = _itemFuncionarioRepositorio.GetById(funcionario.ItemFuncionario.Id);

                _itemFuncionarioRepositorio.AtualizarEstoque(funcionario.ItemFuncionario, itemFuncionarioAntigo);
                //CriarNotificacoesSeEstoqueBaixo(funcionario.ItemFuncionario);
            }

            var funcionarioSalvo = _funcionarioRepositorio.GetById(funcionario.Id);
            funcionario.ListaIntervaloDozeTrintaSeis = funcionario.ListaIntervaloDozeTrintaSeis != null && funcionario.ListaIntervaloDozeTrintaSeis.Any() ?
                                                         funcionario.ListaIntervaloDozeTrintaSeis : funcionarioSalvo.ListaIntervaloDozeTrintaSeis;

            funcionario.ListaIntervaloCompensacao = funcionario.ListaIntervaloCompensacao != null && funcionario.ListaIntervaloCompensacao.Any() ?
                                                         funcionario.ListaIntervaloCompensacao : funcionarioSalvo.ListaIntervaloCompensacao;

            funcionario.ListaIntervaloNoturno = funcionario.ListaIntervaloNoturno != null && funcionario.ListaIntervaloNoturno.Any() ?
                                                         funcionario.ListaIntervaloNoturno : funcionarioSalvo.ListaIntervaloNoturno;

            base.Salvar(funcionario);

            Repositorio.Clear();
            _beneficioFuncionarioRepositorio.DeleteOrphan();
            _itemFuncionarioRepositorio.DeleteOrphan();
            _ocorrenciaFuncionarioRepositorio.DeleteOrphan();
        }

        private void CriarNotificacoesSeEstoqueBaixo(ItemFuncionario itemFuncionario)
        {
            var usuarioId = (int)(HttpContext.Current.User as dynamic).UsuarioId;
            var usuario = _usuarioRepositorio.GetById(usuarioId);

            var listaMaterial = itemFuncionario.ItemFuncionariosDetalhes.Select(x => x.Material).DistinctBy(x => x.Id).ToList();
            foreach (var material in listaMaterial)
            {
                _estoqueManualServico.CriarNotificacaoSeEstoqueBaixo(material, usuario);
            }
        }

        public List<Funcionario> BuscarComDadosSimples()
        {
            return _funcionarioRepositorio.BuscarComDadosSimples();
        }

        public List<Funcionario> BuscarFuncionariosDoSupervisor(int supervisorId)
        {
            return _funcionarioRepositorio.ListBy(x => x.Supervisor.Id == supervisorId).ToList();
        }
    }
}