namespace ArcherMobilApp.DAL.MsSql.Contract
{
    public interface IRepositoryContextFactory
    {
        RepositoryContext CreateDbContext(string connectionString);
    }
}
