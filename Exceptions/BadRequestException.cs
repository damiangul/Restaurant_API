using System;

namespace Restaurant_API.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
            
        }
    }
}