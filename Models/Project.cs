using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERMS.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        
        [Required]
        public string? Title { get; set; }
        
        public string? Description { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
        // A project can be associated with many employees (for simplicity, using a list)
        public ICollection<Employee>? Employees { get; set; }
    }
}
