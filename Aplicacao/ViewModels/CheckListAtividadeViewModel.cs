using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class CheckListAtividadeViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Usuario { get; set; }
        public FuncionarioViewModel Responsavel { get; set; }
        public IList<CheckListAtividadeTipoAtividadeViewModel> TiposAtividade { get; set; }

        public CheckListAtividadeViewModel(CheckListAtividade checkListAtividade)
        {
            Id = checkListAtividade.Id;
            Descricao = checkListAtividade.Descricao;
            Ativo = checkListAtividade.Ativo;
            DataInsercao = checkListAtividade.DataInsercao;
            Usuario = checkListAtividade.Usuario;
            Responsavel = new FuncionarioViewModel(checkListAtividade?.Responsavel ?? new Funcionario());
            TiposAtividade = new CheckListAtividadeTipoAtividadeViewModel().ListaTipoAtividades(checkListAtividade.TiposAtividade);
        }

        public CheckListAtividadeViewModel()
        {
            DataInsercao = DateTime.Now;
            Responsavel = new FuncionarioViewModel();
        }

        public CheckListAtividade ToEntity()
        {
            var entidade = new CheckListAtividade();

            entidade.Id = Id;
            entidade.Descricao = Descricao;
            entidade.DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao;
            entidade.Responsavel = Responsavel?.ToEntity();
            entidade.TiposAtividade = new CheckListAtividadeTipoAtividadeViewModel().ListaTipoAtividades(TiposAtividade);
            entidade.Usuario = Usuario;

            return entidade;

        }
    }
}
