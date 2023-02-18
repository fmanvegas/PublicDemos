﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CRUD_App.DatabaseItems.Brokers
{
    public class SQLiteService : DatabaseService
    {
        private IDbConnection dbConnection;
        readonly string connConstructor = "Data Source={0};Cache=Shared";

        private const string DBNAME = "SuperHeroDB.db3";

        public SQLiteService() { }

       

        /// <summary>
        /// For sqlite db, we're checking if a db3 file exists within our running location
        /// </summary>
        /// <returns></returns>
        public override bool Exists()
        {
            var location = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!string.IsNullOrEmpty(location))
            {
                DirectoryInfo directory = new(location);

                if (directory.Exists)
                {
                    var file = directory.GetFiles(DBNAME, SearchOption.TopDirectoryOnly).FirstOrDefault();
                    
                    if (file != null)
                    {
                        SetConnection(file);
                        return true;
                    }
                }
            }

            return false;
        }

        private void SetConnection(FileInfo file)
        {
            var connectionString = string.Format(connConstructor, file.FullName);
            dbConnection?.Close();
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();
        }

        public override async Task DeleteAsync(int id)
        {

            await Task.Delay(1);
        }

        public override async Task<List<T>> GetAllAsync<T>()
        {
            var sql = "SELECT * FROM [main].[SUPERHEROES]";
            var results = await dbConnection.QueryAsync<T>(sql);
            if (results is List<T> list)
                return list;

            return (List<T>)results;
        }

        public override async Task<T> GetAsync<T>(int id)
        {
            await Task.Delay(1);

            return default;
        }

        public override async Task Insert<T>(T value)
        {
            await Task.Delay(1);
        }

        public override async Task UpdateAsync<T>(int id, T value)
        {
            await Task.Delay(1);

        }
    }
}