using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PTrampert.AutoVersion.Test
{
    public class SemVerTests
    {
        [Theory]
        [InlineData("1.2.3-pre1", 1, 2, 3, "pre1")]
        [InlineData("2.3.5", 2, 3, 5, null)]
        public void CanParseSemverString(string input, int major, int minor, int patch, string suffix)
        {
            var result = SemVer.Parse(input);
            Assert.Equal(result.Major, major);
            Assert.Equal(result.Minor, minor);
            Assert.Equal(result.Patch, patch);
            Assert.Equal(result.Suffix, suffix);
        }

        [Fact]
        public void ThrowsWhenNotMatch()
        {
            Assert.Throws<InvalidOperationException>(() => SemVer.Parse("this is not a semver string"));
        }

        [Fact]
        public void ToStringOutputsTheSemverString()
        {
            var semver = SemVer.Parse("1.2.3-pre1");
            Assert.Equal("1.2.3-pre1", semver.ToString());
        }
    }
}
