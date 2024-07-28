using Microsoft.AspNetCore.Mvc;
using BTL_WNC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BTL_WNC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectDbContext _context;

        public ProjectController(ProjectDbContext context)
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

        // GET: Projects
        public async Task<IActionResult> ProjectView()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Access");
            }

            var projects = from p in _context.Projects
            select p;

            projects = projects.OrderByDescending(p => p.startTime);
            ViewBag.UserName = HttpContext.Session.GetString("NameLogined"); // Assuming the user is authenticated
            ViewBag.UserId = HttpContext.Session.GetString("UserId");
            ViewBag.IsAdmin = IsAdmin();
            return View(await projects.ToListAsync());
        }


        // GET: Projects/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var project = await _context.Projects
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(project);
        //}

        // GET: Project/Create
        public IActionResult AddProject()
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProject([Bind("Name,Decription")] Project project)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid();
                project.startTime = DateTime.Now; // Gán thời gian hiện tại cho StartTime
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ProjectView));
            }
            return View(project);
        }

        public IActionResult EditProject(Guid id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return PartialView("EditProject", project); 
        }

        // POST: Project/EditProject
        [HttpPost]
        public IActionResult EditProject(Project model)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                var project = _context.Projects.Find(model.Id);
                if (project == null)
                {
                    return NotFound();
                }

                project.Name = model.Name;
                project.Decription = model.Decription;

                _context.Update(project);
                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
        }

        // API: Projects/Delete
        [HttpPost]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }


        private bool ProjectExists(Guid id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
