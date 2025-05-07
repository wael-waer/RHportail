namespace PortailRH.API.Models.DataBase
{
    public class PaymentPolicy: EntityBase
    {
        public decimal TaxRate { get; set; }
        public decimal SocialSecurityRate { get; set; }
        public decimal OtherDeductions { get; set; }
        public int PaymentDay { get; set; }
        public int ExcessDayPayment { get; set; }
        public int AllowedDays { get; set; }

        public int SickLeave { get; set; }
        public int PaidLeave { get; set; }
        public int UnpaidLeave { get; set; }
        public int Bereavement { get; set; }
        public int PersonalReasons { get; set; }
        public int Maternity { get; set; }
        public int Paternity { get; set; }
        public int RTT { get; set; }
        public int Other { get; set; }
    }
}
