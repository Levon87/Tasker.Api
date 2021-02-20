using Auth.Service.Model.Entities;

namespace Auth.Service.DataRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthServiceDbContext _context;

        public UnitOfWork(
            AuthServiceDbContext context)
        {
            _context = context;

            AuthClients = new AuthClientRepository(context);
        }


        public IAuthClientRepository AuthClients { get; set; }

        public IRepository<T> Repository<T>() where T : class
        {
            if (typeof(T) == typeof(AuthClient))
            {
                return AuthClients as IRepository<T>;
            }

            return null;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}