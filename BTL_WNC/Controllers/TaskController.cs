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

        public async Task<IActionResult> TaskView(Guid id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            var doingTasks = await _context.Tasks
                .Include(t => t.Users)
                .Where(t => t.ProjectId == project.Id && t.Status == "Doing")
                .ToListAsync();

            var doneTasks = await _context.Tasks
                .Include(t => t.Users)
                .Where(t => t.ProjectId == project.Id && t.Status == "Done")
                .ToListAsync();

            var viewModel = new TaskViewModel
            {
                Project = project,
                DoingTasks = doingTasks,
                DoneTasks = doneTasks
            };

            return View(viewModel);
        }
        //public async Task<IActionResult> TaskView(Guid id)
        //{
        //    var project = await _context.Projects
        //        .Include(p => p.Tasks)
        //        .ThenInclude(t => t.UserTasks)
        //        .ThenInclude(ut => ut.User)
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (project == null)
        //    {
        //        return NotFound();
        //    }

        //    var doingTasks = project.Tasks
        //        .Where(t => t.Status == "Doing")
        //        .ToList();

        //    var doneTasks = project.Tasks
        //        .Where(t => t.Status == "Done")
        //        .ToList();

        //    var viewModel = new TaskViewModel
        //    {
        //        Project = project,
        //        DoingTasks = doingTasks,
        //        DoneTasks = doneTasks
        //    };

        //    return View(viewModel);
        //}


        public async Task<IActionResult> AddTask(Guid projectId, string status)
        {
            var users = await _context.Users.ToListAsync();
            var taskViewModel = new TaskViewModel
            {
                Task = new Models.Task { ProjectId = projectId, Status = status },
                Users = users
            };
            return View(taskViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTask()
        {
            // Lấy dữ liệu từ request
            var formValues = Request.Form;

            // In ra các giá trị để kiểm tra
            foreach (var key in formValues.Keys)
            {
                Console.WriteLine($"{key}: {formValues[key]}");
            }

            // Tạo một đối tượng Task từ form dữ liệu
            var task = new Models.Task
            {
                ProjectId = Guid.Parse(formValues["Task.ProjectId"]),
                Status = formValues["Task.Status"],
                Title = formValues["Task.Title"],
                Content = formValues["Task.Content"],
                DueDate = DateTime.Parse(formValues["Task.DueDate"])
            };

            // Xử lý SelectedUsers
            var selectedUsers = formValues["SelectedUsers"].ToArray();

            // Kiểm tra dữ liệu
            Console.WriteLine($"Task.ProjectId: {task.ProjectId}");
            Console.WriteLine($"Task.Status: {task.Status}");
            Console.WriteLine($"Task.Title: {task.Title}");
            Console.WriteLine($"Task.Content: {task.Content}");
            Console.WriteLine($"Task.DueDate: {task.DueDate}");
            Console.WriteLine($"SelectedUsers: {string.Join(", ", selectedUsers)}");

            // Kiểm tra nếu có bất kỳ lỗi nào
            if (string.IsNullOrEmpty(task.Title) || task.DueDate == DateTime.MinValue)
            {
                // Trả về lỗi hoặc làm gì đó khi thiếu dữ liệu
                return View(); // Hoặc RedirectToAction với thông báo lỗi
            }

            // Thực hiện lưu task vào cơ sở dữ liệu
            task.Id = Guid.NewGuid();
            task.UpdateAt = DateTime.Now;

            // Xử lý selected users
            task.Users = new List<User>();
            foreach (var userId in selectedUsers)
            {
                var user = await _context.Users.FindAsync(Guid.Parse(userId));
                if (user != null)
                {
                    task.Users.Add(user);
                }
            }

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("TaskView", new { id = task.ProjectId });
        }





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddTask(TaskViewModel viewModel)
        //{
        //    // Log các giá trị từ form
        //    var formValues = Request.Form;
        //    foreach (var key in formValues.Keys)
        //    {
        //        Console.WriteLine($"{key}: {formValues[key]}");
        //    }
        //    // Log dữ liệu nhận được từ form
        //    Console.WriteLine($"ProjectId: {viewModel.Task.ProjectId}");
        //    Console.WriteLine($"Status: {viewModel.Task.Status}");
        //    Console.WriteLine($"Title: {viewModel.Task.Title}");
        //    Console.WriteLine($"Content: {viewModel.Task.Content}");
        //    Console.WriteLine($"DueDate: {viewModel.Task.DueDate}");
        //    Console.WriteLine($"SelectedUsers: {string.Join(", ", viewModel.SelectedUsers)}");

        //    // Kiểm tra ModelState
        //    if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState.Values.SelectMany(v => v.Errors);
        //        foreach (var error in errors)
        //        {
        //            Console.WriteLine($"Error: {error.ErrorMessage}");
        //        }

        //        viewModel.Users = await _context.Users.ToListAsync();
        //        return View(viewModel);
        //    }

        //    // Lưu task vào cơ sở dữ liệu
        //    var task = viewModel.Task;
        //    task.Id = Guid.NewGuid();
        //    task.UpdateAt = DateTime.Now;

        //    task.Users = new List<User>();
        //    if (viewModel.SelectedUsers != null)
        //    {
        //        foreach (var userId in viewModel.SelectedUsers)
        //        {
        //            var user = await _context.Users.FindAsync(userId);
        //            if (user != null)
        //            {
        //                task.Users.Add(user);
        //            }
        //        }
        //    }

        //    _context.Tasks.Add(task);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("TaskView", new { id = task.ProjectId });
        //}






        public IActionResult Index()
        {
            return View();
        }
    }
}
