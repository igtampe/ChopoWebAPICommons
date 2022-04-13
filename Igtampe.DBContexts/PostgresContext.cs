using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Igtampe.DBContexts {

    /// <summary>Abstract class for a DBContext that connects to a Postgresql database hosted by Heroku</summary>
    public abstract class PostgresContext : DbContext {

        /// <summary>Overrides onConfiguring to use a DBURL that can be found either in the DATABASE_URL environment variable, or in the DBURL text file</summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

            var DBURL = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "";
            if (string.IsNullOrWhiteSpace(DBURL)) {
                if (File.Exists("DBURL.txt")) { DBURL = File.ReadAllText("DBURL.txt"); } else { File.WriteAllText("DBURL.txt", "here"); }
                if (string.IsNullOrWhiteSpace(DBURL)) { throw new InvalidOperationException("Could not determine what DBURL to connect to!"); }
            }

            optionsBuilder.UseNpgsql(ConvertPostgresURLToConnectionString(DBURL));
        }

        /// <summary>Overrides on model creation to remove the plural convention</summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //This will singularize all table names
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()) { entityType.SetTableName(entityType.DisplayName()); }
        }

        /// <summary>Converts a URI (From Heroku usually) to the expected connection string of npgsql</summary>
        /// <param name="DBURL"></param>
        /// <returns></returns>
        public static string ConvertPostgresURLToConnectionString(string DBURL) {
            //OK so now we have this
            //postgres://user:password@host:port/database

            //Drop the beginning 
            string PURL = DBURL.Replace("postgres://", "");

            //Split the beginning and end into two parts at the @
            string[] PurlSplit = PURL.Split('@');

            //We should now have:
            //user:password
            string Username = PurlSplit[0].Split(':')[0];
            string Password = PurlSplit[0].Split(':')[1];

            //And:
            //host:port/database

            //Split this again by /
            PurlSplit = PurlSplit[1].Split('/');

            //Now we should have
            //host:port
            string Host = PurlSplit[0].Split(':')[0];
            string Port = PurlSplit[0].Split(':')[1];

            //Database
            string Database = PurlSplit[1];

            return @$"
                        Host={Host}; Port={Port}; 
                        Username={Username}; Password={Password};
                        Database={Database};
                        Pooling=true;
                        SSL Mode=Require;
                        TrustServerCertificate=True;
                    ";
        }
    }
}
