﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Models
{
    public class Student : Person
    {
        public int? Id { get; set; }
        public int? Grade { get; set; }
    }
}
