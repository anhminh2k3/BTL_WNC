using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WNC.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string Decription { get; set; }
        public DateTime startTime { get; set; }
        // Mối quan hệ 1-n với Task
        //public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
