using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class UsuarioController : GenericController<Usuario>
    {
        public List<Usuario> ListaUsuarios => Aplicacao?.Buscar()?.ToList() ?? new List<Usuario>();
        public List<Perfil> ListaPerfis => _usuarioAplicacao.BuscarPerfis() ?? new List<Perfil>();
        public List<Funcionario> ListaFuncionario => _funcionarioAplicacao?.Buscar()?.ToList() ?? new List<Funcionario>();
        public IEnumerable<ChaveValorViewModel> ListaOperadorPerfilSoftpark => Aplicacao.BuscarValoresDoEnum<OperadorPerfilSoftpark>();
        public List<Unidade> ListaUnidade => _unidadeAplicacao?.Buscar()?.ToList() ?? new List<Unidade>();

        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;
        private readonly IPessoaAplicacao _pessoaAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public UsuarioController(
            IUsuarioAplicacao usuarioAplicacao
            , IFuncionarioAplicacao funcionarioAplicacao
            , IPessoaAplicacao pessoaAplicacao
            , IUnidadeAplicacao unidadeAplicacao
            )
        {
            Aplicacao = usuarioAplicacao;
            _pessoaAplicacao = pessoaAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            Session["ImgAvatar"] = null;
            return View("Index");
        }


        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(UsuarioViewModel model)
        {
            try
            {
                var id = model.Id.HasValue && model.Id.Value > 0 ? model.Id.Value : 0;
                var usuario = Aplicacao.BuscarPorId(id) ?? new Usuario();
                var perfis = _usuarioAplicacao.BuscarPerfis().ToList();

                if (usuario.Perfils == null)
                    usuario.Perfils = new List<UsuarioPerfil>();

                if (usuario.Funcionario == null)
                    usuario.Funcionario = new Funcionario();

                usuario.Perfils.Clear();
                foreach (var perfilId in model.ListaPerfilId)
                    usuario.Perfils.Add(new UsuarioPerfil { Perfil = perfis.FirstOrDefault(x => x.Id == perfilId) });

                usuario.Funcionario = _funcionarioAplicacao.BuscarPorId(model.Funcionario.Id);
                if(usuario.Funcionario != null)
                {
                    usuario.Funcionario.Pessoa.ContatoEmail = model.Email;
                    usuario.Login = usuario.Funcionario.Pessoa.DocumentoCpf;
                }
                else
                {
                    usuario.Login = model.Login;
                    usuario.Email = model.Email;
                }

                usuario.Senha = model.Senha;
                usuario.PrimeiroLogin = model.PrimeiroLogin;
                usuario.Ativo = model.Ativo;
                usuario.ImagemUpload = (byte[])Session["ImgAvatar"];
                usuario.TemAcessoAoPDV = model.TemAcessoAoPDV;
                usuario.OperadorPerfil = model.OperadorPerfil.ToString();
                usuario.Unidade = model.Unidade?.ToEntity();
                usuario.EhFuncionario = model.EhFuncionario;
                usuario.NomeCompleto = model.NomeCompleto;
                Session["UserAvatar"] = usuario.ImagemUpload;

                _usuarioAplicacao.Salvar(usuario);

                ModelState.Clear();
                Session["ImgAvatar"] = null;

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
                return View("Index", model);
            }
            catch (SoftparkIntegrationException sx)
            {
                CriarModalAvisoComRetornoParaIndex(sx.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro(ex.Message);
                return View("Index", model);
            }

            return View("Index");
        }

        [HttpPost]
        public JsonResult BuscarDadosDaPessoa(int funcionarioId)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);

            return Json(new
            {
                Login = funcionario.Pessoa.DocumentoCpf,
                Email = funcionario.Pessoa?.Contatos?.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Email)?.Contato?.Email ?? string.Empty
            });
        }

        public override ActionResult Edit(int id)
        {
            var usuario = Aplicacao.BuscarPorId(id);
            var viewModel = new UsuarioViewModel(usuario);

            Session["ImgAvatar"] = usuario.ImagemUpload;

            return View("Index", viewModel);
        }

        [HttpPost]
        public void ArmazenarImagem(string file)
        {
            const string strBase64 = ";base64,";
            Session["ImgAvatar"] = Convert.FromBase64String(file.Substring(file.IndexOf(strBase64, StringComparison.Ordinal) + strBase64.Length));
        }

        [HttpPost]
        public string GetAvatarUser(int id)
        {
            return Session["UserAvatar"] == null ? Aplicacao.BuscarPorId(id).GetImage() : Core.Tools.GetImageFromBase64((byte[])Session["UserAvatar"]);
        }

        public JsonResult VerificarSeUsuarioExiste(int funcionarioId)
        {
            var existe = _usuarioAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId) != null;
            return Json(existe, JsonRequestBehavior.AllowGet);
        }
    }
}