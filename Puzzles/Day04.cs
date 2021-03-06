﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 222
    // Puzzle 2: 140
    public static class Day04
    {
        public static long Puzzle1()
        {
            return Input.Load("Puzzles.Input.input04.txt").Parse().Count(p => p.IsValid1());
        }

        public static long Puzzle2()
        {
            return Input.Load("Puzzles.Input.input04.txt").Parse().Count(p => p.IsValid2());
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
                    .Select(s => s.Split(':'))
                    .Where(s => s[0] != "cid"))
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
                return hcl is {} && Regex.Match(hcl, @"^\#[a-z0-9]{6}\b", RegexOptions.ExplicitCapture).Success;
            }
            private bool IsValid_pid()
            {
                return pid is {} && Regex.Match(pid, @"^[0-9]{9}\b", RegexOptions.ExplicitCapture).Success;
            }
            private bool IsValid_ecl()
            {
                return ecl is {} && Regex.Match(ecl, @"^(amb|blu|brn|gry|grn|hzl|oth)\b", RegexOptions.ExplicitCapture).Success;
            }

            private bool IsInRange(string value, int min, int max)
            {
                if (int.TryParse(value, out var number))
                {
                    return number >= min && number <= max;
                }

                return false;
            }
        }
    }
}
