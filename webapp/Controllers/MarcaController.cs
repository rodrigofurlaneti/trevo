using Aplicacao;
using Entidade;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class MarcaController : GenericController<Marca>
    {
        public List<Marca> ListaMarcas => Aplicacao?.Buscar()?.ToList() ?? new List<Marca>();

        public MarcaController(IMarcaAplicacao marcaAplicacao)
        {
            Aplicacao = marcaAplicacao;
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarMarcaPeloNome(string nome)
        {
            JsonResult result = new JsonResult();

            if (!(string.IsNullOrEmpty(nome) || string.IsNullOrWhiteSpace(nome)))
            {
                var filtro = Aplicacao.BuscarPor(w => w.Nome.Contains(nome));
                if (!ReferenceEquals(filtro, null))
                {
                    if (filtro.Count == 0)
                    {
                        var marca = new Marca();
                        marca.Nome = nome;
                        marca.Id = -1;
                        filtro.Add(marca);
                    }                   

                    result = Json(filtro.Select(c => new
                    {
                        c.Id,
                        c.Nome
                    }));
                }
            }
            return result;
        }
    }
}
