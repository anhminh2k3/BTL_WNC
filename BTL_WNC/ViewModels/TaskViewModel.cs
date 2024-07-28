using BTL_WNC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BTL_WNC.ViewModels
{
    public class TaskViewModel
    {

        [Required(ErrorMessage = "Project is required.")]
        public Project Project { get; set; } // Đảm bảo Project không bị yêu cầu trong trường hợp không có dữ liệu
        public Models.Task Task { get; set; } = new Models.Task();
        public List<Models.Task> DoingTasks { get; set; } = new List<Models.Task>(); // Khởi tạo để tránh lỗi null
        public List<Models.Task> DoneTasks { get; set; } = new List<Models.Task>(); // Khởi tạo để tránh lỗi null
            public List<User> Users { get; set; } = new List<User>();
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<Guid> SelectedUsers { get; set; } = new List<Guid>(); // Đảm bảo không bị yêu cầu nếu không có
    }
}
