using System;

namespace Auth.Service.DataRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthClientRepository AuthClients { get; set; }

        IRepository<T> Repository<T>() where T : class;

        int Complete();
    }
}
