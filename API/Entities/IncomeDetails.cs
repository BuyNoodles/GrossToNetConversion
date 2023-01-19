using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class IncomeDetails
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Tax { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal PIO { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal HealthCare { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Unemployment { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal NetIncome { get; set; }
    }
}
