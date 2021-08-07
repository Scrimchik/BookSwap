﻿using BookSwap.Models;
using BookSwap.Models.Abstraction;
using BookSwap.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private UserManager<User> userManager;
        private ApplicationContext db;
        private ICommentRepository commentRepository;

        public CommentController(UserManager<User> userManager, ApplicationContext db, ICommentRepository commentRepository)
        {
            this.userManager = userManager;
            this.db = db;
            this.commentRepository = commentRepository;
        }

        public async Task<IActionResult> AddComment(string commentText, int bookId, string returnUrl)
        {
            Comment comment = new Comment
            {
                BookId = bookId,
                Text = commentText,
                UserId = userManager.GetUserId(User),
                DateAdded = DateTime.Now.ToString("M") + " в " + DateTime.Now.ToString("t")
            };
            await db.Comments.AddAsync(comment);
            await db.SaveChangesAsync();
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> DeleteComment(int commentId, string returnUrl)
        {
            string commentUserId = await commentRepository.Comments.Where(t => t.Id == commentId).Select(t => t.UserId).FirstOrDefaultAsync();
            if(commentUserId == userManager.GetUserId(User))
            {
                db.Comments.Remove(new Comment { Id = commentId });
                await db.SaveChangesAsync();
            }
            return Redirect(returnUrl);
        }
    }
}
