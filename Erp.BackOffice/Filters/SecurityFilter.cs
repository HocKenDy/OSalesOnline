using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erp.Domain;
using Erp.Domain.Entities;
using Erp.BackOffice.Filters;
using Erp.Domain.Interfaces;
using Erp.BackOffice.Administration.Models;
using Erp.BackOffice.Areas.Administration.Models;
using Erp.Domain.Repositories;
using WebMatrix.WebData;
using System.Web.Routing;
using Erp.BackOffice.Helpers;

namespace Erp.BackOffice.Filters
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                Erp.BackOffice.Helpers.Common.TrackRequest();
            }

            //base.OnAuthorization(filterContext); //returns to login url

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization)
            {
                return;
            }

            base.OnActionExecuting(filterContext);

            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("ErpDbContext", "System_User", "Id", "UserName", autoCreateTables: true);
            }

            // initialize navigation  by user
            string sControlerName = filterContext.RouteData.Values["Controller"] != null ? filterContext.RouteData.Values["Controller"].ToString().ToLower() : "";
            string sActionName = filterContext.RouteData.Values["Action"] != null ? filterContext.RouteData.Values["Action"].ToString().ToLower() : "";
            string sAreaName = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString().ToLower() : "";

            //Ngoài những cái login/logoff ... Thì mới init menu
            if (sActionName != "login".ToLower() && sActionName != "logoff" && sActionName != "forgetpassword")
            {
                //Coi thử đăng nhập chưa
                if (WebSecurity.IsAuthenticated)
                {
                    if (Helpers.Common.CurrentUser == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "User" }, { "action", "logoff" } });
                    }

                    //Kiểm tra truy cập
                    if (AccessRight(sActionName, sControlerName, sAreaName))
                    {
                        if (!filterContext.IsChildAction && !filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            Dictionary<string, object> userLoggedInfo = new Dictionary<string, object> { { "FullName", Helpers.Common.CurrentUser.FullName }, { "UserName", Helpers.Common.CurrentUser.UserName }, { "UserTypeName", Helpers.Common.CurrentUser.UserTypeName } };
                            filterContext.Controller.ViewData["UserLogged"] = userLoggedInfo;

                            //Kiểm tra truy cập ok thì init menu
                            initMenu(filterContext, sControlerName, sActionName);
                        }
                        else //nếu request là ajax thì chuyền viewdata page menu rỗng
                        {
                            filterContext.Controller.ViewData["LayoutMenu"] = new List<PageMenuViewModel>();
                        }

                    }
                    else
                    {
                        if (filterContext.IsChildAction || filterContext.HttpContext.Request.IsAjaxRequest())
                            filterContext.Result = new ContentResult { Content = "" };
                        else
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "ErrorPage" }, { "action", "Index" }, { "area", "" } });
                    }
                }
                else
                {
                    if (filterContext.IsChildAction || filterContext.HttpContext.Request.IsAjaxRequest())
                        filterContext.Result = new ContentResult { Content = "" };
                    else
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "User" }, { "action", "Login" } });
                }
            }
        }

        public static bool AccessRight(string ActionName, string ControlerName, string AreaName)
        {
            string sControlerName = ControlerName != null ? ControlerName.ToLower() : "";
            string sActionName = ActionName != null ? ActionName.ToLower() : "";
            string sAreaName = string.IsNullOrEmpty(AreaName) == false ? AreaName.ToLower() : "home";

            if (sControlerName == "" || sActionName == "")
                return false;

            //Coi thử đăng nhập chưa
            if (WebSecurity.IsAuthenticated)
            {
                //Kiểm tra truy cập được khai báo trong file ==> App_Data/ActionExcepted.xml
                ExceptionActionsModel exceptionActionsModel = ExceptionActionHelper.ReadExceptionActions();
                foreach (ExceptionActionModel item in exceptionActionsModel.ExceptionActions)
                {
                    if (item.ActionName.ToLower() == sActionName && (string.IsNullOrEmpty(item.ControllerName) || item.ControllerName.ToLower() == sControlerName)
                        && (string.IsNullOrEmpty(item.AreaName) || item.AreaName.ToLower() == sAreaName))
                    {
                        return true;
                    }
                }

                //Tiếp tục kiểm tra trong bảng phân quyền của hệ thống
                List<vwPage> pagesAccessRight = CacheHelper.PagesAccessRight;
                var ItemPagesAccessRight = pagesAccessRight.Where(x => x.ActionName.ToLowerOrEmpty() == sActionName.ToLower() && x.ControllerName.ToLowerOrEmpty() == sControlerName.ToLower() && (x.AreaName != null ? x.AreaName.ToLowerOrEmpty() == sAreaName.ToLower() : true)).FirstOrDefault();
                if (ItemPagesAccessRight == null)
                {
                    //Không có quyền
                    return false;
                }

                return true;
            }
            else
                return false;
        }

        private void initMenu(ActionExecutingContext filterContext, string sControlerName, string sActionName)
        {
            string url = "/" + sControlerName + "/" + sActionName;

            var listMenu = getMenu(url, null);

            activeMenu(listMenu, listMenu);

            filterContext.Controller.ViewData["LayoutMenu"] = listMenu;
        }
        void activeMenu(List<MenuViewModel> listMenuFull, List<MenuViewModel> listMenu)
        {
            foreach (var item in listMenu)
            {
                if (item.ListMenu.Any(x => x.IsActived))
                {
                    item.IsActived = true;
                    activeParentMenu(listMenuFull, item.ParentId);
                }
                else
                {
                    if (item.ListMenu.Count > 0)
                    {
                        activeMenu(listMenuFull, item.ListMenu);
                    }
                }
            }
        }

        private void activeParentMenu(List<MenuViewModel> listMenu, int? parentId)
        {
            if (parentId != null)
            {
                var parent = listMenu.Where(item => item.Id == parentId).FirstOrDefault();
                if (parent != null)
                {
                    parent.IsActived = true;
                    activeParentMenu(listMenu, parent.ParentId);
                }
            }
        }

        private List<MenuViewModel> getMenu(string url, int? parentId)
        {
            var q = CacheHelper.PagesMenu.Where(x => x.ParentId == parentId)
                .Select(x => new MenuViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Url = x.PageUrl ?? x.Url,
                    CssClassIcon = x.CssClassIcon,
                    ParentId = x.ParentId,
                    OrderNo = x.OrderNo ?? 0,
                    IsActived = x.PageUrl != null && x.PageUrl.ToLower() == url,
                    ListMenu = getMenu(url, x.Id),
                    //IsNewTab = x.IsNewTab
                })
                .OrderBy(x => x.OrderNo)
                .ToList();
            return q;
        }

        public static bool IsAdmin()
        {
            if (Helpers.Common.CurrentUser.UserTypeId == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}