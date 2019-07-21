using System.Data.Entity.Infrastructure;
using System.Globalization;
using Erp.BackOffice.Areas.Administration.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Interfaces;
using Erp.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Text;
using System;

namespace Erp.BackOffice.Administration.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class AccessRightController : Controller
    {
        private readonly IUserPageRepository _userPageRepository;
        private readonly IUserTypePageRepository _userTypePageRepository;
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISettingLanguageRepository _languageRepository;

        public AccessRightController(IUserTypePageRepository userTypePageRepository, IUserTypeRepository userType, IPageRepository page, IUserRepository userRepository, IUserPageRepository userPageRepository, ISettingLanguageRepository languageRepository)
        {
            _userTypePageRepository = userTypePageRepository;
            _pageRepository = page;
            _userTypeRepository = userType;
            _userRepository = userRepository;
            _userPageRepository = userPageRepository;
            _languageRepository = languageRepository;
        }

        #region Create
        public ActionResult Create(int? userTypeId, int? userId)
        {
            var model = new AccessRightViewModel();
            //var listusertypes = _userTypeRepository.GetUserTypes().Where(item => item.Id != 1).OrderBy(item => item.Name).ToList();
            //model.ListUserTypes = new List<UserTypeViewModel>();
            //AutoMapper.Mapper.Map(listusertypes, model.ListUserTypes);

            //foreach (var item in model.ListUserTypes)
            //{
            model.ListUserType = _userTypeRepository.GetUserTypes().Where(i => i.Code != "admin").ToList();
            //}
            model.ListUser = _userRepository.GetAllvwUsers().Where(i => i.UserTypeCode != "admin").ToList();
            model.UserTypeId = userTypeId;
            model.UserId = userId;
            if (model.UserTypeId != null)
            {
                if (model.UserId != null)
                {
                    model.ListAccessRight = _pageRepository.GetPagesAccessRight(model.UserId.Value, model.UserTypeId.Value).Select(item => item.Id).ToList();
                }
                else
                {
                    model.ListAccessRight = _pageRepository.GetPagesUserTypeAccessRight(model.UserTypeId.Value).Select(item => item.Id).ToList();
                }

                var list = _pageRepository.GetPages().Where(item => item.IsVisible.Value).ToList();
                //var userType = _userTypeRepository.GetUserTypeById(Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId);

                model.ListPageGroup = list.Where(item => item.ParentActionId == null).GroupBy(item => item.GroupName)
                    .Select(g1 => new PageGroupViewModel
                    {
                        Id = 0,
                        Name = g1.Key,
                        ListPageGroup = g1.GroupBy(item => item.ControllerDisplayName).Select(g2 => new PageGroupViewModel
                        {
                            Id = 0,
                            Name = g2.Key,
                            ListPageGroup = g2.Select(g3 => new PageGroupViewModel
                            {
                                Id = g3.Id,
                                Name = g3.Name,
                                Desc = g3.ActionName,
                                Selected = model.ListAccessRight.Contains(g3.Id),
                                Visible = g3.IsVisible == null ? false : g3.IsVisible.Value,
                                ListSubAction = list.Where(x => x.ParentActionId == g3.Id).Select(x => new PageGroupViewModel
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Desc = x.ActionName,
                                    Selected = model.ListAccessRight.Contains(x.Id),
                                    Visible = x.IsVisible == null ? false : x.IsVisible.Value
                                }).OrderBy(item => item.Name).ToList()
                            }).OrderBy(item => item.Name).ToList()
                        }).OrderBy(item => item.Name).ToList()
                    })
                    .OrderBy(item => item.Name)
                    .ToList();
            }

            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(AccessRightCreateModel model)
        {
            if (model.UserTypeId > 0 && model.ListPages != null)
            {
                //Reset cache for pagerightaccess & pagemenu
                Erp.BackOffice.Helpers.CacheHelper.PagesAccessRight = null;
                Erp.BackOffice.Helpers.CacheHelper.PagesMenu = null;

                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {
                        var ListPages = new System.Data.DataTable();
                        ListPages.Columns.Add("Id", typeof(int));
                        for (int i = 0; i < model.ListPages.Count(); i++)
                        {
                            ListPages.Rows.Add(model.ListPages[i]);
                        }

                        _userTypePageRepository.DeleteByUserTypeId(model.UserTypeId, model.UserId);
                        //var user = _userRepository.GetUserById(model.UserId);
                        _userTypePageRepository.InsertAll(model.UserTypeId, model.UserId, ListPages);

                        //if (model.UserId <= 0)
                        //{
                        //    //cập nhật vào phân quyền mẫu
                        //    userTypeDetailRepository.DeleteByUserTypeId(model.UserTypeId);
                        //    userTypeDetailRepository.InsertAll(model.UserTypeId, ListPages);
                        //}
                        //else
                        //{
                        //    _userTypePageRepository.DeleteByUserTypeId(model.UserTypeId, model.UserId);
                        //    var user = _userRepository.GetUserById(model.UserId);
                        //    _userTypePageRepository.InsertAll(model.UserTypeId, user.Id, ListPages);

                        //}
                        scope.Complete();
                    }
                    catch (DbUpdateException)
                    {
                        return Content("fail");
                    }
                }
            }

            return Content("success");
        }
        #endregion

        #region Edit
        public ActionResult Edit()
        {
            var list = _pageRepository.GetPages().ToList();

            var ListPageGroup = list.Where(item => item.ParentActionId == null).GroupBy(item => item.GroupName)
                .Select(g1 => new PageGroupViewModel
                {
                    Id = 0,
                    Name = g1.Key,
                    ListPageGroup = g1.GroupBy(item => item.ControllerDisplayName).Select(g2 => new PageGroupViewModel
                    {
                        Id = 0,
                        Name = g2.Key,
                        ListPageGroup = g2.Select(g3 => new PageGroupViewModel
                        {
                            Id = g3.Id,
                            Name = g3.Name,
                            Desc = g3.ControllerName + " / " + g3.ActionName,
                            Visible = g3.IsVisible == null ? false : g3.IsVisible.Value,
                            CountSubAction = list.Where(x => x.ParentActionId == g3.Id).Count()
                        }).OrderBy(item => item.Name).ToList()
                    }).OrderBy(item => item.Name).ToList()
                })
                .OrderBy(item => item.Name)
                .ToList();

            return View(ListPageGroup);
        }
        #endregion
    }
}
