using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace ExtraFunction.DAL
{
    public class DatabaseContext : DbContext
    {


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseCosmos("https://cosmin.documents.azure.com:443/",
               "yEWSz1XH7ys7y44CdOkEaBkuha7tSwXk1TS5XoJKHVOn6qV08J8VpXbeF19YyXBk8WXiTZabILaoCXzPXIXDJw==",
               "cloud-homework");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
