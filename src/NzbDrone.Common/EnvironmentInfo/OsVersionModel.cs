namespace NzbDrone.Common.EnvironmentInfo
{
    public class OsVersionModel
    {
        public OsVersionModel(string name, string version, string fullName = null)
        {
            Name = name;
            Version = version;

            if (string.IsNullOrWhiteSpace(fullName))
            {
                fullName = $"{name} {version}";
            }

            FullName = fullName;
        }

        public string Name { get; }
        public string FullName { get; }
        public string Version { get; }
    }
}