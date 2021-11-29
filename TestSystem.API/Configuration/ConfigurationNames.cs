namespace TestSystem.API.Configuration
{
    internal static class ConfigurationNames
    {
        public const string LocalDbConnectionString = "LocalSqlServer";
        public const string RemoteDbConnectionString = "RemoteSqlServer";
        public const string DefaultAdminSeedingData = "DefaultAdmin";
        public const string TokenGenerationSecret = "JwtSecret";
    }
}
