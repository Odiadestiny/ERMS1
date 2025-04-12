using System.ComponentModel.DataAnnotations;

namespace ERMS.Models
{
    public enum TaskPriority { Low, Medium, High }
    public enum TaskStatus { NotStarted, InProgress, Completed }

    public class TaskItem
    {
        public int TaskItemId { get; set; }

        [Required]
        public string? Title { get; set; }
        
        public string? Description { get; set; }
        
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        
        // Foreign key for the related Project
        [Required]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        
        // NEW: Direct link to the Employee assigned to the task.
        public int? EmployeeId { get; set; }  // Nullable in case some tasks are not yet assigned.
        public Employee? Employee { get; set; }
    }
}
