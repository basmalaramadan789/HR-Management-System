using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly HRMSDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, HRMSDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var departments = _context.Departments
                .Include(d => d.Employees)
                .Select(d => new DepartmentViewModel
                {
                    Name = d.Name,
                    EmployeeCount = d.Employees.Count()
                })
                .ToList();

            ViewBag.EmployeeCount = _context.Employees.Count();
            ViewBag.DepartmentCount = _context.Departments.Count();
            ViewBag.PositionCount = _context.Positions.Count();



            // Get total salary for last month
            var totalSalary = _context.Payrolls
                .Where(p => p.PaymentDate.Month == DateTime.Now.Month - 1)
                .Sum(p => p.NetAmount);

            // Retrieve departments with their employee counts
            var departmentss = _context.Departments
                .Include(d => d.Employees)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    EmployeeCount = d.Employees.Count
                })
                .ToList();

            // Store department names and employee counts in ViewBag
            ViewBag.DepartmentNames = departments.Select(d => d.Name).ToList();
            ViewBag.EmployeeCounts = departments.Select(d => d.EmployeeCount).ToList();
            ViewBag.TotalSalary = totalSalary;


            // Retrieve the employee with the highest salary
            var highestSalaryEmployee = _context.Employees
                .OrderByDescending(e => e.Salary)
                .FirstOrDefault();

            // Pass the data to the view using ViewBag
            ViewBag.EmployeeName = highestSalaryEmployee?.FirstName + " " + highestSalaryEmployee?.LastName;
            ViewBag.HighestSalary = highestSalaryEmployee?.Salary;



            // Define the late threshold time (7:00 AM)
            TimeSpan lateThreshold = new TimeSpan(7, 0, 0);

            // Get today's date
            DateTime today = DateTime.Today;

            // Retrieve the number of late employees today
            var lateEmployeeCount = _context.Attendances
                .Where(a => a.CheckInTime.Date == today && a.CheckInTime.TimeOfDay > lateThreshold)
                .Count();

            
            var absentEmployeeCount = _context.Attendances
                .Where(a => a.IsAbsent && a.CheckInTime.Date == today)
                .Count();

           
            ViewBag.LateEmployeeCount = lateEmployeeCount;
            ViewBag.AbsentEmployeeCount = absentEmployeeCount;



            return View(departments);
        }

     
              



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
}
