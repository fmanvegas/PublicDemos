using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SuperheroDBFactory
    {
        public static SuperHero Get(string data)
        {
            if (string.IsNullOrEmpty(data))
                return new();

            data = data.Replace("'", "''");

            List<string> splitData = new();

            if (data.Contains('\"'))
                splitData = SplitByQuotes(data);
            else
                splitData = SplitNormal(data);

            if (splitData.Count != 12)
                return new();

            SuperHero hero = new(splitData);


            return hero;
        }

        private static List<string> SplitByQuotes(string data)
        {
            List<string> result = new();
            var split = data.Split('\"');
            if (split.Length == 3)
            {
                result.AddRange(split[0].Split(',')[0..2]);
                result.Add(split[1]);
                result.AddRange(split[2].Split(',')[1..]);
            }
            return result;
        }
        private static List<string> SplitNormal(string data) => data.Split(',').ToList();



    }


}
