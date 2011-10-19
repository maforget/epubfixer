using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ePubFixer
{
    public static class SettingsArray
    {
        private static char delimeter = '|';

        public static string AddSettings(this string Settings, string value)
        {
            List<string> load = Settings.LoadSettings().ToList();
            load.Add(value);
            return load.SaveSettings();
        }

        public static IEnumerable<string> LoadSettings(this string value)
        {
            IEnumerable<string> result = value.Split(delimeter).Where(x=>!string.IsNullOrEmpty(x)).Select(x => x);
            return result;
        }

        public static string SaveSettings(this IEnumerable<string> values)
        {
            string result = String.Join(delimeter.ToString(), values.Where(x => !string.IsNullOrEmpty(x))
                .Select(i => i.ToString()).ToArray());
            return result;
        }
    }
}
