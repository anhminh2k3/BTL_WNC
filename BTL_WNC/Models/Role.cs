using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WNC.Models
{
    public class Role
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
    }
}
