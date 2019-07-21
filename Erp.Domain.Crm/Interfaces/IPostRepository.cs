using Erp.Domain.Crm.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Crm.Interfaces
{
    public interface IPostRepository
    {
        /// <summary>
        /// Get all Post
        /// </summary>
        /// <returns>Post list</returns>
        IQueryable<Post> GetAllPost();
        IQueryable<vwPost> GetAllvwPost();

        /// <summary>
        /// Get Post information by specific id
        /// </summary>
        /// <param name="Id">Id of Post</param>
        /// <returns></returns>
        Post GetPostById(int Id);

        /// <summary>
        /// Insert Post into database
        /// </summary>
        /// <param name="Post">Object infomation</param>
        void InsertPost(Post Post);

        /// <summary>
        /// Delete Post with specific id
        /// </summary>
        /// <param name="Id">Post Id</param>
        void DeletePost(int Id);

        /// <summary>
        /// Delete a Post with its Id and Update IsDeleted IF that Post has relationship with others
        /// </summary>
        /// <param name="Id">Id of Post</param>
        void DeletePostRs(int Id);

        /// <summary>
        /// Update Post into database
        /// </summary>
        /// <param name="Post">Post object</param>
        void UpdatePost(Post Post);
    }
}
