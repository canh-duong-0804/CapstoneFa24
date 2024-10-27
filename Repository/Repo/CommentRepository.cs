﻿using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class CommentRepository : ICommentRepository
    {
        public Task<Comment> CreateComment(Comment comment) => CommentDAO.Instance.CreateComment(comment);

        public Task<bool> DeleteComment(int commentId) => CommentDAO.Instance.DeleteComment(commentId);

        public Task<IEnumerable<Comment>> GetCommentsByPostId(int postId) => CommentDAO.Instance.GetCommentsByPostId(postId);

        public Task<Comment?> UpdateComment(Comment updatedComment) => CommentDAO.Instance.UpdateComment(updatedComment);

        public Task<Comment?> GetCommentById(int commentId) => CommentDAO.Instance.GetCommentById(commentId);

    }
}