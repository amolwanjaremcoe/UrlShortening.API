using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortening.Model.Exception
{
    public class InvalidUrlException : System.Exception
    {
        public InvalidUrlException() : base($"Invalid Url value was provided.")
        {
        }
        public InvalidUrlException(string url) : base($"Invalid Url - {url} was provided.")
        {
        }
    }
}
