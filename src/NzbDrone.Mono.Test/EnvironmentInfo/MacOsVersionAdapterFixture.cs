using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Common.Disk;
using NzbDrone.Mono.EnvironmentInfo;
using NzbDrone.Mono.EnvironmentInfo.VersionAdapters;
using NzbDrone.Test.Common;

namespace NzbDrone.Mono.Test.EnvironmentInfo
{
    [TestFixture]
    public class MacOsVersionAdapterFixture : TestBase<MacOsVersionAdapter>
    {
        [TestCase("10.8.0")]
        [TestCase("10.8")]
        [TestCase("10.8.1")]
        [TestCase("10.11.20")]
        public void should_get_version_info(string versionString)
        {
            var fileContent = File.ReadAllText(GetTestPath("Files/macOS/SystemVersion.plist")).Replace("10.0.0", versionString);

            const string plistPath = "/System/Library/CoreServices/System-Version.plist";

            Mocker.GetMock<IDiskProvider>()
                .Setup(c => c.GetFiles("/System/Library/CoreServices/", SearchOption.TopDirectoryOnly))
                .Returns(new[] { plistPath });

            Mocker.GetMock<IDiskProvider>()
                .Setup(c => c.ReadAllText(plistPath))
                .Returns(fileContent);

            var versionName = Subject.Read();
            versionName.Version.Should().Be(versionString);
            versionName.Name.Should().Be("macOS");
            versionName.FullName.Should().Be("macOS " + versionString);
        }
    }
}