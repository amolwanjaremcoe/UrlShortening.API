using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortening.Model.Exception
{
    public class KeyGenerationFailedException : System.Exception
    {
        public KeyGenerationFailedException() : base($"Short key generation failed.")
        {
        }
        public KeyGenerationFailedException(int maxAttemps) : base($"Short key generation failed after {maxAttemps} attempts")
        {
        }
    }
}
