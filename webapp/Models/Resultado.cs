namespace Portal.Models
{
    public class Resultado<T>
    {
        public bool Sucesso { get; set; }
        public string TipoModal { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public bool Redirect { get; set; }
        public T Model { get; set; }
        public object Adicional { get; set; }
    }
}