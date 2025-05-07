namespace PortailRH.API.Models.DataBase
{
    public class Payslip : EntityBase
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;
        public int PaymentPolicyId { get; set; }
        public PaymentPolicy PaymentPolicy { get; set; } = default!;
        public decimal BasicSalary { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal SocialSecurityDeduction { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}
