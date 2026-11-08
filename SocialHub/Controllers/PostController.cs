using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SocialHub.Context;
using SocialHub.Models;
using Microsoft.AspNetCore.Authorization;

namespace SocialHub.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? author)
        {
            var posts = _context.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(author))
            {
                posts = posts.Where(p => p.Author.Contains(author));
            }

            return View(posts.OrderByDescending(p => p.CreatedAt).ToList());
        }

        public IActionResult Details(int id)
        {
            var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Post post)
        {
            post.Author = User.Identity!.Name!;
            post.CreatedAt = DateTime.Now;

            _context.Posts.Add(post);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = _context.Posts.Find(id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.Author != User.Identity!.Name && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.Author != User.Identity!.Name && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(post);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(Post post)
        {
            var existingPost = _context.Posts.Find(post.Id);

            if (existingPost == null)
            {
                return NotFound();
            }

            if (existingPost.Author != User.Identity!.Name && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            existingPost.Author = post.Author;
            existingPost.Title = post.Title;
            existingPost.Text = post.Text;

            _context.SaveChanges();

            return RedirectToAction("Details", new { id = post.Id });
        }
    }
}
