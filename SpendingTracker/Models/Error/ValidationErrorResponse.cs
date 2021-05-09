using System.Collections.Generic;

namespace SpendingTracker.Models.Error
{
    public class ValidationErrorResponse
    {
        public Dictionary<string, string> ErrorMessages { get; set; }
    }
}
