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

        // GET: Projects
        public async Task<IActionResult> ProjectView(string searchString)
        {
            var projects = from p in _context.Projects
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString));
            }

            projects = projects.OrderByDescending(p => p.startTime);

            return View(await projects.ToListAsync());

            //var projects = await _context.Projects.OrderByDescending(p => p.startTime).ToListAsync();
            //return View(projects);
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
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProject([Bind("Name,Decription")] Project project)
        {
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

        // GET: Projects/Edit/5
        public async Task<IActionResult> EditProject(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View("EditProject", project); // Sử dụng cùng một view cho cả thêm mới và chỉnh sửa
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Decription,startTime")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProjectView)); // Chuyển hướng về trang ProjectView
            }
            return View("EditProject", project);
        }



        // API: Projects/Delete
        [HttpPost]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
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
