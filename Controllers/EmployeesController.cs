using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HRMSDbContext _context;

        public EmployeesController(HRMSDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Include(e => e.Department).Include(e => e.Position).ToListAsync();
            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            var departments = _context.Departments.ToList();
            var positions = _context.Positions.ToList();

            if (departments == null || !departments.Any())
            {
                // Handle this case (e.g., log the error or return an error message)
            }

            if (positions == null || !positions.Any())
            {
                // Handle this case (e.g., log the error or return an error message)
            }

            ViewBag.Departments = departments;
            ViewBag.Positions = positions;

            return View();
        }


        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEmployeeDTO employeeDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Employee entity
                var employee = new Employee
                {
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    Email = employeeDto.Email,
                    DateOfBirth = employeeDto.DateOfBirth,
                    HireDate = employeeDto.HireDate,
                    PhoneNumber = employeeDto.PhoneNumber,
                    Address = employeeDto.Address,
                    Salary = employeeDto.Salary,
                    DepartmentId = employeeDto.DepartmentId,
                    PositionId = employeeDto.PositionId
                };

                _context.Employees.Add(employee);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // If ModelState is not valid, re-display the form with validation messages
            return View(employeeDto);
        }


        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            // Fetch the departments and positions for the dropdowns
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Positions = _context.Positions.ToList();

            // Map the employee object to an EditEmployeeDTO
            var response = new EditEmployeeDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                PositionId = employee.PositionId
            };

            return View(response); // Pass the DTO to the view
        }


        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEmployeeDTO model)
        {
            if (!ModelState.IsValid)
            {
                // Reload the departments and positions if the form submission fails validation
                ViewBag.Departments = _context.Departments.ToList();
                ViewBag.Positions = _context.Positions.ToList();
                return View(model); // Return the view with validation messages
            }

            // If model is valid, proceed with the update operation
            var employee = await _context.Employees.FindAsync(model.Id);
            if (employee == null)
            {
                return NotFound();
            }

            // Map the DTO back to the employee entity
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.DateOfBirth = model.DateOfBirth;
            employee.HireDate = model.HireDate;
            employee.PhoneNumber = model.PhoneNumber;
            employee.Address = model.Address;
            employee.Salary = model.Salary;
            employee.DepartmentId = model.DepartmentId;
            employee.PositionId = model.PositionId;

            _context.Update(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the index after successful update
        }

        // GET: Employees/Details/5
        public IActionResult Details(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Department) // Include navigation properties if needed
                .Include(e => e.Position)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
    

    // GET: Employees/Delete/5
    public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
