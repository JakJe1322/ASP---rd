﻿namespace ReaderDiary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Uložené hashované heslo
    }

}
