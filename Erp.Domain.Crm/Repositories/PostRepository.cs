using Erp.Domain.Crm.Entities;
using Erp.Domain.Crm.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Crm.Repositories
{
    public class PostRepository : GenericRepository<ErpCrmDbContext, Post>, IPostRepository
    {
        public PostRepository(ErpCrmDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all Post
        /// </summary>
        /// <returns>Post list</returns>
        public IQueryable<Post> GetAllPost()
        {
            return Context.Post.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwPost> GetAllvwPost()
        {
            return Context.vwPost.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get Post information by specific id
        /// </summary>
        /// <param name="PostId">Id of Post</param>
        /// <returns></returns>
        public Post GetPostById(int Id)
        {
            return Context.Post.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert Post into database
        /// </summary>
        /// <param name="Post">Object infomation</param>
        public void InsertPost(Post Post)
        {
            Context.Post.Add(Post);
            Context.Entry(Post).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete Post with specific id
        /// </summary>
        /// <param name="Id">Post Id</param>
        public void DeletePost(int Id)
        {
            Post deletedPost = GetPostById(Id);
            Context.Post.Remove(deletedPost);
            Context.Entry(deletedPost).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a Post with its Id and Update IsDeleted IF that Post has relationship with others
        /// </summary>
        /// <param name="PostId">Id of Post</param>
        public void DeletePostRs(int Id)
        {
            Post deletePostRs = GetPostById(Id);
            deletePostRs.IsDeleted = true;
            UpdatePost(deletePostRs);
        }

        /// <summary>
        /// Update Post into database
        /// </summary>
        /// <param name="Post">Post object</param>
        public void UpdatePost(Post Post)
        {
            Context.Entry(Post).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
