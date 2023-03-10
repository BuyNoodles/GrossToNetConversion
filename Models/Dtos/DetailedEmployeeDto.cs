namespace Models.Dtos
{
    public class DetailedEmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public decimal GrossIncome { get; set; }
        public string WorkPosition { get; set; }
        public IncomeDetailsDto IncomeDetails { get; set; }
    }
}
