using Contacts.Infrastructure.Models;

namespace Contacts.Infrastructure.Storages
{
    public interface IStorage<T> : IDisposable where T : BaseModel
    {
        List<T> GetAll(int offset, int count);
        T GetById(int id);
        T Create(T entity);
        T Update(T entity);
        void Remove(T entity);
        void Remove(int id);
        void Synchronize();
    }
}