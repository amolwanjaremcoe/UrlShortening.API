using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortening.Model
{
    public class ServerConfig
    {
        public int CodeGenerationMaxAttempts { get; set; }

        public int ShortcodeExpirationYear { get; set; }

        public int CacheTimeoutHour { get; set; }
    }
}
