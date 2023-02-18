namespace SuperheroDatabaseCreator
{
    public enum CreatorStateType { Idle, Processing, Finished, Error }


    public class Creator
    {
        private const string DATA_NAME = "superheroData.csv";

        public CreatorDisplayProperties Properties { get; set; } = new();

        public static string GetOperatingPath()
        {
            var location = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(location))
                return string.Empty;

            DirectoryInfo di = new(location);
            if (!di.Exists)
                return string.Empty;

            return di.FullName;
        }

        public static string GetSuperHeroDataPath()
        {
            var location = GetOperatingPath();
            if (string.IsNullOrEmpty(location))
                return string.Empty;

            var dataFile = Directory.GetFiles(location, DATA_NAME, SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(dataFile))
                return string.Empty;

            return dataFile;
        }

        public async Task<bool> Create()
        {
            try
            {
                Properties.Reset();

                if (GetSuperHeroDataPath() is not string path || path.Length == 0)
                    throw new Exception("Could not find SuperHero Data");

                Properties.State = CreatorStateType.Processing;

                var data = await PullDataFromFile(path);
                if (data is null || data.Any() == false)
                    throw new Exception("Could not read SuperHero Data");

                SqliteDataAccess sqliteDataAccess = new($"{GetOperatingPath()}\\SuperHeroDB.db3");
                await sqliteDataAccess.CreateMainTable();

                Properties.Maximum = data.Count();

                Progress<int> progress = new();
                progress.ProgressChanged += (o, i) => { Properties.Current += i; };

                if (await sqliteDataAccess.InsertBaseData(data, progress))
                {
                    Properties.State = CreatorStateType.Finished;
                    return true;
                }

            }
            catch (Exception ex)
            {
                Properties.ErrorMessage = ex.Message;
            }


            Properties.State = CreatorStateType.Error;
            return false;
        }

        public static async Task<IEnumerable<string>> PullDataFromFile(string path)
        {
            if (File.Exists(path))
                return await File.ReadAllLinesAsync(path);
            //If we fail, return an empty string array
            return Array.Empty<string>();
        }
    }
}
