using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;
using Erp.BackOffice.Controllers;
using qts.webapp.backend.domain.Services.Sale;
using qts.webapp.backend.domain.Models.Sale;
using Erp.Domain.Account.Interfaces;
using Erp.Domain.Account.Helper;

namespace Erp.BackOffice.Sale.Controllers
{
    public class MemberCardController : BaseController
    {
        private readonly IMemberCardService MemberCardService;
        private readonly IvwMemberCardService vwMemberCardService;
        private readonly IMemberCardTypeService MemberCardTypeService;
        private readonly IUserRepository userRepository;
        private readonly ICustomerRepository CustomerRepository;

        public MemberCardController(
            IMemberCardService _MemberCard
            , IvwMemberCardService _vwMemberCard
            , IMemberCardTypeService _MemberCardTypeService
            , IUserRepository _user
            , ICustomerRepository _CustomerRepository
            )
        {
            MemberCardService = _MemberCard;
            vwMemberCardService = _vwMemberCard;
            MemberCardTypeService = _MemberCardTypeService;
            userRepository = _user;
            CustomerRepository = _CustomerRepository;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {
            IEnumerable<MemberCardViewModel> q = MemberCardService.Get()
                .Select(item => new MemberCardViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate
                }).OrderByDescending(m => m.ModifiedDate);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create(int CustomerId)
        {
            var model = new MemberCardViewModel();
            model.CustomerId = CustomerId;
            model.ListMemberCardType = MemberCardTypeService.Get()
                .Select(item => new MemberCardTypeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image
                })
                .OrderBy(item => item.Name)
                .ToList();
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MemberCardViewModel model, bool? IsInvoice)
        {
            if (ModelState.IsValid)
            {
                var MemberCard = MemberCardService.GetMemberCardByCode(model.Code);
                if (MemberCard != null)
                {
                    TempData[Globals.FailedMessageKey] = "Mã thẻ đã được cấp";
                    return RedirectToAction("Create", new { CustomerId = model.CustomerId, IsPopup = true, IsInvoice = IsInvoice });
                }
                var customer = CustomerRepository.GetCustomerById(model.CustomerId.Value);
                var MemberCardType = MemberCardTypeService.MemberCardTypeGetById(model.MemberCardTypeId.Value);
                ProductOutboundController.CreateForMemberCard(TempData, MemberCardType.CardId, Helpers.Common.CurrentUser.BranchId, customer.Name, customer.Code);
                if (TempData[Globals.FailedMessageKey] != null)
                {
                    return RedirectToAction("Create", new { CustomerId = model.CustomerId, IsPopup = true, IsInvoice = IsInvoice });
                }

                if (customer.MemberCardId != null)
                {
                    var memberCard_Before = MemberCardService.GetMemberCardById(customer.MemberCardId.Value);
                    if(memberCard_Before != null)
                    {
                        memberCard_Before.Status = MemberCardViewModel.Inactive;
                        MemberCardService.Update(memberCard_Before);
                    }
                }

                MemberCard = new MemberCard();
                AutoMapper.Mapper.Map(model, MemberCard);
                SetModifier(MemberCard);
                MemberCard.DateOfIssue = DateTime.Now;
                MemberCard.Status = MemberCardViewModel.Active;
                MemberCardService.Create(MemberCard);
                //Thay đổi thẻ cho khách hàng
                customer.MemberCardId = MemberCard.Id;
                CustomerRepository.UpdateCustomer(customer);

                TempData[Globals.SuccessMessageKey] += App_GlobalResources.Wording.InsertSuccess;
               if(IsInvoice == true)
                {
                    ViewBag.closePopup = "close Popup";
                    model.ListMemberCardType = MemberCardTypeService.Get()
                                            .Select(item => new MemberCardTypeViewModel
                                            {
                                                Id = item.Id,
                                                Name = item.Name,
                                                Image = item.Image
                                            })
                                            .OrderBy(item => item.Name)
                                            .ToList();
                    return View(model);
                }
                if (Request["IsPopup"] != null && Request["IsPopup"].ToString().ToLower().Equals("true"))
                {
                    return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var MemberCard = MemberCardService.Get(Id.Value);
            if (MemberCard != null && MemberCard.IsDeleted != true)
            {
                var model = new MemberCardViewModel();
                AutoMapper.Mapper.Map(MemberCard, model);
                model.ListMemberCardType = MemberCardTypeService.Get()
                  .Select(item => new MemberCardTypeViewModel
                  {
                      Id = item.Id,
                      Name = item.Name,
                      Image = item.Image
                  })
                  .OrderBy(item => item.Name)
                  .ToList();
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.FailedMessage = TempData["FailedMessage"];
                ViewBag.AlertMessage = TempData["AlertMessage"];
                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(MemberCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var MemberCard = MemberCardService.Get(model.Id);
                    var MemberCard_Check = MemberCardService.GetAnyCode(MemberCard.Code, model.Code);
                    if (MemberCard_Check)
                    {
                        TempData[Globals.FailedMessageKey] = "Mã thẻ đã được cấp";
                        return RedirectToAction("Edit", new { Id = model.Id, IsPopup = true });
                    }
                    AutoMapper.Mapper.Map(model, MemberCard);
                    SetModifier(MemberCard, true);
                    MemberCardService.Update(MemberCard);
                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    if (Request["IsPopup"] != null && Request["IsPopup"].ToString().ToLower().Equals("true"))
                    {
                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                    }
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
            var MemberCard = vwMemberCardService.Get(Id.Value);
            if (MemberCard != null && MemberCard.IsDeleted != true)
            {
                var model = new MemberCardViewModel();
                AutoMapper.Mapper.Map(MemberCard, model);
                model.ListMemberCard = vwMemberCardService.GetListMemberCardByCustomerId(MemberCard.CustomerId.Value)
                   .Select(item => new MemberCardViewModel
                   {
                       Id = item.Id,
                       Code = item.Code,
                       DateOfIssue = item.DateOfIssue,
                       MemberCardTypeImage = item.MemberCardTypeImage
                   })
                   .OrderByDescending(item => item.DateOfIssue)
                   .ToList();

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
                    var item = MemberCardService.Get(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }
                        MemberCardService.DeleteRs(item);
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

        #region Shynch Update Type Card
        public static string SynchUpdateTypeCrad(int customerId, double? accumulated, double? used, int? frequency, bool addition = true)
        {
            ICustomerRepository CustomerRepository = DependencyResolver.Current.GetService<ICustomerRepository>();
            IMemberCardService memberCardService = DependencyResolver.Current.GetService<IMemberCardService>();
            IMemberCardTypeService memberCardTypeService = DependencyResolver.Current.GetService<IMemberCardTypeService>();
            var customer = CustomerRepository.GetCustomerById(customerId);
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            if (customer != null )
            {
                if (addition)
                {
                    customer.Point += accumulated;
                    customer.PaidPoint += used;
                    customer.RemainingPoint = customer.Point - customer.PaidPoint;
                }
                else
                {
                    customer.Point -= accumulated;
                    customer.PaidPoint -= used;
                    customer.RemainingPoint = customer.Point - customer.PaidPoint;
                }
                if(frequency != null)
                {
                    customer.Frequency = frequency;
                    customer.EndDateProductInvoice = aDateTime;
                }
                CustomerRepository.UpdateCustomer(customer);
                if(customer.MemberCardId != null)
                {
                    var memberCard = memberCardService.Get(customer.MemberCardId.Value);
                    var memberCardType = memberCardTypeService.GetByPoint(customer.Point);
                    if (memberCardType != null)
                    {
                        memberCard.MemberCardTypeId = memberCardType.Id;
                        memberCardService.Update(memberCard);
                    }
                }
            }
            return null;
        }
        #endregion

        #region SynchPoint
        public static string SynchPoint(int customerId)
        {
            IMemberCardService memberCardService = DependencyResolver.Current.GetService<IMemberCardService>();
            IMemberCardTypeService memberCardTypeService = DependencyResolver.Current.GetService<IMemberCardTypeService>();
            var points = SqlHelper.QuerySP<HistoryPointViewModel>("spSale_HistoryPoint", new
            {
                CustomerId = customerId
            });
            if (points != null && points.Count() > 0)
            {
                var point = points.FirstOrDefault();
                if (point.MemberCardId != null)
                {
                    var memberCard = memberCardService.Get(point.MemberCardId.Value);
                    var memberCardType = memberCardTypeService.GetByPoint(point.AccumulatedPoint);
                    if (memberCardType != null)
                    {
                        memberCard.MemberCardTypeId = memberCardType.Id;
                        memberCardService.Update(memberCard);
                    }
                }
            }
            return null;
        }
        #endregion
       
    }
}
