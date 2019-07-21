using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class PlanningOfProductionDailyController : Controller
    {
        private readonly IPlanningOfProductionDailyRepository PlanningOfProductionDailyRepository;
        private readonly IPlanningOfProductionRepository planningOfProductionRepository;
        private readonly IPlanningOfProductionDetailRepository planningOfProductionDetailRepository;
        private readonly IUserRepository userRepository;

        public PlanningOfProductionDailyController(
            IPlanningOfProductionDailyRepository _PlanningOfProductionDaily
            , IUserRepository _user
            , IPlanningOfProductionRepository _PlanningOfProduction
            , IPlanningOfProductionDetailRepository _PlanningOfProductionDetail
            )
        {
            PlanningOfProductionDailyRepository = _PlanningOfProductionDaily;
            planningOfProductionRepository = _PlanningOfProduction;
            planningOfProductionDetailRepository = _PlanningOfProductionDetail;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {

            IQueryable<PlanningOfProductionDailyViewModel> q = PlanningOfProductionDailyRepository.GetAllPlanningOfProductionDaily()
                .Select(item => new PlanningOfProductionDailyViewModel
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
        #endregion

        #region Create
        public ViewResult Create(int Id)
        {
            //Id: Id của PlanningOfProduction
            // var modelPlan = planningOfProductionRepository.GetPlanningOfProductionById(Id);
            var modelPlanDetail = planningOfProductionDetailRepository.GetAllvwPlanningOfProductionDetail().Where(n => n.PlanningOfProductionId == Id).ToList();


            var model = new PlanningOfProductionDailyViewModel();
            // model.Planning = modelPlan;
            model.PlanningDetail = modelPlanDetail;
            model.PlanningOfProductionId = Id;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PlanningOfProductionDailyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DailyList != null && model.DailyList.Count > 0)
                {
                    for (int i = 0; i < model.DailyList.Count; i++)
                    {
                        model.DailyList[i].IsDeleted = false;
                        model.DailyList[i].CreatedUserId = WebSecurity.CurrentUserId;
                        model.DailyList[i].ModifiedUserId = WebSecurity.CurrentUserId;
                        model.DailyList[i].AssignedUserId = WebSecurity.CurrentUserId;
                        model.DailyList[i].CreatedDate = DateTime.Now;
                        model.DailyList[i].ModifiedDate = DateTime.Now;
                        PlanningOfProductionDailyRepository.InsertPlanningOfProductionDaily(model.DailyList[i]);
                    }
                }

                //update lại thành trạng thái đang sản xuất
                var plan = planningOfProductionRepository.GetPlanningOfProductionById(model.PlanningOfProductionId);
                plan.Status = PlanningOfProductionViewModel.DANGSANXUAT;
                planningOfProductionRepository.UpdatePlanningOfProduction(plan);


                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Building", "PlanningOfProduction", new { area = "Sale", idbc = plan.Id, status = plan.Status });
            }
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id, string model)
        {
            ViewBag.Edit = !string.IsNullOrEmpty(model) && model == "Edit" ? true : false;
            ViewBag.Id = Id;
            var planDetails = planningOfProductionDetailRepository.GetAllvwPlanningOfProductionDetail().Where(n => n.PlanningOfProductionId == Id).ToList();
            var ids = planDetails.Select(n => n.Id).ToList();
            //Tìm những chi tiết trong daily
            var planDailys = PlanningOfProductionDailyRepository.GetAllPlanningOfProductionDaily().ToList();
            //var PlanningOfProductionDaily = PlanningOfProductionDailyRepository.GetPlanningOfProductionDailyById(Id.Value);
            var q = new List<PlanDailyOfPlan>();
            for (int i = 0; i < planDetails.Count; i++)
            {
                var _model = new PlanDailyOfPlan();
                _model.Id = Id.Value;
                _model.ProductCode = planDetails[i].ProdutCode;
                _model.ProductName = planDetails[i].ProdutName;
                _model.QuantityPlan = planDetails[i].QuantityPlan;
                _model.FromDate = planDetails[i].FromDate;
                _model.ToDate = planDetails[i].ToDate;
                var daily = planDailys.Where(n => n.PlanningOfProductionDetailId == planDetails[i].Id).ToList();
                _model.PlanningDaily = daily;

                q.Add(_model);
            }
            //model.PlanningDetail = planDetails;
            //model.PlanDaily = planDailys; 


            return View(q);
        }

        [HttpPost]
        public ActionResult Edit(PlanningOfProductionDailyViewModel model)
        {
            int planID = int.Parse(Request["PlanningOfProductionId"].ToString());
            if (ModelState.IsValid && planID!=-1)
            {
                
                if (Request["Submit"] == "Save")
                {
                    for (int i = 0; i < model.DailyList.Count; i++)
                    {
                        var PlanningOfProductionDaily = PlanningOfProductionDailyRepository.GetPlanningOfProductionDailyById(model.DailyList[i].Id);

                        PlanningOfProductionDaily.ModifiedUserId = WebSecurity.CurrentUserId;
                        PlanningOfProductionDaily.ModifiedDate = DateTime.Now;
                        PlanningOfProductionDaily.QuantityReality = model.DailyList[i].QuantityReality;
                        PlanningOfProductionDaily.MachineReality = model.DailyList[i].MachineReality;
                        PlanningOfProductionDailyRepository.UpdatePlanningOfProductionDaily(PlanningOfProductionDaily);
                        
                    }
                    return RedirectToAction("Building", "PlanningOfProduction", new { area = "Sale", idbc = planID, status = PlanningOfProductionViewModel.HOANTHANH });

                }
                else if (Request["Submit"] == "Update")
                {
                    //Clear all by DetailId | PlanningOfProduction
                    //int planID = int.Parse(Request["PlanningOfProduction"].ToString());
                    var planDetails = planningOfProductionDetailRepository.GetAllvwPlanningOfProductionDetail().Where(n => n.PlanningOfProductionId == planID).ToList();
                    var ids = planDetails.Select(n => n.Id).ToList();

                    //Delete ALL cập nhật lại Daily
                    for (int i = 0; i < ids.Count; i++)
                    {
                        PlanningOfProductionDailyRepository.DeletelanningOfProductionDailyByDetailId(ids[i]);
                    }

                    //Insert lại
                    if (model.DailyList != null && model.DailyList.Count > 0)
                    {
                        for (int i = 0; i < model.DailyList.Count; i++)
                        {
                            model.DailyList[i].IsDeleted = false;
                            model.DailyList[i].CreatedUserId = WebSecurity.CurrentUserId;
                            model.DailyList[i].ModifiedUserId = WebSecurity.CurrentUserId;
                            model.DailyList[i].AssignedUserId = WebSecurity.CurrentUserId;
                            model.DailyList[i].CreatedDate = DateTime.Now;
                            model.DailyList[i].ModifiedDate = DateTime.Now;
                            PlanningOfProductionDailyRepository.InsertPlanningOfProductionDaily(model.DailyList[i]);
                        }
                    }
                    return RedirectToAction("Building", "PlanningOfProduction", new { area = "Sale", idbc = planID, status = PlanningOfProductionViewModel.DANGSANXUAT});
                }
            }

            return RedirectToAction("Building", "PlanningOfProduction", new { area = "Sale", idbc = planID, status = PlanningOfProductionViewModel.DANGSANXUAT });

            //if (Request.UrlReferrer != null)
            //    return Redirect(Request.UrlReferrer.AbsoluteUri);
            //return RedirectToAction("Index");
        }

        #endregion

        #region Detail
        public ActionResult Detail(int? Id)
        {
            var PlanningOfProductionDaily = PlanningOfProductionDailyRepository.GetPlanningOfProductionDailyById(Id.Value);
            if (PlanningOfProductionDaily != null && PlanningOfProductionDaily.IsDeleted != true)
            {
                var model = new PlanningOfProductionDailyViewModel();
                AutoMapper.Mapper.Map(PlanningOfProductionDaily, model);

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
                    var item = PlanningOfProductionDailyRepository.GetPlanningOfProductionDailyById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }

                        item.IsDeleted = true;
                        PlanningOfProductionDailyRepository.UpdatePlanningOfProductionDaily(item);
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

        #region Update reality

        public ViewResult UpdateReality(int Id)
        {
            var modelPlanDetail = planningOfProductionDetailRepository.GetAllvwPlanningOfProductionDetail().Where(n => n.PlanningOfProductionId == Id).ToList();

            var model = new PlanningOfProductionDailyViewModel();
            // model.Planning = modelPlan;
            model.PlanningDetail = modelPlanDetail;
            model.PlanningOfProductionId = Id;
            return View(model);
        }
        #endregion

        #region Report
        public ActionResult DetailEstimates(int? Id)
        {
            //ViewBag.Edit = !string.IsNullOrEmpty(model) && model == "Edit" ? true : false;
            ViewBag.Id = Id;
            var planDetails = planningOfProductionDetailRepository.GetAllvwPlanningOfProductionDetail().Where(n => n.PlanningOfProductionId == Id).ToList();
            var ids = planDetails.Select(n => n.Id).ToList();
            //Tìm những chi tiết trong daily
            var planDailys = PlanningOfProductionDailyRepository.GetAllPlanningOfProductionDaily().ToList();
            //var PlanningOfProductionDaily = PlanningOfProductionDailyRepository.GetPlanningOfProductionDailyById(Id.Value);
            var q = new List<PlanDailyOfPlan>();
            for (int i = 0; i < planDetails.Count; i++)
            {
                var _model = new PlanDailyOfPlan();
                _model.Id = Id.Value;
                _model.ProductCode = planDetails[i].ProdutCode;
                _model.ProductName = planDetails[i].ProdutName;
                _model.QuantityPlan = planDetails[i].QuantityPlan;
                _model.FromDate = planDetails[i].FromDate;
                _model.ToDate = planDetails[i].ToDate;
                _model.ProductGroup = planDetails[i].ProductGroup;
                _model.QuantityOrder = planDetails[i].QuantityOrder.Value;
                _model.QuantityTotalInventory = planDetails[i].QuantityTotalInventory;

                var daily = planDailys.Where(n => n.PlanningOfProductionDetailId == planDetails[i].Id).ToList();
                _model.PlanningDaily = daily;

                q.Add(_model);
            }
            //model.PlanningDetail = planDetails;
            //model.PlanDaily = planDailys; 


            return View(q);
        }
        #endregion
    }
}
