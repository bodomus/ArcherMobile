using System;

namespace ArcherMobilApp.Core.Exceptions
{
    public class UserNotFoundException : InvalidOperationException
    {
        public UserNotFoundException(string username) : base($"User '{username}' not found.") { }
    }
}

