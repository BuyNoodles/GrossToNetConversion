﻿using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public decimal GrossIncome { get; set; }
        public string WorkPosition { get; set; }
    }
}