using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BTL_WNC.Models
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string Content { get; set; }
        public DateTime UpdateAt { get; set; }
        [Required(ErrorMessage = "Due Date is required")]
        public DateTime DueDate { get; set; }
        [Required(ErrorMessage = "Status is required")]
        [Column(TypeName = "nvarchar(50)")]

        public string Status { get; set; }
        
        public Guid ProjectId { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>(); // nhieu ng dùng cho task

        //public Guid UserId { get; set; }
        //[ForeignKey("ProjectId")]
        //public Project Project { get; set; } // Đối tượng Project liên kết
        // Mối quan hệ nhiều-nhiều với User thông qua UserTask.
        //public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }
}
