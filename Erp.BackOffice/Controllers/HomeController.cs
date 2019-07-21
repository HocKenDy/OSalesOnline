using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Erp.Domain.Entities;
using System.Globalization;
using Erp.BackOffice.Helpers;
using System;
using Erp.BackOffice.Models;
using Erp.Domain.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Web.Security;
using WebMatrix.WebData;
using Erp.Utilities;
using System.IO;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Helper;
using Erp.Domain.Sale.Interfaces;
using Erp.Domain.Crm.Interfaces;
using Erp.BackOffice.Crm.Models;

namespace Erp.BackOffice.Controllers
{
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class HomeController : Controller
    {
        private readonly IPageMenuRepository _pageMenuRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoginLogRepository _loginlogRepository;
        private readonly ITaskRepository taskRepository;

        public HomeController(
            IUserRepository userRepo
            , IPageMenuRepository pageMenuRepository
            , ILoginLogRepository loginlog
            , ITaskRepository task
            )
        {
            _pageMenuRepository = pageMenuRepository;
            _loginlogRepository = loginlog;
            _userRepository = userRepo;
            taskRepository = task;
        }

        [Authorize]
        public ActionResult Index()
        {
            string home_page = Helpers.Common.GetSetting("home_page");
            return Redirect(home_page);
        }

        [Authorize]
        public ActionResult Dashboard(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [Authorize]
        public ActionResult DashboardInventory()
        {
            return View();
        }
        [Authorize]
        public ActionResult DashboardInventoryCard()
        {
            return View();
        }

        [Authorize]
        public ActionResult DashboardSale()
        {
            return View();
        }

        [Authorize]
        public ActionResult DashboardTransaction()
        {
            return View();
        }
        [Authorize]
        public ActionResult DashboardTransactionCustomer()
        {
            return View();
        }
        [Authorize]
        public ActionResult DashboardTransactionSupplier()
        {
            return View();
        }
        [Authorize]
        public ActionResult TrackRequest()
        {
            var model = ControllerContext.HttpContext.Application["ListRequest"] as List<RequestInfo>;

            return View(model.OrderByDescending(item=>item.FirstDate).ToList());
        }
        [Authorize]
        public ActionResult Breadcrumb(List<Erp.BackOffice.Areas.Administration.Models.PageMenuViewModel> DataMenuItem, string controllerName, string actionName, string areaName)
        {
            var model = new List<BreadcrumbViewModel>();

            var currentUrl = ("/" + controllerName + "/" + actionName).ToLower();
            var pageMenuItem = DataMenuItem.Where(x => (x.Url != null && x.Url.ToLower() == currentUrl) || (x.PageUrl != null && x.PageUrl.ToLower() == currentUrl)).FirstOrDefault();

            if (pageMenuItem == null)
            {
                currentUrl = ("/" + controllerName + "/index").ToLower();
                pageMenuItem = DataMenuItem.Where(x => x.PageUrl != null && x.PageUrl.ToLower() == currentUrl).FirstOrDefault();
            }

            while (pageMenuItem != null)
            {
                var breadCrumb = new BreadcrumbViewModel
                {
                    Area = pageMenuItem.AreaName,
                    ParrentId = pageMenuItem.ParentId,
                    Name = pageMenuItem.Name,
                    Url = pageMenuItem.PageUrl
                };

                model.Insert(0, breadCrumb);

                if (pageMenuItem.ParentId != null)
                {
                    pageMenuItem = DataMenuItem.Where(x => x.Id == pageMenuItem.ParentId.Value).FirstOrDefault();
                }
                else
                {
                    pageMenuItem = null;
                }
            }
            

            return PartialView("_BreadcrumbPartial", model);

            //var page = _pageMenuRepository.GetPageByAction(areaName, controllerName, actionName);

            //if (page != null)
            //{
            //    var model = new BreadcrumbViewModel
            //    {
            //        Area = page.AreaName,
            //        Controller = page.ControllerName,
            //        Action = page.ActionName,
            //        ParrentId = page.ParentId,
            //        Name = _pageMenuRepository.GetPageName(page.Id, Session["CurrentLanguage"].ToString()),
            //        Url = page.Url
            //    };
            //    return PartialView("_BreadcrumbPartial", model);
            //}

            //return PartialView("_BreadcrumbPartial", null);
        }

        public ActionResult BreadcrumbParentPage(int parrentId)
        {
            var page = _pageMenuRepository.GetPageById(parrentId);

            if (page != null)
            {
                var model = new BreadcrumbViewModel
                {
                    Area = page.AreaName,
                    Controller = page.ControllerName,
                    Action = page.ActionName,
                    ParrentId = page.ParentId,
                    Name = _pageMenuRepository.GetPageName(page.Id, Session["CurrentLanguage"].ToString())
                };
                return PartialView("_BreadcrumbParrentPagePartial", model);
            }

            return PartialView("_BreadcrumbParrentPagePartial", null);
        }

        [AllowAnonymous]
        public ActionResult _ClosePopup()
        {            
            return View();
        }

        [Authorize]
        public ActionResult Notifications()
        {

            var user = _userRepository.GetUserById(WebSecurity.CurrentUserId);
            //xóa thông báo quá hạn.
            DeleteNotifications(user.Id);
            //lấy danh sách thông báo của user hiện lên.
            var q = taskRepository.GetAllvwTask().Where(x => x.ModifiedUserId == user.Id && x.Type == "notifications").Take(50)
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
                }).ToList();
            q = q.OrderByDescending(x => x.CreatedDate).ToList();
            return View(q);
        }

        #region xóa thông báo quá hạn
        public static void DeleteNotifications(int? userID)
        {
            //chỉ xóa những tin nhắn quá hạn mà đã xem.
            Erp.Domain.Crm.Repositories.TaskRepository taskRepository = new Erp.Domain.Crm.Repositories.TaskRepository(new Domain.Crm.ErpCrmDbContext());
            var quantityDate = Helpers.Common.GetSetting("quantity_notifications_date");
            var date = DateTime.Now.AddDays(Convert.ToInt32(quantityDate));
            var notifications = taskRepository.GetAllTaskFull().Where(x => x.AssignedUserId == userID && x.Type == "notifications").ToList();
            notifications = notifications.Where(x => x.CreatedDate <= date).ToList();
            for (int i = 0; i < notifications.Count(); i++)
            {
                taskRepository.DeleteTask(notifications[i].Id);
            }

        }
        #endregion
    }
}
