using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ERMS.Models;
using ERMS.Repositories;
using Microsoft.EntityFrameworkCore;  

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
            // Retrieve all employees via the repository.
            var employees = await _repository.GetAllEmployeesAsync();
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                // If no id is provided, return a 404 Not Found response.
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
        public IActionResult Create()
        {
            // Populate the Role drop-down list.
            ViewBag.RoleList = new SelectList(_availableRoles);
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,Email,Role")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            // Ensure the role list is repopulated if the model state is invalid.
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
            // Populate the role drop-down with the current role selected.
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
            // Repopulate the role list if model state is invalid.
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
