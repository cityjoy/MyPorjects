using System;
using System.Collections.Generic;
using System.Transactions;

namespace EF_Web_Test.Repository
{
    /// <summary>
    /// 工作单元实现
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private readonly EFDBContext context;
        private bool disposed;
        private Dictionary<string, object> repositories;

        public UnitOfWork(EFDBContext context)
        {

            this.context = context;
        }
        public UnitOfWork()
        {
            context = new EFDBContext();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Commit()
        {
            return context.SaveChanges()>0;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public Repository<T> Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.Add(type, repositoryInstance);
            }
            return (Repository<T>)repositories[type];
        }

    }
}