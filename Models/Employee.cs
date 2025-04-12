using System.ComponentModel.DataAnnotations;

namespace ERMS.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string? FirstName { get; set; }
        
        [Required]
        public string? LastName { get; set; }
        
        [Required, EmailAddress]
        public string? Email { get; set; }
        
        public string? Role { get; set; }
    }
}
