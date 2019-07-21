using System.Globalization;
using Erp.BackOffice.Crm.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Crm.Entities;
using Erp.Domain.Crm.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;

namespace Erp.BackOffice.Crm.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class PostController : Controller
    {
        private readonly IPostRepository PostRepository;
        private readonly IUserRepository userRepository;

        public PostController(
            IPostRepository _Post
            , IUserRepository _user
            )
        {
            PostRepository = _Post;
            userRepository = _user;
        }

        #region Index
        public ViewResult Index(string txtSearch)
        {

            IQueryable<PostViewModel> q = PostRepository.GetAllPost()
                .Select(item => new PostViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    //CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    //ModifiedUserName = item.ModifiedUserName,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
                }).OrderByDescending(m => m.ModifiedDate);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }

        public ViewResult ListByTarget(string TargetModule, int TargetId)
        {
            var model = PostRepository.GetAllvwPost()
                .Where(item => item.TargetModule == TargetModule && item.TargetId == TargetId)
                .Select(item => new PostViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Content = item.Content,
                    ProfileImage = item.ProfileImage
                }).OrderByDescending(m => m.CreatedDate).ToList();

            ViewBag.TargetModule = TargetModule;
            ViewBag.TargetId = TargetId;

            return View(model);
        }
        #endregion

        #region Create
        public ViewResult Create()
        {
            var model = new PostViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Post = new Post();
                AutoMapper.Mapper.Map(model, Post);
                Post.IsDeleted = false;
                Post.CreatedUserId = WebSecurity.CurrentUserId;
                Post.ModifiedUserId = WebSecurity.CurrentUserId;
                Post.AssignedUserId = WebSecurity.CurrentUserId;
                Post.CreatedDate = DateTime.Now;
                Post.ModifiedDate = DateTime.Now;
                PostRepository.InsertPost(Post);

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Comment
        [HttpPost]
        public ContentResult Comment(string TargetModule, int TargetId, string CommentContent)
        {
            if (ModelState.IsValid)
            {
                var post = new Post();
                post.IsDeleted = false;
                post.CreatedUserId = WebSecurity.CurrentUserId;
                post.ModifiedUserId = WebSecurity.CurrentUserId;
                post.AssignedUserId = WebSecurity.CurrentUserId;
                post.CreatedDate = DateTime.Now;
                post.ModifiedDate = DateTime.Now;
                post.TargetModule = TargetModule;
                post.TargetId = TargetId;
                post.Content = CommentContent;

                PostRepository.InsertPost(post);

                return Content("success");
            }

            return Content("fail");
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Post = PostRepository.GetPostById(Id.Value);
            if (Post != null && Post.IsDeleted != true)
            {
                var model = new PostViewModel();
                AutoMapper.Mapper.Map(Post, model);
                
                if (model.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                {
                    TempData["FailedMessage"] = "NotOwner";
                    return RedirectToAction("Index");
                }                

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Post = PostRepository.GetPostById(model.Id);
                    AutoMapper.Mapper.Map(model, Post);
                    Post.ModifiedUserId = WebSecurity.CurrentUserId;
                    Post.ModifiedDate = DateTime.Now;
                    PostRepository.UpdatePost(Post);

                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    return RedirectToAction("Index");
                }

                return View(model);
            }

            return View(model);

            //if (Request.UrlReferrer != null)
            //    return Redirect(Request.UrlReferrer.AbsoluteUri);
            //return RedirectToAction("Index");
        }

        #endregion

        #region Detail
        public ActionResult Detail(int? Id)
        {
            var Post = PostRepository.GetPostById(Id.Value);
            if (Post != null && Post.IsDeleted != true)
            {
                var model = new PostViewModel();
                AutoMapper.Mapper.Map(Post, model);
                
                if (model.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                {
                    TempData["FailedMessage"] = "NotOwner";
                    return RedirectToAction("Index");
                }                

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                string idDeleteAll = Request["DeleteId-checkbox"];
                string[] arrDeleteId = idDeleteAll.Split(',');
                for (int i = 0; i < arrDeleteId.Count(); i++)
                {
                    var item = PostRepository.GetPostById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }

                        item.IsDeleted = true;
                        PostRepository.UpdatePost(item);
                    }
                }
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return RedirectToAction("Index");
            }
        }
        #endregion

        public static void SavePost(int TargetID, string TargetModule, string CommentContent)
        {
            Erp.Domain.Crm.Repositories.PostRepository postRepository = new Erp.Domain.Crm.Repositories.PostRepository(new Domain.Crm.ErpCrmDbContext());
            var post = new Domain.Crm.Entities.Post();
            post.IsDeleted = false;
            post.CreatedUserId = WebSecurity.CurrentUserId;
            post.ModifiedUserId = WebSecurity.CurrentUserId;
            post.AssignedUserId = WebSecurity.CurrentUserId;
            post.CreatedDate = DateTime.Now;
            post.ModifiedDate = DateTime.Now;
            post.TargetModule = TargetModule;
            post.TargetId = TargetID;
            post.Content = CommentContent;
            postRepository.InsertPost(post);
        }
    }
}
