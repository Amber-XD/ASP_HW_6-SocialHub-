using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SocialHub.Context;
using SocialHub.Models;
using Microsoft.AspNetCore.Authorization;

namespace SocialHub.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create(int postId)
        {
            ViewBag.PostId = postId;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Comment comment)
        {
            comment.Author = User.Identity!.Name!;
            comment.CreatedAt = DateTime.Now;

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", "Post", new { id = comment.PostId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id, int postId)
        {
            var comment = _context.Comments.Find(id);

            if (comment == null)
            {
                return NotFound();
            }

            if (comment.Author != User.Identity!.Name && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", "Post", new { id = postId });
        }
    }
}
