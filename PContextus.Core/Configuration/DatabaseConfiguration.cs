using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.Core.Configuration
{
    public sealed class DatabaseConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string DatabaseName { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
