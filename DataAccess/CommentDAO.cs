using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CommentDAO
    {
        private static CommentDAO instance = null;
        private static readonly object instanceLock = new object();

        public static CommentDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CommentDAO();
                    }
                    return instance;
                }
            }
        }

        // Create a new comment
        public async Task<Comment> CreateComment(Comment comment)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    comment.CreateDate = DateTime.Now;
                    comment.Status = true; // Active by default
                    await context.Comments.AddAsync(comment);
                    await context.SaveChangesAsync();
                    return comment;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /* // Get comments by post ID
         public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
         {
             using (var context = new HealthTrackingDBContext())
             {
                 return await context.Comments
                     .Where(c => c.PostId == postId && c.Status == true) // Active comments only
                     .OrderByDescending(c => c.CreateDate)
                     .ToListAsync();
             }
         }*/

        // Update a comment
        public async Task<Comment?> UpdateComment(Comment updatedComment)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var comment = await context.Comments.FindAsync(updatedComment.CommentId);
                    if (comment == null || comment.Status == false) return null;

                    // Update comment properties
                    comment.Content = updatedComment.Content;
                    comment.ChangeBy = updatedComment.ChangeBy;
                    comment.ChangeDate = DateTime.Now;

                    await context.SaveChangesAsync();
                    return comment;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Get a comment by ID
        public async Task<Comment?> GetCommentById(int commentId)
        {
            using (var context = new HealthTrackingDBContext())
            {
                return await context.Comments.FindAsync(commentId);
            }
        }


        // Soft delete a comment
        public async Task<bool> DeleteComment(int commentId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var comment = await context.Comments.FindAsync(commentId);
                    if (comment == null || comment.Status == false) return false;

                    comment.Status = false; // Soft delete
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Get all active comments for a specific post ID
        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.Comments
                        .Include(comment => comment.CreateByNavigation)
                        .Where(comment => comment.PostId == postId && comment.Status == true) // Active comments only
                        .OrderByDescending(comment => comment.CreateDate)
                        .ToListAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
