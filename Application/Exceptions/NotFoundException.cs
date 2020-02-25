using System;

namespace Tymish.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityType, object key)
            : base($"Entity \"{entityType}\" with key ({key}) was not found.")
        {}
    }
}