using NzbDrone.Common.EnvironmentInfo;

namespace NzbDrone.Common.Http
{
    public interface IUserAgentBuilder
    {
        string UserAgent { get; }
        string UserAgentSimplified { get; }
    }

    public class UserAgentBuilder : IUserAgentBuilder
    {
        public string UserAgent { get; }
        public string UserAgentSimplified { get; }

        public string GetUserAgent(bool full)
        {
            if (full)
            {
                return UserAgent;
            } 

            return UserAgentSimplified;
        }




        public UserAgentBuilder(IPlatformInfo platformInfo, IOsInfo osInfo)
        {
            var osName = osInfo.Name.ToLower();
            var osVersion = osInfo.Version.ToLower();
            var platformName = PlatformInfo.Platform.ToString().ToUpper();

            UserAgent = $"Sonarr/{BuildInfo.Version} ({OsInfo.Os}; {osName} {osVersion}) {platformName} {platformInfo.Version}";
            UserAgentSimplified = $"Sonarr/{BuildInfo.Version.ToString(2)}";
        }
    }
}