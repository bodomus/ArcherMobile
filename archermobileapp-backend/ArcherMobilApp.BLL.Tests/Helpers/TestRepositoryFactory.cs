using System;

using Microsoft.EntityFrameworkCore;

using Archer.AMA.DAL.EntityFramework.Base;

namespace ArcherMobilApp.BLL.Tests.Helpers
{
    public class TestRepositoryFactory<T1>
    {
        private ArcherContext _dataContext;

        public ArcherContext DataContext => _dataContext;

        public TestRepositoryFactory()
        {

        }

        public T1 Create()
        {
            var builder = new DbContextOptionsBuilder<ArcherContext>();
            //builder.UseInMemoryDatabase(databaseName: "ArcherMobile");
            var options = builder.Options;

            _dataContext = new ArcherContext(options);
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureDeletedAsync();
            _dataContext.Database.EnsureCreated();
            _dataContext.Database.EnsureCreatedAsync();
            return (T1)Activator.CreateInstance(typeof(T1), new object[] { options });
        }
    }
}
