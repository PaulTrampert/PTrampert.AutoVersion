using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PTrampert.AutoVersion
{
    public class SemVer
    {
        public int Major { get; set; }

        public int Minor { get; set; }

        public int Patch { get; set; }

        public string Suffix { get; set; }

        public bool PreReleaseVersion => !string.IsNullOrWhiteSpace(Suffix);

        public static SemVer Parse(string str)
        {
            var match = Regex.Match(str, @"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(-(?<suffix>\S+))?");
            if (!match.Success)
            {
                throw new InvalidOperationException($"{nameof(str)} must be a valid SemVer string.");
            }
            return new SemVer
            {
                Major = int.Parse(match.Groups["major"].Value),
                Minor = int.Parse(match.Groups["minor"].Value),
                Patch = int.Parse(match.Groups["patch"].Value),
                Suffix = string.IsNullOrWhiteSpace(match.Groups["suffix"].Value) ? null : match.Groups["suffix"].Value
            };
        }

        public override string ToString()
        {
            var result = $"{Major}.{Minor}.{Patch}";
            if (!string.IsNullOrWhiteSpace(Suffix))
            {
                result += $"-{Suffix}";
            }
            return result;
        }
    }
}
