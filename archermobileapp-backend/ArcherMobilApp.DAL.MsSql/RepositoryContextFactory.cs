using Microsoft.EntityFrameworkCore;

using ArcherMobilApp.DAL.MsSql.Contract;

namespace ArcherMobilApp.DAL.MsSql
{
    public class RepositoryContextFactory : IRepositoryContextFactory
    {
        public RepositoryContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new RepositoryContext(optionsBuilder.Options);
        }
    }
}
