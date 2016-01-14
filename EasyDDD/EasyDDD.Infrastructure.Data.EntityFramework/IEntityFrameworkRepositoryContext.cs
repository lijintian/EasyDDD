using EasyDDD.Core.Repository;
using System.Data.Entity;

namespace EasyDDD.Infrastructure.Data.EntityFramework
{
    public interface IEntityFrameworkRepositoryContext : IRepositoryContext
    {
        DbContext Context { get; }
    }
}
