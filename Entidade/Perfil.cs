using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entidade.Base;

namespace Entidade
{
    public class Perfil : BaseEntity, IAudit
    {
        public Perfil()
        {
            Permissoes = new List<Permissao>();
            Usuarios = new List<UsuarioPerfil>();
        }

        [Required(ErrorMessage = "Campo deve ser preenchido!"), StringLength(100)]
        public virtual string Nome { get; set; }
        public virtual IList<PerfilMenu> Menus { get; set; }
        public virtual IList<Permissao> Permissoes { get; set; }
        public virtual IList<UsuarioPerfil> Usuarios { get; set; }
        private List<int> ListaIds { get; set; }
        public virtual List<int> ListaMenu
        {
            get
            {

                var retorno = new List<int>();

                if(Menus != null)
                    retorno.AddRange(Menus.Select(perfilMenu => perfilMenu.Menu.Id));

                return ListaIds != null && ListaIds.Any() 
                        ? ListaIds 
                        : retorno;
            }
            set { ListaIds = value; }
        }

    }
}