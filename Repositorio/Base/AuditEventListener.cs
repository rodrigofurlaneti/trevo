
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entidade.Base;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Event;
using Dominio.IRepositorio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Base
{
    public class AuditEventListener : IPostUpdateEventListener
    {
        private const string NoValueString = "*No Value*";

        private static string GetStringValueFromStateArray(IList<object> stateArray, int position)
        {
            var value = stateArray[position];

            return value == null || value.ToString() == string.Empty
                    ? NoValueString
                    : value.ToString();
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            if (@event.Entity is Audit || !(@event.Entity is IAudit))
            {
                return;
            }

            if (@event.OldState == null)
            {
                return;
            }
            
            var dirtyFieldIndexes = @event.Persister.FindDirty(@event.State, @event.OldState, @event.Entity, @event.Session);
            var session = @event.Session.GetSession(EntityMode.Poco);
            var currentUser = HttpContext.Current.User as dynamic;
            var usuarioNome = string.Empty;
            var usuarioId = 0;

            if (currentUser.GetType() == typeof(System.Security.Principal.GenericPrincipal))
            {
                var usuarioRepositorio = ServiceLocator.Current.GetInstance<IUsuarioRepositorio>();
                var usuarioRoot = usuarioRepositorio.FirstBy(x => x.Ativo && x.Perfils.Any(p => p.Perfil.Nome.Equals("Root")));
                usuarioId = usuarioRoot.Id;
            }
            else if (currentUser.GetType().Name.Contains("CustomPrincipal"))
            {
                usuarioNome = currentUser?.Nome;
                usuarioId = currentUser?.UsuarioId ?? 0;
            }

            if (usuarioId > 0)
                foreach (var audit in from dirtyFieldIndex in dirtyFieldIndexes
                                      let oldValue = GetStringValueFromStateArray(@event.OldState, dirtyFieldIndex)
                                      let newValue = GetStringValueFromStateArray(@event.State, dirtyFieldIndex)
                                      where oldValue != newValue
                                      select new Audit
                                      {
                                          Entidade = @event.Entity.GetType().Name,
                                          Atributo = @event.Persister.PropertyNames[dirtyFieldIndex],
                                          ValorAntigo = oldValue,
                                          ValorNovo = newValue,
                                          Usuario = (usuarioId).ToString(),
                                          CodigoEntidade = (int)@event.Id,
                                          Data = DateTime.Now,
                                          UsuarioNome = (string)usuarioNome
                                      })
                {
                    session.Save(audit);
                }

            session.Flush();
        }

        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
