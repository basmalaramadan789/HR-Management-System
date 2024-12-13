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



            return View(departments);
        }

        //public IActionResult Index()
        //{
        //    var departments = _context.Departments
        //        .Include(d => d.Employees)
        //        .Select(d => new DepartmentViewModel
        //        {
        //            Name = d.Name,
        //            EmployeeCount = d.Employees.Count()
        //        })
        //        .ToList();

            ////    // Existing ViewBag values
            ////    ViewBag.EmployeeCount = _context.Employees.Count();
            ////    ViewBag.DepartmentCount = _context.Departments.Count();
            ////    ViewBag.PositionCount = _context.Positions.Count();

            ////    // New data for charts
            ////    ViewBag.LastMonthPayroll = _context.Payrolls
            ////        .Where(p => p.PaymentDate.Month == DateTime.Now.AddMonths(-1).Month && p.PaymentDate.Year == DateTime.Now.Year)
            ////        .Sum(p => p.NetAmount);

            ////    ViewBag.TopEmployees = _context.Employees
            ////        .OrderByDescending(e => e.Salary)
            ////        .Take(5)
            ////        .Select(e => new { Name = e.FirstName + " " + e.LastName, e.Salary })
            ////        .ToList();

            ////    ViewBag.TopDepartment = _context.Departments
            ////        .Select(d => new { d.Name, EmployeeCount = d.Employees.Count() })
            ////        .OrderByDescending(d => d.EmployeeCount)
            ////        .FirstOrDefault();

            ////    return View(departments);
            ////}






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
