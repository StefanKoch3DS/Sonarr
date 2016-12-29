using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NzbDrone.Common.Disk;
using NzbDrone.Common.EnvironmentInfo;

namespace NzbDrone.Mono.EnvironmentInfo.VersionAdapters
{
    public class MacOsVersionAdapter : IOsVersionAdapter
    {
        private static readonly Regex DarwinVersionRegex = new Regex("<string>(?<version>10\\.\\d{1,2}\\.?\\d{0,2}?)<\\/string>",
            RegexOptions.Compiled |
            RegexOptions.IgnoreCase
        );

        private readonly IDiskProvider _diskProvider;

        public MacOsVersionAdapter(IDiskProvider diskProvider)
        {
            _diskProvider = diskProvider;
        }

        public OsVersionModel Read()
        {
            var version = "10.0";

            var allFiles = _diskProvider.GetFiles("/System/Library/CoreServices/", SearchOption.TopDirectoryOnly);

            var versionFiles = allFiles.Where(c => c.EndsWith("-Version.plist")).ToList();

            foreach (var file in versionFiles)
            {
                var text = _diskProvider.ReadAllText(file);
                var match = DarwinVersionRegex.Match(text);

                if (match.Success)
                {
                    version = match.Groups["version"].Value;
                }
            }

            return new OsVersionModel("macOS", version);
        }

        public bool Enabled => OsInfo.IsOsx;
    }
}