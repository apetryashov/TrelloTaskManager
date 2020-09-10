using System;
using System.Text;
using JetBrains.Annotations;

namespace TaskManager.Common
{
    [PublicAPI]
    public class MongoConnectionProperties
    {
        public string[] Hosts { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string GetConnectionString()
        {
            if (Hosts == null || Hosts.Length == 0)
                throw new ArgumentException("Hosts should contain at least one host");

            if (string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Username) ||
                !string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(Username))
                throw new ArgumentException("Username and password should both be present");

            var stringBuilder = new StringBuilder("mongodb+srv://");

            if (!string.IsNullOrEmpty(Username))
                stringBuilder.Append($"{Username}:{Password}@");

            stringBuilder.Append(string.Join(',', Hosts));

            if (!string.IsNullOrEmpty(Database))
                stringBuilder.Append($"/{Database}");

            stringBuilder.Append("?retryWrites=true&w=majority");

            return stringBuilder.ToString();
        }
    }
}