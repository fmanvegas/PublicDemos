using Common;

namespace SuperheroDatabaseCreator
{
    public record SuperHeroDatabaseBroker
    {
        public static string InsertIntoPortion = "INSERT INTO [main].[SUPERHEROES] ([NAME], [IDENTITY], [BIRTHPLACE], [PUBLISHER], [HEIGHT], [WEIGHT], [GENDER], [APPEARANCE], [EYE], [HAIR], [STRENGTH], [INTELLIGENCE]) VALUES ";
        public static string UpdatePortion = "INSERT INTO [main].[SUPERHEROES] ([NAME], [IDENTITY], [BIRTHPLACE], [PUBLISHER], [HEIGHT], [WEIGHT], [GENDER], [APPEARANCE], [EYE], [HAIR], [STRENGTH], [INTELLIGENCE]) VALUES ";

        public static string InsertValuesPortion(SuperHero h) => $"('{h.Name}', '{h.Identity}', '{h.BirthPlace}', '{h.Publisher}', '{h.Height}', '{h.Weight}', '{h.Gender}', '{h.Appearance}', '{h.Eye}', '{h.Hair}', '{h.Strength}', '{h.Intelligence}')";

        public static string InsertSql(SuperHero h) => $"{InsertIntoPortion} " + InsertValuesPortion(h);

        public static string SelectSql(SuperHero h) => $"SELECT * FROM FROM [main].[SUPERHEROES] WHERE [Name]='{h.Name}' AND [IDENTITY]='{h.Identity}'";
        public static string SelectSql(int id) => $"SELECT * FROM [main].[SUPERHEROES] WHERE [Id] = '{id}'";


        public static string DeleteSql(SuperHero h) => $"DELETE FROM [main].[SUPERHEROES] WHERE [Name]='{h.Name}' AND [IDENTITY]='{h.Identity}' AND [PUBLISHER]='{h.Publisher}' AND [GENDER]='{h.Gender}' AND [EYE]='{h.Eye}' AND [HAIR]='{h.Hair}'";
        public static string DeleteSql(int id) => $"DELETE FROM [main].[SUPERHEROES] WHERE [Id]='{id}'";

        public static string UpdateSql(SuperHero h) => $"UPDATE [main].[SUPERHEROES] SET  {BuildUpdateSetToValues(h)}  WHERE [Id] = '{h.Id}'";

        /// <summary>
        /// Get all property names and values for updating
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public static string BuildUpdateSetToValues(SuperHero h)
        {
            string updateAllProperties = string.Empty;

            var properties = h.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var property in properties)
            {
                if (property.Name.Equals("Id"))
                    continue;

                updateAllProperties += $" [{property.Name}] = '{property.GetValue(h)}',";
            }
            return updateAllProperties[..^1];
        }

        public const string CREATE = "CREATE TABLE IF NOT EXISTS [SUPERHEROES] " +
          "([ID] INTEGER NOT NULL UNIQUE, " +
            "[NAME] TEXT, " +
            "[IDENTITY] TEXT, " +
            "[BIRTHPLACE] TEXT, " +
            "[PUBLISHER] TEXT, " +
            "[HEIGHT] TEXT, " +
            "[WEIGHT] TEXT, " +
            "[GENDER] TEXT, " +
            "[APPEARANCE] TEXT, " +
            "[EYE] TEXT, " +
            "[HAIR] TEXT, " +
            "[STRENGTH] TEXT, " +
            "[INTELLIGENCE] TEXT, " +
            "PRIMARY KEY([ID] AUTOINCREMENT))";
    }
}
