using BTL_WNC.Models;
using BTL_WNC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BTL_WNC.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectDbContext _dbContext;

        public UserController(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Kiểm tra quyền admin của người dùng
        private bool IsAdmin()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            return user != null && user.RoleId == "role1";
        }

        public IActionResult Profile(Guid id)
        {
            // Include related tasks for the user
            var user = _dbContext.Users
                .Include(u => u.Tasks) // Load related tasks
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // Action to list all users
        public async Task<IActionResult> AllUser()
        {
            var users = await _dbContext.Users.ToListAsync();

            // Create ViewModel to pass to the view
            var viewModel = new TaskViewModel
            {
                Users = users
            };
            ViewBag.IsAdmin = IsAdmin();
            return View(viewModel);
        }

        public async Task<IActionResult> EditProfile(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(Guid id, string name, string email, string phoneNumber, string password)
        {
            // Manual validation checks
            if (string.IsNullOrEmpty(name))
            {
                // Add a model error
                ModelState.AddModelError("Name", "Name is required.");
            }
            if (string.IsNullOrEmpty(email) || !new EmailAddressAttribute().IsValid(email))
            {
                ModelState.AddModelError("Email", "A valid email is required.");
            }
            if (string.IsNullOrEmpty(phoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Phone number is required.");
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }

            // Check if the manual validation passed
            if (ModelState.IsValid)
            {
                // Fetch and update the user
                var user = await _dbContext.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Name = name;
                user.Email = email;
                user.PhoneNumber = phoneNumber;
                user.Password = password;

                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Profile", new { id = user.Id });
            }

            // If validation failed, return the view with errors
            return View(new User { Id = id, Name = name, Email = email, PhoneNumber = phoneNumber, Password = password });
        }

        //edit profile
        //[HttpPost]
        //public IActionResult EditProfile(User updatedUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _dbContext.Users.Find(updatedUser.Id);
        //        if (user != null)
        //        {
        //            user.Name = updatedUser.Name;
        //            user.Email = updatedUser.Email;
        //            user.PhoneNumber = updatedUser.PhoneNumber;
        //            _dbContext.SaveChanges();

        //            // Retrieve the updated user with tasks
        //            user = _dbContext.Users
        //                .Include(u => u.Tasks)
        //                .FirstOrDefault(u => u.Id == updatedUser.Id);

        //            return View("Profile", user);
        //        }
        //        return NotFound(); // Handle case where user is not found
        //    }
        //    return View("Profile", updatedUser);
        //}


        //Xóa
        [HttpPost]
        public IActionResult DeleteUser(Guid userId)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            var user = _dbContext.Users.Find(userId);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("AllUser"); // Adjust this to your actual user list action
        }

    }
}
