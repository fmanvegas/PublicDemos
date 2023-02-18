﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_App.DatabaseItems.Interfaces
{
    public interface IDatabaseOperations
    {
        bool Exists();
        Task<T> GetAsync<T>(int id);
        Task<List<T>> GetAllAsync<T>();
        Task DeleteAsync(int id);
        Task UpdateAsync<T>(int id, T value);
        Task Insert<T>(T value);
    }
}