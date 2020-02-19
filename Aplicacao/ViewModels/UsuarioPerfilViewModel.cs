namespace Aplicacao.ViewModels
{
    public class UsuarioPerfilViewModel
    {
        public virtual int Id { get; set; }
        public virtual PerfilViewModel Perfil { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }
    }
}