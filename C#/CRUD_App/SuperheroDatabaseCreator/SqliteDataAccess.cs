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
        }

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
            await ExecuteSQLAsync(CREATE);

            return true;
        }

        internal async Task<bool> InsertBaseData(IEnumerable<string> data, IProgress<int> progress)
        {
            bool toReturn = false;

            int maxValueToGrab = 100;

            await Task.Run(async () =>
            {
                try
                {
                    for (int indx = 0; indx < data.Count();)
                    {
                        int amountToGrab = indx < data.Count() - maxValueToGrab ? maxValueToGrab : data.Count() - indx;

                        var range = data.AsList().GetRange(indx, amountToGrab);
                        string insert = "INSERT INTO [main].[SUPERHEROES] ([NAME], [IDENTITY], [BIRTHPLACE], [PUBLISHER], [HEIGHT], [WEIGHT], [GENDER], [APPEARANCE], [EYE], [HAIR], [STRENGTH], [INTELLIGENCE]) VALUES ";
                        foreach(var item in range)
                        {
                            SuperHero hero = SuperheroDBFactory.Get(item);
                            if (hero.Valid)
                                insert += $"\r\n {SuperHeroDatabaseBroker.ToInsertValuesPortion(hero)},";
                        }
                        insert = insert[..^1];
                        await ExecuteSQLAsync(insert);
                        indx += amountToGrab;
                        progress?.Report(amountToGrab);
                    }

                    toReturn = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }               
            });
            
            return toReturn;
        }

        private const string CREATE = "CREATE TABLE IF NOT EXISTS [SUPERHEROES] " +
            "([ID] INTEGER NOT NULL UNIQUE, [NAME] TEXT, [IDENTITY] TEXT, [BIRTHPLACE] TEXT, [PUBLISHER] TEXT, [HEIGHT] TEXT, [WEIGHT] TEXT, [GENDER] TEXT, [APPEARANCE] TEXT, [EYE] TEXT, [HAIR] TEXT, [STRENGTH] TEXT, [INTELLIGENCE] TEXT, PRIMARY KEY([ID] AUTOINCREMENT))";
    }
}
