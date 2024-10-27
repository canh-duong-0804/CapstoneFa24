using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface ICommentRepository 
    {
        Task<Comment> CreateComment(Comment comment);
        Task<Comment?> UpdateComment(Comment updatedComment);
        Task<bool> DeleteComment(int commentId);
        Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);

        Task<Comment?> GetCommentById(int commentId);



    }
}
