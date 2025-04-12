using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERMS.Models
{
    public enum TaskPriority { Low, Medium, High }
    public enum TaskStatus { NotStarted, InProgress, Completed }

    public class TaskItem
    {
        public int TaskItemId { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }
        
        public string? Description { get; set; }
        
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        
        // Foreign key for the related Project.
        [Required]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        
        // Direct link to the Employee assigned to the task.
        // Use a foreign key attribute to clarify the relationship.
        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
