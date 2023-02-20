﻿namespace JWTAuthentication.Models
{
    public class UserRegisterDTO
    {
        public string UserName = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmedPassword { get; set; } = string.Empty;
        
    }

}
