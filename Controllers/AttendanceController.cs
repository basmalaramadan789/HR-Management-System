using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly HRMSDbContext _context;

        public AttendanceController(HRMSDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            // Fetch all attendance records along with the related Employee information
            var attendanceList = await _context.Attendances
                .Include(a => a.Employee) // Include the related Employee
                .ToListAsync();

            return View(attendanceList);
        }






        // GET: Attendance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

       
        public async Task<IActionResult> Create()
        {
            
            var employees = await _context.Employees.ToListAsync();

            
            ViewBag.Employees = new SelectList(employees, "Id", "FirstName");

            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttendanceViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var attendance = new Attendance
                {
                    EmployeeId = model.EmployeeId,
                    CheckInTime = model.CheckInTime,
                    CheckOutTime = model.CheckOutTime,
                    IsAbsent = model.IsAbsent
                };

               
                _context.Add(attendance);
                await _context.SaveChangesAsync();

                
                return RedirectToAction(nameof(Index));
            }

            
            var employees = await _context.Employees.ToListAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "FirstName");

            return View(model);
        }
             
              

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.ToListAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "FirstName");

            return View(attendance);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Attendance attendance)
              {

            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var employees = await _context.Employees.ToListAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "FirstName");

            return View(attendance);
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(a => a.Id == id);
        }




        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }



       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AttendenceConfirmed()
        {
            return View();
        }

    }
}
