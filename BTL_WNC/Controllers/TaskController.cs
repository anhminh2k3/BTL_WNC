using Microsoft.AspNetCore.Mvc;
using BTL_WNC.Models;
using BTL_WNC.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_WNC.Controllers
{
    public class TaskController : Controller
    {
        private readonly ProjectDbContext _context;

        public TaskController(ProjectDbContext context)
        {
            _context = context;
        }

        // Kiểm tra quyền admin của người dùng
        private bool IsAdmin()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            return user != null && user.RoleId == "role1";
        }

        public async Task<IActionResult> TaskView(Guid id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            var tasks = _context.Tasks.Include(t => t.Users).Where(t => t.ProjectId == project.Id);
            
            tasks = tasks.OrderByDescending(p => p.UpdateAt);
            var model = new TaskViewModel
            {
                Project = project,
                DoingTasks = tasks.Where(t => t.Status == "Doing").ToList(),
                DoneTasks = tasks.Where(t => t.Status == "Done").ToList(),
                Users = await _context.Users.ToListAsync(),
            };
            ViewBag.IsAdmin = IsAdmin();
            return View(model);

        }

        public async Task<IActionResult> AddTask(Guid projectId)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            var users = await _context.Users.ToListAsync();
            var taskViewModel = new TaskViewModel
            {
                Task = new Models.Task { ProjectId = projectId},
                Users = users,
            };
            return View(taskViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTask(TaskViewModel viewModel)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            var task = viewModel.Task;
            task.Id = Guid.NewGuid();
            task.UpdateAt = DateTime.Now;
            
            // Xử lý người dùng được chọn
            if (viewModel.SelectedUsers != null)
            {
                task.Users = new List<User>();
                foreach (var userId in viewModel.SelectedUsers)
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user != null)
                    {
                        task.Users.Add(user);
                    }
                }
            }
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("TaskView", new { id = task.ProjectId });
        }

        //public async Task<IActionResult> TaskView(Guid id)
        //{
        //    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }

        //    var doingTasks = await _context.Tasks
        //        .Include(t => t.Users)
        //        .Where(t => t.ProjectId == project.Id && t.Status == "Doing")
        //        .ToListAsync();

        //    var doneTasks = await _context.Tasks
        //        .Include(t => t.Users)
        //        .Where(t => t.ProjectId == project.Id && t.Status == "Done")
        //        .ToListAsync();

        //    var viewModel = new TaskViewModel
        //    {
        //        Project = project,
        //        DoingTasks = doingTasks,
        //        DoneTasks = doneTasks
        //    };

        //    return View(viewModel);
        //}

        
        // Hiển thị form chỉnh sửa task
        public async Task<IActionResult> EditTask(Guid id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            var task = await _context.Tasks.Include(t => t.Users).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            var viewModel = new TaskViewModel
            {
                Task = task,
                Users = await _context.Users.ToListAsync(),
                Project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == task.ProjectId),
                SelectedUsers = task.Users.Select(u => u.Id).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(TaskViewModel viewModel)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            // Debug để kiểm tra dữ liệu
            Console.WriteLine($"Task: {viewModel.Task}");
            Console.WriteLine($"Project: {viewModel.Project}");
            Console.WriteLine($"SelectedUsers: {viewModel.SelectedUsers}");
            try
            {
                if (viewModel.Task == null || viewModel.Project == null || viewModel.SelectedUsers == null)
                {
                    throw new Exception("Invalid data provided.");
                }
                //// Check if Due Date is today and set the status
                //if (viewModel.Task.DueDate.Date == DateTime.Today)
                //{
                //    viewModel.Task.Status = "Done";
                //}
                var task = await _context.Tasks.Include(t => t.Users).FirstOrDefaultAsync(t => t.Id == viewModel.Task.Id);
                if (task == null)
                {
                    throw new Exception("Task not found.");
                }

                // Cập nhật task
                task.Title = viewModel.Task.Title ?? task.Title;
                task.Content = viewModel.Task.Content ?? task.Content;
                task.DueDate = viewModel.Task.DueDate;
                task.Status = viewModel.Task.Status ?? task.Status;
                task.UpdateAt = DateTime.Now;

                // Cập nhật danh sách người dùng
                task.Users.Clear();
                if (viewModel.SelectedUsers != null)
                {
                    foreach (var userId in viewModel.SelectedUsers)
                    {
                        var user = await _context.Users.FindAsync(userId);
                        if (user != null)
                        {
                            task.Users.Add(user);
                        }
                    }
                }

                _context.Update(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("TaskView", new { id = viewModel.Project.Id });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                TempData["Error"] = ex.Message;
                return RedirectToAction("TaskView", new { id = viewModel.Project.Id });
            }
        }

        // Xóa một task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            if (taskId == Guid.Empty)
            {
                return BadRequest("Invalid task ID.");
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound();
            }

            var projectId = task.ProjectId;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("TaskView", new { id = projectId });
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus([FromBody] UpdateTaskStatusModel model)
        {
            Console.WriteLine("UpdateStatus called with TaskId: {0} and NewStatus: {1}", model.TaskId, model.NewStatus);

            if (ModelState.IsValid)
            {
                var task = _context.Tasks.FirstOrDefault(t => t.Id == model.TaskId);
                if (task != null)
                {
                    Console.WriteLine("Task found: {0}", task.Id);
                    task.Status = model.NewStatus;
                    _context.SaveChanges();
                    Console.WriteLine("Task status updated successfully for TaskId: {0}", model.TaskId);
                    return Ok(new { success = true });
                }
                Console.WriteLine("Task not found with TaskId: {0}", model.TaskId);
                return NotFound(new { success = false, message = "Task not found" });
            }
            Console.WriteLine("Model state is invalid: {0}", ModelState);
            return BadRequest(new { success = false, message = "Invalid model state" });
        }
    }

    public class UpdateTaskStatusModel
    {
        public Guid TaskId { get; set; }
        public string NewStatus { get; set; }
    }
}
