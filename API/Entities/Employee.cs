using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public partial class Employee
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public decimal GrossIncome { get; set; }
    [Required]
    public string WorkPosition { get; set; }
}
