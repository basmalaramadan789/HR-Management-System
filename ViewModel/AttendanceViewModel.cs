namespace WebApplication1.ViewModel;

public class AttendanceViewModel
{
    public int EmployeeId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime CheckOutTime { get; set; }
    public bool IsAbsent { get; set; }
}
