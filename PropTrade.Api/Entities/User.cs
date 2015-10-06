using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropTrade.Api.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }        

        public DateTime Created { get; set; }

        public virtual UserProfile Profile { get; set; }
    }
}