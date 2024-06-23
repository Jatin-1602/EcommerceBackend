﻿using System.ComponentModel.DataAnnotations;

namespace EcommerceMariaDB.Models
{
    public class User
    {
        public int Id { get; set; }

        [RegularExpression("^[\\p{L} .'-]+$", ErrorMessage = "Invalid Name")]
        public string Name { get; set; } = string.Empty;

        [RegularExpression("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }
}
