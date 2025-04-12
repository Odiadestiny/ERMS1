using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERMS.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string? Title { get; set; }
        
        public string? Description { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
        // A project can be associated with many employees.
        // (This might be a many-to-many relationship in a full implementation.)
        public ICollection<Employee>? Employees { get; set; }
    }
}
