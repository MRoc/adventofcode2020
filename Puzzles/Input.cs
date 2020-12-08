
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzles
{
    public static class Input
    {
        public static string Load(string name)
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(name);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static IReadOnlyList<string> LoadLines(string name)
        {
            return Load(name).Split('\n').Where(l => !string.IsNullOrEmpty(l)).ToArray();
        }
    }
}
