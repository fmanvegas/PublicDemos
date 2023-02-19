using System.Collections.Generic;
using System.Diagnostics;

namespace Common
{
    [DebuggerDisplay("{Name} ({Identity})")]
    public class SuperHero
    {
        public bool Valid = false;

        public SuperHero() { }  

        public SuperHero(List<string> splitData)
        {
            if (splitData is null || splitData.Count < 12)
                return;

            int idx = -1;
            Name = splitData[++idx];
            Identity = splitData[++idx];
            BirthPlace = splitData[++idx];
            Publisher = splitData[++idx];
            Height = splitData[++idx];
            Weight = splitData[++idx];
            Gender = splitData[++idx];
            Appearance = splitData[++idx];
            Eye = splitData[++idx];
            Hair = splitData[++idx];
            Strength = splitData[++idx];
            Intelligence = splitData[++idx];

            Valid = true;
        }

        public string Name { get; set; } = string.Empty;
        public string Identity { get; set; } = string.Empty;
        public string BirthPlace { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Appearance { get; set; } = string.Empty;
        public string Eye { get; set; } = string.Empty;
        public string Hair { get; set; } = string.Empty;
        public string Strength { get; set; } = string.Empty;
        public string Intelligence { get; set; } = string.Empty;

        public int Id { get; set; }

        public override string ToString() => Name;
    }
}
