using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface IDescontoServico : IBaseServico<Desconto>
    {
        //void Salvar(NegociacaoSeloDesconto NegociacaoSeloDesconto, int idUsuario);
        Notificacao SalvarNotificacaoComRetorno(Desconto pedidoLocacao, int idUsuario, string descricaoAuxiliar = "");
    }
    public class DescontoServico : BaseServico<Desconto, IDescontoRepositorio>, IDescontoServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INegociacaoSeloDescontoNotificacaoServico _negociacaoSeloDescontoNotificacaoServico;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public DescontoServico(IUsuarioRepositorio usuarioRepositorio,
                                      ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                      INegociacaoSeloDescontoNotificacaoServico negociacaoSeloDescontoNotificacaoServico,
                                      INotificacaoRepositorio notificacaoRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _negociacaoSeloDescontoNotificacaoServico = negociacaoSeloDescontoNotificacaoServico;
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        public Notificacao SalvarNotificacaoComRetorno(Desconto desconto, int idUsuario, string descricaoAuxiliar = "")
        {
            var enumerador = (Entidades)Enum.Parse(typeof(Entidades), nameof(Desconto));
            var notificacao = new Notificacao
            {
                Usuario = new Usuario { Id = idUsuario },
                TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == enumerador),
                Status = StatusNotificacao.Aguardando,
                Descricao = $"{enumerador.ToDescription()} ID: {desconto.Id.ToString()} {descricaoAuxiliar}",
                DataInsercao = DateTime.Now,
                AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar,
                DataVencimentoNotificacao = desconto.DataVencimentoNotificacao <= System.Data.SqlTypes.SqlDateTime.MinValue.Value 
                                            ? DateTime.Now.AddDays(+4).Date 
                                            : desconto.DataVencimentoNotificacao
            };
            var notificacaoId = _notificacaoRepositorio.SaveAndReturn(notificacao);

            return _notificacaoRepositorio.GetById(notificacaoId);
        }
    }
}