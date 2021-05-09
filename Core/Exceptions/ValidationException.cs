using System;
using System.Collections.Generic;

namespace Core.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, string> ErrorMessages { get; set; }

        public ValidationException(Dictionary<string, string> errorMessages) 
            : base("There are validation errors for provided data.")
        {
            ErrorMessages = errorMessages;
        }
    }
}
