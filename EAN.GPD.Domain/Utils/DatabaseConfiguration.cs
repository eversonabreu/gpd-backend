using EAN.GPD.Infrastructure.Database;
using EAN.GPD.Infrastructure.Database.Migrations;

namespace EAN.GPD.Domain.Utils
{
    public static class DatabaseConfiguration
    {
        private static string connectionString;
        public static string ConnectionString
        {
            get => connectionString;
            set
            {
                connectionString = value;
                DatabaseProvider.ConnectionString = value;
            }
        }

        public static void Migrate() => new Migration();
    }
}
