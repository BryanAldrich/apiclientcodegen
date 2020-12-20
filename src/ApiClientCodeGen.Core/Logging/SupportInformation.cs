using System;
using System.Diagnostics.CodeAnalysis;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Extensions;

namespace ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Logging
{
    public static class SupportInformation
    {
        public static string GetAnonymousName()
            => GetSupportKey().Substring(0, 7);

        public static string GetSupportKey()
            => $"{Environment.UserName}@{GetMachineName()}".ToSha256();

        [ExcludeFromCodeCoverage]
        private static string GetMachineName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch
            {
                return "localhost";
            }
        }
    }
}