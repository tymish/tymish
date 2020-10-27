using System;

namespace Tymish.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User()
        {
            this.Email = "";
            this.Password = "";
        }
    }
}