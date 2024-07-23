using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WNC.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Password { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string RoleId { get; set; }

        // Mối quan hệ nhiều-nhiều với Task thông qua UserTask.
        //public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }
}
