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
    public class TaskController : Controller
    {
        private readonly ITaskRepository TaskRepository;
        private readonly IUserRepository userRepository;

        public TaskController(
            ITaskRepository _Task
            , IUserRepository _user
            )
        {
            TaskRepository = _Task;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index()
        {
            var start_date = Request["startDate"];
            var end_date = Request["endDate"];
            IQueryable<TaskViewModel> q = TaskRepository.GetAllvwTaskFull()
                .Where(x => x.Type == "task")
                .Select(item => new TaskViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    //CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    //ModifiedUserName = item.ModifiedUserName,
                    ModifiedDate = item.ModifiedDate,
                    Subject = item.Subject,
                    Status = item.Status,
                    ParentType = item.ParentType,
                    ParentId = item.ParentId,
                    AssignedUserId = item.AssignedUserId,
                    ProfileImage = item.ProfileImage,
                    FullName = item.FullName,
                    Type = item.Type,
                    Note = item.Note
                }).OrderByDescending(m => m.ModifiedDate);
            if (!string.IsNullOrEmpty(start_date) && !string.IsNullOrEmpty(end_date))
            {
                DateTime start_d;
                if (DateTime.TryParseExact(start_date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out start_d))
                {
                    DateTime end_d;
                    if (DateTime.TryParseExact(end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out end_d))
                    {
                        end_d = end_d.AddHours(23);
                        q = q.Where(x => start_d <= x.CreatedDate && x.CreatedDate <= end_d);
                    }
                }
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }

        public ViewResult MyTasks()
        {
            IQueryable<TaskViewModel> q = TaskRepository.GetAllTask()
                .Where(item => item.AssignedUserId == WebSecurity.CurrentUserId
                && item.Status != "Deferred"
                && item.Status != "Completed")
                .Select(item => new TaskViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    //CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    //ModifiedUserName = item.ModifiedUserName,
                    ModifiedDate = item.ModifiedDate,
                    Subject = item.Subject,
                    Status = item.Status
                }).OrderByDescending(m => m.ModifiedDate);
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create()
        {
            var model = new TaskViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Task = new Task();
                AutoMapper.Mapper.Map(model, Task);
                Task.IsDeleted = false;
                Task.CreatedUserId = WebSecurity.CurrentUserId;
                Task.ModifiedUserId = WebSecurity.CurrentUserId;
                Task.AssignedUserId = WebSecurity.CurrentUserId;
                Task.CreatedDate = DateTime.Now;
                Task.ModifiedDate = DateTime.Now;
                TaskRepository.InsertTask(Task);

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Task = TaskRepository.GetTaskById(Id.Value);
            if (Task != null && Task.IsDeleted != true)
            {
                var model = new TaskViewModel();
                AutoMapper.Mapper.Map(Task, model);

                if (model.AssignedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
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
        public ActionResult Edit(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Task = TaskRepository.GetTaskById(model.Id);
                    AutoMapper.Mapper.Map(model, Task);
                    Task.ModifiedUserId = WebSecurity.CurrentUserId;
                    Task.ModifiedDate = DateTime.Now;
                    TaskRepository.UpdateTask(Task);

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
            var Task = TaskRepository.GetTaskById(Id.Value);
            if (Task != null && Task.IsDeleted != true)
            {
                var model = new TaskViewModel();
                AutoMapper.Mapper.Map(Task, model);

                if (model.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
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
                    var item = TaskRepository.GetTaskById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        if (item.CreatedUserId != Erp.BackOffice.Helpers.Common.CurrentUser.Id && Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }

                        item.IsDeleted = true;
                        TaskRepository.UpdateTask(item);
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

        #region CheckSeen
        //[HttpPost]
        public ActionResult CheckSeen(int? Id)
        {
            var notifications = TaskRepository.GetTaskById(Id.Value);
            if (notifications != null)
            {
                if (notifications.AssignedUserId == notifications.ModifiedUserId)
                {
                    notifications.AssignedUserId = 0;
                    TaskRepository.UpdateTask(notifications);
                    return Content("notseen");
                }
                else
                {
                    notifications.AssignedUserId = WebSecurity.CurrentUserId;
                    TaskRepository.UpdateTask(notifications);
                    return Content("success");
                }
            }
            else
            {
                return Content("error");
            }
        }
        #endregion

        #region CheckAllSeen
        //[HttpPost]
        public ActionResult CheckAllSeen()
        {
            var notifications = TaskRepository.GetAllTask().Where(x => x.ModifiedUserId == WebSecurity.CurrentUserId).ToList();
            for (int i = 0; i < notifications.Count(); i++)
            {
                notifications[i].AssignedUserId = WebSecurity.CurrentUserId;
                TaskRepository.UpdateTask(notifications[i]);
            }
            return Content("success");
        }
        #endregion

        #region CheckDisable
        //[HttpPost]
        public ActionResult CheckDisable(int? Id)
        {
            var notifications = TaskRepository.GetTaskFullById(Id.Value);
            //chia làm 2 trường hợp. 1 TH chưa xem thì chuyển thành đã xem và ẩn thông báo đi
            //TH2 đã xem rồi thì chỉ ẩn thông báo thôi.
            if (notifications != null)
            {
                if (notifications.AssignedUserId.Value <= 0)
                {
                    notifications.IsDeleted = true;
                    notifications.AssignedUserId = WebSecurity.CurrentUserId;
                    TaskRepository.UpdateTask(notifications);
                    return Content("notseen");
                }
                else
                {
                    notifications.IsDeleted = true;
                    TaskRepository.UpdateTask(notifications);
                    return Content("seen");
                }
            }
            else
            {
                return Content("error");
            }
        }
        #endregion

        #region UpdateTask

        public ActionResult UpdateTask(int? Id)
        {
            var task = TaskRepository.GetvwTaskById(Id.Value);
            if (task != null && task.IsDeleted != true)
            {
                var model = new TaskViewModel();
                AutoMapper.Mapper.Map(task, model);
                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult UpdateTask(TaskViewModel model)
        {
            var urlRefer = Request["UrlReferrer"];
            var Task = TaskRepository.GetTaskById(model.Id);
            //   AutoMapper.Mapper.Map(model, Task);
            Task.Note = model.Note;
            Task.DueDate = model.DueDate;
            Task.Status = model.Status;
            Task.ModifiedUserId = WebSecurity.CurrentUserId;
            Task.ModifiedDate = DateTime.Now;
            TaskRepository.UpdateTask(Task);

            TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
            if (Request["IsPopup"] == "true" || Request["IsPopup"] == "True")
            {
                ViewBag.closePopup = "true";
                model.Id = Task.Id;
                ViewBag.urlRefer = urlRefer;
                return View(model);
            }
            return Redirect(urlRefer);
        }
        #endregion

        public ActionResult LogNotifications()
        {
            var user = userRepository.GetUserById(WebSecurity.CurrentUserId);
            //lấy danh sách thông báo của user hiện lên.
            List<TaskViewModel> q = TaskRepository.GetAllvwTask().Where(x => x.ModifiedUserId == user.Id && x.Type == "notifications")
                .Select(x => new TaskViewModel
                {
                    AssignedUserId = x.AssignedUserId,
                    CreatedDate = x.CreatedDate,
                    CreatedUserId = x.CreatedUserId,
                    FullName = x.FullName,
                    ProfileImage = x.ProfileImage,
                    Id = x.Id,
                    IsDeleted = x.IsDeleted,
                    ParentId = x.ParentId,
                    ParentType = x.ParentType,
                    Subject = x.Subject,
                    UserName = x.UserName,
                    ModifiedUserId = x.ModifiedUserId
                }).OrderByDescending(x => x.CreatedDate).ToList();
            return View(q);
        }
    }
}