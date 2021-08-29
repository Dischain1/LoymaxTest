﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Panronimic { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}