﻿using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}
