namespace WebApplication1.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }

        // Navigation property
        public Employee Employee { get; set; }
    }


    public enum LeaveStatus
    {
        Pending,
        Approved,
        Denied
    }
}
