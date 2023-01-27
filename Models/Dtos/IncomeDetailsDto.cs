namespace Models.Dtos
{
    public class IncomeDetailsDto
    {
        public decimal Tax { get; set; }
        public decimal PIO { get; set; }
        public decimal HealthCare { get; set; }
        public decimal Unemployment { get; set; }
        public decimal NetIncome { get; set; }
    }
}
