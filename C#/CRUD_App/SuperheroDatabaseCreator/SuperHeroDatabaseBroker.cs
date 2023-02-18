using Common;

namespace SuperheroDatabaseCreator
{
    public record SuperHeroDatabaseBroker
    {
        public static string ToInsert(SuperHero h) => "INSERT INTO [main].[SUPERHEROES] ([NAME], [IDENTITY], [BIRTHPLACE], [PUBLISHER], [HEIGHT], [WEIGHT], [GENDER], [APPEARANCE], [EYE], [HAIR], [STRENGTH], [INTELLIGENCE]) VALUES " + ToInsertValuesPortion(h);

        public static string ToInsertValuesPortion(SuperHero h) => $"('{h.Name}', '{h.Identity}', '{h.BirthPlace}', '{h.Publisher}', '{h.Height}', '{h.Weight}', '{h.Gender}', '{h.Appearance}', '{h.Eye}', '{h.Hair}', '{h.Strength}', '{h.Intelligence}')";
    }
}
