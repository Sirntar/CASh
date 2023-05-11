using System;
using System.Reflection;
using System.Globalization;

namespace CASh
{
    internal static class Version
    {
        public static readonly string __Ver = "v1.0.0";
        public static readonly string __Version = "Version: " + __Ver + " (build: " + Version.GetBuildDate(Assembly.GetExecutingAssembly()).ToLocalTime().ToString().Replace(".", "").Replace(":", "") + ")";

        private static DateTime GetBuildDate(Assembly assembly)
        {
            const string BuildVersionMetadataPrefix = "+build";

            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion != null)
            {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf(BuildVersionMetadataPrefix);
                if (index > 0)
                {
                    value = value.Substring(index + BuildVersionMetadataPrefix.Length);
                    if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                    {
                        return result;
                    }
                }
            }
            return default;
        }
    }
}
