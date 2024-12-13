using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class CreateLeaveDTO
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }
    }
}
