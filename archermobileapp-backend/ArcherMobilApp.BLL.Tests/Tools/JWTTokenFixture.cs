using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ArcherMobilApp.Infrastracture;
using ArcherMobilApp.Infrastracture.Security.Jwt;
using Microsoft.Extensions.Configuration;

namespace ArcherMobilApp.BLL.Tests.Tools
{
    public class JwtTokenFixture
    {
        private readonly JwtToken _token;

        public JwtToken Token => _token;

        public JwtTokenFixture()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            _token = new TokenGenerator(configuration).GenerateToken("test@gmail.com", new List<string>() { "Admins" });
        }
    }
}
