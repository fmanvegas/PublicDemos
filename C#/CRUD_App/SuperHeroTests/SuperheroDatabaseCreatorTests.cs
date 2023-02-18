using SuperheroDatabaseCreator;
using Common;
using Xunit.Sdk;

namespace SuperHeroTests
{
    public class SuperheroDatabaseCreatorTests
    {
        [Fact]
        public void Test_GetOperatingPath_NotNull()
        {
            var path = Creator.GetOperatingPath();
            Assert.False(string.IsNullOrWhiteSpace(path));
        }

        [Fact]
        public void Test_GetSuperHeroDataPath_FilePathExists()
        {
            var path = Creator.GetSuperHeroDataPath();
            Assert.False(string.IsNullOrWhiteSpace(path));
        }
        [Fact]
        public void Test_SuperHero_InvalidData_ValidFalse()
        {
            var hero = SuperheroDBFactory.Get("Name,Identity,Birth place,Publisher,Gender,First appearance,Eye color,Hair color,Strength,Intelligence");
            Assert.False(hero.Valid);
        }
        [Fact]
        public void Test_SuperHero_EmptyData_ValidFalse()
        {
            var hero = SuperheroDBFactory.Get(string.Empty);
            Assert.False(hero.Valid);
        }
        [Fact]
        public void Test_SuperHero_NullData_ValidFalse()
        {
            var hero = SuperheroDBFactory.Get(null);
            Assert.False(hero.Valid);
        }
        [Fact]
        public void Test_SuperHero_ValidIncompleteData_ValidTrue()
        {
            var hero = SuperheroDBFactory.Get("Yoda,Yoda,,George Lucas,66.29,17.01,M,1980,Brown,White,55,high");
            Assert.True(hero.Valid);
        }
        [Fact]
        public void Test_SuperHero_ValidCompleteData_ValidTrue()
        {
            var hero = SuperheroDBFactory.Get("Lady Deathstrike,Yuriko Oyama,\"Osaka, Japan\",Marvel Comics,175.85,58.89,F,1985,Brown,Black,30,good");
            Assert.True(hero.Valid);
        }

        [Fact]
        public async void Test_PullDataFromFile_ValidPath_19Lines()
        {
            var path = Creator.GetSuperHeroDataPath();
            if (string.IsNullOrWhiteSpace(path))
                throw new FailException("SuperHero Path Not Found");

            var data = await Creator.PullDataFromFile(path);

            Assert.Equal(19, data.Count());
        }
        [Fact]
        public async void Test_PullDataFromFile_InvalidPath()
        {
            var data = await Creator.PullDataFromFile(string.Empty);

            Assert.Empty( data );
        }       

    }
}