using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Puzzle04
{
    // https://adventofcode.com/2020/day/4
    static class Program
    {
        // Puzzle 1: 222
        // Puzzle 2: 140
        static void Main()
        {
            Console.WriteLine($"Puzzle 1: {Puzzle1()}");
            Console.WriteLine($"Puzzle 2: {Puzzle2()}");
        }

        private static long Puzzle1()
        {
            return LoadInput().Parse().Count(p => p.IsValid1());
        }

        private static long Puzzle2()
        {
            return LoadInput().Parse().Count(p => p.IsValid2());
        }

        private static IEnumerable<Passport> Parse(this string input)
        {
            return input.Split("\n\n")
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => new Passport(s.Replace('\n', ' ')));
        }

        public class Passport
        {
            public Passport(string text)
            { 
                foreach (var kv in text
                    .Split(' ')
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(s => s.Split(':')))
                {
                    var key = kv[0];
                    var value = kv[1];
                    GetType().GetProperty(key).SetValue(this, value);
                }
            }

            public string byr { get; set; }
            public string iyr { get; set; }
            public string eyr { get; set; }
            public string hgt { get; set; }
            public string hcl { get; set; }
            public string ecl { get; set; }
            public string pid { get; set; }
            public string cid { get; set; }

            public bool IsValid1()
            {
                return !string.IsNullOrEmpty(byr)
                       && !string.IsNullOrEmpty(iyr)
                       && !string.IsNullOrEmpty(eyr)
                       && !string.IsNullOrEmpty(hgt)
                       && !string.IsNullOrEmpty(hcl)
                       && !string.IsNullOrEmpty(ecl)
                       && !string.IsNullOrEmpty(pid);
            }
            public bool IsValid2()
            {
                return IsValid_byr()
                       && IsValid_iyr()
                       && IsValid_eyr()
                       && IsValid_hgt()
                       && IsValid_hcl()
                       && IsValid_ecl()
                       && IsValid_pid();
            }

            private bool IsValid_byr()
            {
                return IsInRange(byr, 1920, 2002);
            }
            private bool IsValid_iyr()
            {
                return IsInRange(iyr, 2010, 2020);
            }
            private bool IsValid_eyr()
            {
                return IsInRange(eyr, 2020, 2030);
            }
            private bool IsValid_hgt()
            {
                if (string.IsNullOrEmpty(hgt))
                {
                    return false;
                }

                if (hgt.EndsWith("cm"))
                {
                    return IsInRange(hgt.Replace("cm", string.Empty), 150, 193);
                }

                if (hgt.EndsWith("in"))
                {
                    return IsInRange(hgt.Replace("in", string.Empty), 59, 76);
                }

                return false;
            }
            private bool IsValid_hcl()
            {
                if (string.IsNullOrEmpty(hcl))
                {
                    return false;
                }

                return Regex.Match(hcl, @"^\#[a-z0-9]{6}\b", RegexOptions.ExplicitCapture).Success;
            }
            private bool IsValid_pid()
            {
                if (string.IsNullOrEmpty(pid))
                {
                    return false;
                }

                return Regex.Match(pid, @"^[0-9]{9}\b", RegexOptions.ExplicitCapture).Success;
            }
            private bool IsValid_ecl()
            {
                if (string.IsNullOrEmpty(ecl))
                {
                    return false;
                }

                return Regex.Match(ecl, @"^(amb|blu|brn|gry|grn|hzl|oth)\b", RegexOptions.ExplicitCapture).Success;
            }

            private bool IsInRange(string value, int min, int max)
            {
                if (int.TryParse(value, out var number))
                {
                    return number >= min && number <= max;
                }

                return false;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                foreach (var prop in GetType().GetProperties())
                {
                    var value = prop.GetValue(this);

                    if (value != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(' ');
                        }

                        sb.Append(prop.Name);
                        sb.Append(':');
                        sb.Append(value);
                    }

                }

                return sb.ToString();
            }
        }

        private static string LoadInput()
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Puzzle04.input.txt");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
