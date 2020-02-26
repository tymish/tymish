using System;

namespace Tymish.Application.Exceptions
{
    public class CannotCreateException : Exception
    {
        public CannotCreateException(string entityType, string because)
            : base($"Cannot create entity \"{entityType}\" {because}.")
        {}
    }
}