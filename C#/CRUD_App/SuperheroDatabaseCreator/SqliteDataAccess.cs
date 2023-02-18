using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using Common;

namespace SuperheroDatabaseCreator
{
    internal class SqliteDataAccess
    {
        string connectionString;
        readonly string connConstructor = "Data Source={0};Cache=Shared";
        readonly IDbConnection dbConnection;

        public SqliteDataAccess(string locationToDB)
        {
            connectionString = string.Format(connConstructor, locationToDB);
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            ErrorMessage = string.Empty;
        }

        public string ErrorMessage { get; set; } 

        public async Task ExecuteSQLAsync(string sql)
        {
            await dbConnection.ExecuteAsync(sql);
        }

        public async Task<bool> VerifyDataExists()
        {
            string sql = "SELECT count(*) FROM [SUPERHEROES]";
            var result = await dbConnection.ExecuteScalarAsync(sql);
            return result is not null && Convert.ToInt32(result) > 0;
        }
        public async Task<bool> CreateMainTable()
        {
            try
            {
                await ExecuteSQLAsync(SuperHeroDatabaseBroker.CREATE);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Given a full list of what to insert, put the valid entries into the database.
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        internal async Task<bool> InsertBaseData(IEnumerable<string> data, IProgress<int> progress)
        {
            bool toReturn = true;

            //This can go up to 999, but happens so fast the user can't see he progress.
            int maxValueToGrab = 200;

            await Task.Run(async () =>
            {
                try
                {
                    for (int indx = 0; indx < data.Count();)
                    {
                        //Grab either 200, or whatever is remaining
                        int amountToGrab = indx < data.Count() - maxValueToGrab ? maxValueToGrab : data.Count() - indx;
                        var range = data.AsList().GetRange(indx, amountToGrab);
                        //Initial statement
                        string insert = SuperHeroDatabaseBroker.InsertIntoPortion;
                        //Append each in our range to the end of this statement
                        foreach (var item in range)
                        {
                            SuperHero hero = SuperheroDBFactory.Get(item);
                            if (hero.Valid)
                                insert += $"\r\n {SuperHeroDatabaseBroker.InsertValuesPortion(hero)},";
                        }
                        //Remove the trailing ,
                        insert = insert[..^1];
                        //Push to db
                        await ExecuteSQLAsync(insert);
                        //Increment and report to the UI
                        indx += amountToGrab;
                        progress?.Report(amountToGrab);
                    }
                    toReturn = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    toReturn = false;
                }               
            });
            
            return toReturn;
        }      
    }
}
