using Npgsql;
using System;

namespace Api.Helpers
{
    public static class NpgsqlExtensions
    {
        public static string ToNpgsqlBuilder(string url)
        {
            // 1. Limpieza inicial para prevenir el error del índice 0
            var cleanUrl = url.Trim(); 

            if (cleanUrl.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
                cleanUrl.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
            {
                // ... (El código completo de conversión con NpgsqlConnectionStringBuilder) ...
                
                var fixedUrl = cleanUrl.Replace("postgresql://", "postgres://", StringComparison.OrdinalIgnoreCase);
                if (!Uri.TryCreate(fixedUrl, UriKind.Absolute, out var uri))
                {
                    return cleanUrl;
                }

                var userInfo = uri.UserInfo.Split(':', 2);
                
                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = uri.IsDefaultPort ? 5432 : uri.Port,
                    Database = uri.AbsolutePath.TrimStart('/'),
                    Username = Uri.UnescapeDataString(userInfo[0]),
                    Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty, 
                    SslMode = SslMode.Require,
                    TrustServerCertificate = true,
                    Pooling = true
                };
                
                return builder.ConnectionString; 
            }
            
            return cleanUrl; 
        }
    }
}