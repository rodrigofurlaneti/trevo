namespace Entidade
{
    public class DadosLancamentosVO
    {
        public virtual string Unidade { get; set; }
        public virtual string TipoServico { get; set; }
        public virtual int QuantidadeTotal { get; set; }
        public virtual decimal ValorTotal { get; set; }
        public virtual string Cliente { get; set; }
    }
}