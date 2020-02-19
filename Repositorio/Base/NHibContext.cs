using NHibernate;
using NHibernate.Context;

namespace Repositorio.Base
{
    /// <summary>
    /// A NHibernate-context object which operates on a single-database.
    /// Offers functionality for creating UnitOfWorks for use in e.g
    /// Repositories that uses NHibernate.
    /// To create a custom Context implementation, inherit from this
    /// class and ensure that SessionFactory is assigned in cTor.
    /// </summary>
    public abstract class NHibContext
    {
        public static ISessionFactory SessionFactory { get; set; }

        public static ISession InnerSession
        {
            get
            {
                if (CurrentSessionContext.HasBind(SessionFactory))
                {
                    return SessionFactory.GetCurrentSession();
                }

                CurrentSessionContext.Bind(SessionFactory.OpenSession());
                InnerSession.FlushMode = FlushMode.Commit;

                return SessionFactory.GetCurrentSession();
            }
        }

        /// <summary>
        /// Creates an new UnitOfWork.
        /// </summary>
        /// <returns></returns>
        public NHibSession CreateNewSession()
        {
            return new NHibSession();
        }
    }
}