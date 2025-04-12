using System.ComponentModel.DataAnnotations;

namespace ERMS.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }
        
        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }
        
        public string? Role { get; set; }
    }
}
