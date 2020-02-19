using Portal.App_Helpers;

namespace Portal
{
    public static class IdentityConfig
    {
        public static void RegisterIdentities()
        {
            // Ensures the default demo user is available to login with
            UserManager.Seed();
        }
    }
}