using BTL_WNC.Models;
using System.ComponentModel.DataAnnotations;

namespace BTL_WNC.ViewModels
{
    public class TaskViewModel
    {
        
            public Project Project { get; set; } // Không nên yêu cầu nếu không có thông tin trong view
            public Models.Task Task { get; set; } = new Models.Task();
            public List<Models.Task> DoingTasks { get; set; } = new List<Models.Task>(); // Khởi tạo để tránh lỗi null
            public List<Models.Task> DoneTasks { get; set; } = new List<Models.Task>(); // Khởi tạo để tránh lỗi null
            public List<User> Users { get; set; } = new List<User>();
            public List<Guid> SelectedUsers { get; set; } = new List<Guid>(); // Đảm bảo không bị yêu cầu nếu không có
       

        //public Project Project { get; set; }
        //public Models.Task Task { get; set; } = new Models.Task();
        //public List<Models.Task> DoingTasks { get; set; }
        //public List<Models.Task> DoneTasks { get; set; }
        //public List<User> Users { get; set; } = new List<User>();
        ////public List<Guid> SelectedUserIds { get; set; } = new List<Guid>();
        //public List<Guid> SelectedUsers { get; set; } = new List<Guid>();
    }
}
