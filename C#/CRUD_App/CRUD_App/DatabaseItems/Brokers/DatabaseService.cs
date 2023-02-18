using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using CRUD_App.DatabaseItems.Interfaces;

namespace CRUD_App.DatabaseItems.Brokers
{
    public abstract class DatabaseService : IDatabaseOperations
    {
        public abstract Task DeleteAsync(int id);
        public abstract bool Exists();
        public abstract Task<List<T>> GetAllAsync<T>();
        public abstract Task<T> GetAsync<T>(int id);
        public abstract Task Insert<T>(T value);
        public abstract Task UpdateAsync<T>(int id, T value);
    }
}
