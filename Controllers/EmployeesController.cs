using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // For [Authorize]
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERMS.Models;
using ERMS.Repositories;

namespace ERMS.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _repository;

        // Define the available roles.
        private readonly List<string> _availableRoles = new List<string>
        {
            "Admin", "Manager", "Employee"
        };

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _repository.GetAllEmployeesAsync();
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.RoleList = new SelectList(_availableRoles);
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,Email,Role")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.RoleList = new SelectList(_availableRoles, employee.Role);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.RoleList = new SelectList(_availableRoles, employee.Role);
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Email,Role")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateEmployeeAsync(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.EmployeeExists(employee.EmployeeId))
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
            ViewBag.RoleList = new SelectList(_availableRoles, employee.Role);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetEmployeeByIdAsync(id.Value);
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
            await _repository.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
