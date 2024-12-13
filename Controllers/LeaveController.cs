using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    namespace WebApplication1.Controllers
    {
        public class LeaveController : Controller
        {
            private readonly HRMSDbContext _context;

            public LeaveController(HRMSDbContext context)
            {
                _context = context;
            }

            // GET: Leave
            public async Task<IActionResult> Index()
            {
                var leaves = await _context.Leaves.Include(l => l.Employee).ToListAsync();
                return View(leaves);
            }

            // GET: Leave/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var leave = await _context.Leaves
                    .Include(l => l.Employee)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (leave == null)
                {
                    return NotFound();
                }

                return View(leave);
            }

            // GET: Leave/Create
            //public IActionResult Create()
            //{
            //    var employees = _context.Employees
            //        .ToList();

            //    ViewBag.Employees = employees;
            //    return View();
            //}

            //// POST: Leave/Create
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> Create([Bind("EmployeeId,StartDate,EndDate,Reason,Status")] CreateLeaveDTO leaveDTO)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var leave = new Leave
            //        {
            //            EmployeeId = leaveDTO.EmployeeId,
            //            StartDate = leaveDTO.StartDate,
            //            EndDate = leaveDTO.EndDate,
            //            Reason = leaveDTO.Reason,
            //            Status = leaveDTO.Status
            //        };

            //        _context.Add(leave);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    ViewBag.Employees = new SelectList(_context.Employees, "Id", "FirstName", leaveDTO.EmployeeId);
            //    return View(leaveDTO);
            //}
            [HttpGet]
            public IActionResult Create()
            {
                // Populate the dropdown with employee data
                ViewBag.Employees = new SelectList(
                    _context.Employees.Select(e => new
                    {
                        e.Id,
                        FullName = e.FirstName + " " + e.LastName
                    }).ToList(),
                    "Id",
                    "FullName"
                );

                // Return the view
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("EmployeeId,StartDate,EndDate,Reason,Status")] CreateLeaveDTO leaveDTO)
            {
                if (ModelState.IsValid)
                {
                    // Map DTO to the Leave entity
                    var leave = new Leave
                    {
                        EmployeeId = leaveDTO.EmployeeId,
                        StartDate = leaveDTO.StartDate,
                        EndDate = leaveDTO.EndDate,
                        Reason = leaveDTO.Reason,
                        Status = leaveDTO.Status
                    };

                    // Add the leave to the database
                    _context.Add(leave);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                // Re-populate ViewBag.Employees in case of validation errors
                ViewBag.Employees = new SelectList(
                    _context.Employees.Select(e => new
                    {
                        e.Id,
                        FullName = e.FirstName + " " + e.LastName
                    }).ToList(),
                    "Id",
                    "FullName",
                    leaveDTO.EmployeeId
                );

                return View(leaveDTO);
            }


            // GET: Leave/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var leave = await _context.Leaves.FindAsync(id);
                if (leave == null)
                {
                    return NotFound();
                }
                ViewBag.Employees = new SelectList(_context.Employees, "Id", "FirstName", leave.EmployeeId);
                return View(leave);
            }

            // POST: Leave/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,StartDate,EndDate,Reason,Status")] Leave leave)
            {
                if (id != leave.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(leave);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LeaveExists(leave.Id))
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
                ViewBag.Employees = new SelectList(_context.Employees, "Id", "FirstName", leave.EmployeeId);
                return View(leave);
            }

            // GET: Leave/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var leave = await _context.Leaves
                    .Include(l => l.Employee)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (leave == null)
                {
                    return NotFound();
                }

                return View(leave);
            }

            // POST: Leave/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var leave = await _context.Leaves.FindAsync(id);
                _context.Leaves.Remove(leave);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool LeaveExists(int id)
            {
                return _context.Leaves.Any(e => e.Id == id);
            }
        }
    }
}
